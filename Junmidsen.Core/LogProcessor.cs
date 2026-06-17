using System.Text;
using System.Text.RegularExpressions;

namespace Junmidsen.Core
{
    /// <summary>
    /// Processes log files with two possible input formats and converts them to a unified format
    /// </summary>
    public class LogProcessor
    {
        private readonly string _inputPath;
        private readonly string _outputPath;
        private readonly string _problemsPath;

        public LogProcessor(string inputPath, string outputPath, string problemsPath = "problems.txt")
        {
            _inputPath = inputPath;
            _outputPath = outputPath;
            _problemsPath = problemsPath;
        }

        public void Process()
        {
            using var outputWriter = new StreamWriter(_outputPath, false, Encoding.UTF8);
            using var problemsWriter = new StreamWriter(_problemsPath, false, Encoding.UTF8);

            foreach (string line in File.ReadLines(_inputPath, Encoding.UTF8))
            {
                if (TryParseLine(line, out LogEntry? entry))
                    outputWriter.WriteLine(entry.ToOutputFormat());
                else
                    problemsWriter.WriteLine(line);
            }
        }

        private bool TryParseLine(string line, out LogEntry? entry)
        {
            entry = null;

            var match1 = Regex.Match(line,  @"^(\d{2}\.\d{2}\.\d{4}) (\d{2}:\d{2}:\d{2}\.\d{1,3}) (INFORMATION|WARNING|ERROR|DEBUG) (.*)$");
            if (match1.Success)
            {
                entry = new LogEntry
                {
                    Date = DateTime.ParseExact(match1.Groups[1].Value, "dd.MM.yyyy", null),
                    Time = match1.Groups[2].Value,
                    Level = NormalizeLevel(match1.Groups[3].Value),
                    Method = "DEFAULT",
                    Message = match1.Groups[4].Value.Trim()
                };
                return true;
            }

            var match2 = Regex.Match(line, @"^(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2}\.\d+)\|\s*(INFO|WARN|ERROR|DEBUG)\|\d+\|([^|]+)\|\s*(.*)$");
            if (match2.Success)
            {
                entry = new LogEntry
                {
                    Date = DateTime.ParseExact(match2.Groups[1].Value, "yyyy-MM-dd", null),
                    Time = match2.Groups[2].Value,
                    Level = NormalizeLevel(match2.Groups[3].Value),
                    Method = match2.Groups[4].Value.Trim(),
                    Message = match2.Groups[5].Value.Trim()
                };
                return true;
            }

            return false;
        }

        private string NormalizeLevel(string level)
        {
            return level.ToUpperInvariant() switch
            {
                "INFORMATION" => "INFO",
                "WARNING" => "WARN",
                "ERROR" => "ERROR",
                "DEBUG" => "DEBUG",
                _ => level
            };
        }

        private class LogEntry
        {
            public DateTime Date { get; set; }
            public string? Time { get; set; }
            public string? Level { get; set; }
            public string? Method { get; set; }
            public string? Message { get; set; }

            public string ToOutputFormat()
            {
                string dateStr = Date.ToString("dd-MM-yyyy");
                return $"{dateStr}\t{Time}\t{Level}\t{Method}\t{Message}";
            }
        }
    }
}