using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Nustache.Core;

namespace Nustache.Mvc
{
    public class NustacheViewEngine : VirtualPathProviderViewEngine
    {
        public NustacheViewEngineRootContext RootContext { get; set; }
        public string[] AdditionalLocations { get; private set; }

        public NustacheViewEngine(string[] fileExtensions = null, string[] additionalLocations = null)
        {
            // If we're using MVC, we probably want to use the same encoder MVC uses.
            Encoders.HtmlEncode = HttpUtility.HtmlEncode;

            FileExtensions = fileExtensions ?? new[] { "mustache" };
            AdditionalLocations = additionalLocations ?? new string[0];
            SetLocationFormats();
            RootContext = NustacheViewEngineRootContext.ViewData;
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return GetView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return GetView(controllerContext, partialPath, null);
        }

        private IView GetView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new NustacheView(this, controllerContext, viewPath, masterPath);
        }

        private void SetLocationFormats()
        {
            var formats = new List<string>();
            var areaFormats = new List<string>();

            foreach (var extension in FileExtensions)
            {
                formats.Add("~/Views/{1}/{0}." + extension);
                formats.Add("~/Views/Shared/{0}." + extension);

                foreach (var additionalLocation in AdditionalLocations)
                {
                    var pathFormats = additionalLocation.EndsWith("*")
                        ? FindSubFolders(additionalLocation, extension)
                        : new[] {additionalLocation + extension};

                    if (pathFormats.Length > 0)
                    {
                        formats.AddRange(pathFormats);
                    }
                }

                areaFormats.Add("~/Areas/{2}/Views/{1}/{0}." + extension);
                areaFormats.Add("~/Areas/{2}/Views/Shared/{0}." + extension);
            }

            MasterLocationFormats = formats.ToArray();
            ViewLocationFormats = MasterLocationFormats;
            PartialViewLocationFormats = MasterLocationFormats;

            AreaMasterLocationFormats = areaFormats.ToArray();
            AreaViewLocationFormats = AreaMasterLocationFormats;
            AreaPartialViewLocationFormats = AreaPartialViewLocationFormats;
        }

        private string[] FindSubFolders(string location, string extension)
        {
            location = location.TrimEnd('*');

            if (VirtualPathProvider.DirectoryExists(location))
            {
                var result = new List<string> {location + "{0}." + extension};

                var dir = VirtualPathProvider.GetDirectory(location);

                foreach (VirtualDirectory subDir in dir.Directories)
                {
                    result.AddRange(FindSubFolders(subDir.VirtualPath, extension));                    
                }

                return result.ToArray();
            }

            return new string[0];
        }
    }

    public enum NustacheViewEngineRootContext
    {
        ViewData,
        Model
    }
}