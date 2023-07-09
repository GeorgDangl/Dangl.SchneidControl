using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Utilities.Collections;
using System;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.Docker.DockerTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion(Framework = "net7.0")] GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    [Solution] readonly Solution Solution;

    readonly string DockerImageName = "dangl-schneid-control";

    [Parameter] readonly string DockerRegistryUrl;
    [Parameter] readonly string DockerRegistryUsername;
    [Parameter] readonly string DockerRegistryPassword;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(d => d.DeleteDirectory());
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target GenerateVersion => _ => _
        .Executes(() =>
        {
            var buildDate = DateTime.UtcNow;

            var filePath = SourceDirectory / "Dangl.SchneidControl" / "VersionsService.cs";

            var currentDateUtc = $"new DateTime({buildDate.Year}, {buildDate.Month}, {buildDate.Day}, {buildDate.Hour}, {buildDate.Minute}, {buildDate.Second}, DateTimeKind.Utc)";

            var content = $@"using System;

namespace Dangl.SchneidControl
{{
    // This file is automatically generated
    [System.CodeDom.Compiler.GeneratedCode(""GitVersionBuild"", """")]
    public static class VersionsService
    {{
        public static string Version => ""{GitVersion.NuGetVersionV2}"";
        public static string CommitInfo => ""{GitVersion.FullBuildMetaData}"";
        public static string CommitDate => ""{GitVersion.CommitDate}"";
        public static string CommitHash => ""{GitVersion.Sha}"";
        public static string InformationalVersion => ""{GitVersion.InformationalVersion}"";
        public static DateTime BuildDateUtc {{ get; }} = {currentDateUtc};
    }}
}}";
            filePath.WriteAllText(content);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProcessArgumentConfigurator(a => a.Add("/nodeReuse:false"))
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(GenerateVersion)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProcessArgumentConfigurator(a => a.Add("/nodeReuse:false"))
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target BuildFrontendSwaggerClient => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var nSwagConfigPath = SourceDirectory / "dangl-schneid-control" / "src" / "nswag.json";
            var nSwagToolPath = NuGetToolPathResolver.GetPackageExecutable("NSwag.MSBuild", "tools/Net70/dotnet-nswag.dll");
            DotNet($"{nSwagToolPath} run /Input:\"{nSwagConfigPath}\"", SourceDirectory / "dangl-schneid-control" / "src");
        });

    Target FrontEndRestore => _ => _
        .After(Clean)
        .Executes(() =>
        {
            (SourceDirectory / "dangl-schneid-control" / "node_modules").CreateOrCleanDirectory();
            (SourceDirectory / "dangl-schneid-control" / "node_modules").DeleteDirectory();
            Npm("ci", SourceDirectory / "dangl-schneid-control");
        });

    Target BuildFrontend => _ => _
        .DependsOn(BuildFrontendSwaggerClient)
        .DependsOn(GenerateVersion)
        .DependsOn(FrontEndRestore)
        .Executes(() =>
        {
            (SourceDirectory / "Dangl.SchneidControl" / "wwwroot" / "dist").CreateOrCleanDirectory();

            NpmRun(x => x
                .SetCommand("build:prod")
                .SetProcessWorkingDirectory(SourceDirectory / "dangl-schneid-control"));
        });

    Target BuildDocker => _ => _
        .DependsOn(Restore)
        .DependsOn(GenerateVersion)
        .DependsOn(BuildFrontend)
        .Requires(() => Configuration == "Release")
        .Executes(() =>
        {
            DotNetPublish(s => s
                .SetProcessArgumentConfigurator(a => a.Add("/nodeReuse:false"))
                .SetProject(SourceDirectory / "Dangl.SchneidControl")
                .SetOutput(OutputDirectory)
                .SetConfiguration(Configuration));

            foreach (var configFileToDelete in OutputDirectory.GlobFiles("web*.config"))
            {
                configFileToDelete.DeleteFile();
            }

            CopyFile(SourceDirectory / "Dangl.SchneidControl" / "Dockerfile", OutputDirectory / "Dockerfile", FileExistsPolicy.Overwrite);

            DockerBuild(c => c
                .SetFile(OutputDirectory / "Dockerfile")
                .SetTag(DockerImageName + ":dev")
                .SetPath(".")
                .SetPull(true)
                .SetProcessWorkingDirectory(OutputDirectory));

            OutputDirectory.CreateOrCleanDirectory();
        });

    Target PushDocker => _ => _
        .DependsOn(BuildDocker)
        .Requires(() => DockerRegistryUrl)
        .Requires(() => DockerRegistryUsername)
        .Requires(() => DockerRegistryPassword)
        .OnlyWhenDynamic(() => IsOnBranch("main") || IsOnBranch("develop"))
        .Executes(() =>
        {
            DockerLogin(x => x
                .SetUsername(DockerRegistryUsername)
                .SetServer(DockerRegistryUrl.ToLowerInvariant())
                .SetPassword(DockerRegistryPassword)
                .DisableProcessLogOutput());

            PushDockerWithTag("dev", DockerRegistryUrl, DockerImageName);

            if (IsOnBranch("main"))
            {
                PushDockerWithTag("latest", DockerRegistryUrl, DockerImageName);
                PushDockerWithTag(GitVersion.SemVer, DockerRegistryUrl, DockerImageName);
            }
        });

    private void PushDockerWithTag(string tag, string dockerRegistryUrl, string targetDockerImageName)
    {
        DockerTag(c => c
            .SetSourceImage(DockerImageName + ":" + "dev")
            .SetTargetImage($"{dockerRegistryUrl}/{targetDockerImageName}:{tag}".ToLowerInvariant()));
        DockerPush(c => c
            .SetName($"{dockerRegistryUrl}/{targetDockerImageName}:{tag}".ToLowerInvariant()));
    }

    private bool IsOnBranch(string branchName)
    {
        return GitVersion.BranchName.Equals(branchName) || GitVersion.BranchName.Equals($"origin/{branchName}");
    }
}
