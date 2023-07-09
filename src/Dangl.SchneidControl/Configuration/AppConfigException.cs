namespace Dangl.SchneidControl.Configuration
{
    public partial class SchneidControlSettings
    {
        public class AppConfigException : Exception
        {
            public AppConfigException() : base()
            {
            }

            public AppConfigException(string? message) : base(message)
            {
            }

            public AppConfigException(string? message, Exception? innerException) : base(message, innerException)
            {
            }
        }
    }
}
