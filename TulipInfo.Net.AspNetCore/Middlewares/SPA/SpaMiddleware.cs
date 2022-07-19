using Microsoft.Extensions.Options;
using System.Globalization;

namespace TulipInfo.Net.AspNetCore
{
    public class SpaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;
        private readonly SpaMiddlewareOptions _options;

        public SpaMiddleware(RequestDelegate next,
            ILogger<SpaMiddleware> logger,
            IWebHostEnvironment env, 
            SpaMiddlewareOptions options)
        {
            _next = next;
            _logger = logger;
            _env = env;
            _options = options;
        }

        public Task InvokeAsync(HttpContext context)
        {
            if(context.GetEndpoint() != null)
            {
                return _next(context);
            }

            var req = context.Request;

            if (_options.UseMultipleLanguages)
            {
                HandleMultipleLanguageSPARequest(req);
            }
            else
            {
                HandleSPARequest(req);
            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }

        void HandleSPARequest(HttpRequest req)
        {
            _logger.LogDebug($"Try map {req.Path}");
            string relativePath = req.Path.ToString().Trim('/');
            string fullPath = Path.Combine(_env.WebRootPath, _options.ClientAppFolder, relativePath);
            //check if a static file
            if (File.Exists(fullPath))
            {
                req.Path = $"/{_options.ClientAppFolder}/{relativePath}";
            }
            else
            {
                req.Path = $"/{_options.ClientAppFolder}/index.html";
            }
            _logger.LogDebug($"Mapped to {req.Path}");
        }

        void HandleMultipleLanguageSPARequest(HttpRequest req)
        {
            CultureInfo? culture;
            string relativePathWithoutCalture = "";
            if (TryGetCultureFromPath(req.Path, out culture))
            {
                //the url should start with culture code
                //eg: /en, /en/user-management, /en/static.js .. etc

                relativePathWithoutCalture = req.Path.ToString().Trim('/');
                int idx = relativePathWithoutCalture.IndexOf('/');
                if (idx > 0)
                {
                    relativePathWithoutCalture = relativePathWithoutCalture.Substring(idx + 1);
                }
                else
                {
                    relativePathWithoutCalture = "";
                }
            }
            else
            {
                //that means the url did not contains the culture code
                culture = CultureInfo.CurrentUICulture;
                relativePathWithoutCalture = req.Path.ToString().Trim('/');
            }

            _logger.LogDebug($"Try map {req.Path} to {culture!.Name}/{relativePathWithoutCalture}");

            bool mapped = MapToClientAppFolder(req, culture!.Name, relativePathWithoutCalture);
            if(!mapped)
            {
                mapped = MapToClientAppFolder(req, culture!.TwoLetterISOLanguageName, relativePathWithoutCalture);
            }
            if (!mapped)
            {
                mapped =MapToClientAppFolder(req, _options.DefaultLanguage, relativePathWithoutCalture);
            }
        }

        bool MapToClientAppFolder(HttpRequest req,string cultureName,string relativePath)
        {
            string directory = Path.Combine(_env.WebRootPath, _options.ClientAppFolder, cultureName);
            //check if the culture folder exists
            if (Directory.Exists(directory))
            {
                string fullPath = Path.Combine(directory, relativePath);
                //check if a static file
                if (File.Exists(fullPath))
                {
                    req.Path = $"/{_options.ClientAppFolder}/{cultureName}/{relativePath}";
                }
                else
                {
                    req.Path = $"/{_options.ClientAppFolder}/{cultureName}/index.html";
                }

                _logger.LogDebug($"Mapped to {req.Path}");
                return true;
            }
            return false;
        }

        bool TryGetCultureFromPath(string path, out CultureInfo? culture)
        {
            culture = null;

            if (!string.IsNullOrEmpty(path))
            {
                string cultureCode = "";
                string trimmedPath = path.Trim('/');
                int idx = trimmedPath.IndexOf('/');
                if (idx > 0)
                {
                    cultureCode = trimmedPath.Substring(0, idx);
                }
                else
                {
                    cultureCode = trimmedPath;
                }

                try
                {
                    culture = CultureInfo.GetCultureInfo(cultureCode);
                    return !string.IsNullOrWhiteSpace(culture.Name);
                }
                catch
                {
                    //ignore the invalid culture code
                }
            }

            return false;            
        }

    }
}
