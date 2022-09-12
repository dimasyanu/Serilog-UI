using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SerilogUI
{
    public class SerilogUIRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;

        public SerilogUIRepository(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        public string GetRawHtml()
        {
            var viewAssembly = GetType().GetTypeInfo().Assembly;
            var fileProvider = new EmbeddedFileProvider(viewAssembly);
            var dirContents = fileProvider.GetDirectoryContents("/");
            var indexView = dirContents.First(x => x.Name == "Views.Index.html");
            string result = "";
            using (var stream = indexView.CreateReadStream()) {
                using var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            return result;
        }

        public string GetAsset(string filename)
        {
            var viewAssembly = GetType().GetTypeInfo().Assembly;
            var fileProvider = new EmbeddedFileProvider(viewAssembly);
            var dirContents = fileProvider.GetDirectoryContents("/");
            var indexView = dirContents.First(x => x.Name == filename);
            string result = "";
            using (var stream = indexView.CreateReadStream()) {
                using var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            return result;
        }

        public object GetFilenames()
        {
            var logPath = EnvVar("Logging:Path") ?? "Logs";
            var logFiles = Directory.GetFiles(logPath + "/")
                .Select(x => x.Replace("Logs/", ""));
            logFiles = logFiles.OrderByDescending(x => x);
            return new { Items = logFiles };
        }

        public PaginatedResult GetItems(FilterParam filter)
        {
            var logPath = EnvVar("Logging:Path") ?? "Logs";
            logPath = $"{logPath}/{filter.Filename}";
            IEnumerable<LogModel> logs = null;
            using (var file = System.IO.File.OpenRead(logPath)) {
                using var reader = new StreamReader(file);
                var contents = reader.ReadToEnd();
                contents = $"[{contents}]"
                    .Replace("}\n{", "},\n{")
                    .Replace("}\r\n{", "},\r\n{");
                logs = JsonConvert.DeserializeObject<IEnumerable<LogModel>>(contents);
            }

            if (!string.IsNullOrEmpty(filter.Level)) {
                logs = logs.Where(x => x.Level.ToLower() == filter.Level.ToLower());
            }
            var totalItems = logs.Count();

            logs = logs.OrderByDescending(x => x.Timestamp)
                .Skip(filter.StartIndex);

            if (filter.PageSize > 0) logs = logs.Take(filter.PageSize);

            return new PaginatedResult {
                Items = logs,
                PageSize = filter.PageSize,
                StartIndex = filter.StartIndex,
                TotalItems = totalItems
            };
        }

        public string? EnvVar(string key) => _config.GetSection("EnvVars").GetValue<string>(key);
    }

    public class PaginatedResult
    {
        public IEnumerable<LogModel> Items { get; set; } = new List<LogModel>();
        public int PageSize { get; set; }
        public int StartIndex { get; set; }
        public int TotalItems { get; set; }
    }

    public class FilterParam
    {
        public string Filename { get; set; } = "";
        public int PageSize { get; set; } = 25;
        public string Level { get; set; } = "";
        public int StartIndex { get; set; } = 0;
    }

    public class LogModel
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = "";
        public string MessageTemplate { get; set; } = "";
        public string Exception { get; set; } = "";
    }
}
