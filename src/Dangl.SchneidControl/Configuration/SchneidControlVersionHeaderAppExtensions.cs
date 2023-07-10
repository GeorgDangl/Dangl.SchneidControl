using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dangl.SchneidControl.Configuration
{
    /// <summary>
    /// Extensions for the SchneidControl app pipeline
    /// </summary>
    public static class SchneidControlVersionHeaderAppExtensions
    {
        /// <summary>
        /// This appends the X-DANGL-SCHNEID-CONTROL-VERSION header to all responses
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSchneidControlVersionHeader(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context?.Response.Headers.TryAdd("X-DANGL-SCHNEID-CONTROL-VERSION", VersionsService.Version);
                await next();
            });

            return app;
        }
    }
}
