using IcukBroadbandCheckerCs.Styles;
using Scriban;
using System.Reflection;
using System.Web;

namespace IcukBroadbandCheckerCs {
    public class BroadbandAvailabilityChecker
    {
        public static Random random = new Random();
        public string Id = GenerateId();

        private static string LoadFile(string path) {
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(basePath, path);
            return File.ReadAllText(filePath);
        }

        public static string GenerateId() {
            byte[] buffer = new byte[6];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            return result;
        }

        public string RenderSearch(string apiPath, string error = "Error") {
            Template template = Template.Parse(LoadFile("Templates/Search.sbn"));
            return template.Render(new {
                BroadbandAvailabilityId = Id,
                BroadbandAvailabilityErrorMessage = HttpUtility.HtmlEncode(error),
                BroadbandAvailabilityPath = HttpUtility.JavaScriptStringEncode(apiPath)
            });
        }

        public string RenderAddressSelect() {
            Template template = Template.Parse(LoadFile("Templates/AddressSelect.sbn"));
            return template.Render(new { BroadbandAvailabilityId = Id });
        }

        public string RenderResults() {
            Template template = Template.Parse(LoadFile("Templates/Results.sbn"));
            return template.Render(new { BroadbandAvailabilityId = Id });
        }

        public string RenderStyles(SearchStyleSettings sstyle, ResultsStyleSettings rstyle, AddressSelectStyleSettings astyle) {
            Template template = Template.Parse(LoadFile("Templates/Styles.sbn"));
            return template.Render(new { AddressStyle = astyle, ResultsStyle = rstyle, SearchStyle = sstyle });
        }

        public string RenderScripts(bool minified = false) {
            Template template = Template.Parse(LoadFile($"Templates/Scripts{(minified ? "Minified" : "")}.sbn"));
            return template.Render();
        }
    }
}
