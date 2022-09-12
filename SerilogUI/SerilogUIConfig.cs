namespace SerilogUI
{
    public class SerilogUIConfig
    {
        private string _logPath { get; set; } = "Logs";
        private string _urlPath { get; set; } = "SerilogUI";
        public string LogPath => _logPath;
        public string UrlPath => _urlPath;

        public void SetLogDirectory(string path) => _logPath = path;
        public void SetPath(string path) => _urlPath = path;
    }
}