using Junmidsen.Core;

namespace Junmidsen.Tests
{
    public class LogProcessorTests
    {
        [Fact]
        public void Process_ValidLines_WriteToOutput()
        {
            string input = "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'\n2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'";

            string inputFile = Path.GetTempFileName();
            string outputFile = Path.GetTempFileName();
            string problemsFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(inputFile, input);

                var processor = new LogProcessor(inputFile, outputFile, problemsFile);
                processor.Process();

                string[] outputLines = File.ReadAllLines(outputFile);
                Assert.Equal(2, outputLines.Length);

                Assert.Contains("10-03-2025", outputLines[0]);
                Assert.Contains("INFO", outputLines[0]);
                Assert.Contains("DEFAULT", outputLines[0]);
                Assert.Contains("Версия программы", outputLines[0]);

                Assert.Contains("10-03-2025", outputLines[1]); 
                Assert.Contains("INFO", outputLines[1]);
                Assert.Contains("MobileComputer.GetDeviceId", outputLines[1]);
                Assert.Contains("Код устройства", outputLines[1]);

                string problems = File.ReadAllText(problemsFile);
                Assert.Equal("", problems.Trim());
            }
            finally
            {
                File.Delete(inputFile);
                File.Delete(outputFile);
                File.Delete(problemsFile);
            }
        }

        [Fact]
        public void Process_InvalidLine_WritesToProblems()
        {
            string input = "garbage line";
            string inputFile = Path.GetTempFileName();
            string outputFile = Path.GetTempFileName();
            string problemsFile = Path.GetTempFileName();

            try
            {
                File.WriteAllText(inputFile, input);

                var processor = new LogProcessor(inputFile, outputFile, problemsFile);
                processor.Process();

                Assert.Equal("", File.ReadAllText(outputFile).Trim());
                Assert.Equal("garbage line", File.ReadAllText(problemsFile).Trim());
            }
            finally
            {
                File.Delete(inputFile);
                File.Delete(outputFile);
                File.Delete(problemsFile);
            }
        }
    }
}