using TimeLinerOptimze.Core.Dtos;

namespace TimeLinerOptimze.Core.Loggers
{
    public class TextFileLogger:ILogger<GaTimeLineDto>
    {
        private readonly string _dirPath;
        private readonly string _fileName = "TimeLinerOptimizeLog";
        //private readonly IRepository _repository;
        private readonly string _fullPath;
        public TextFileLogger(string dirPath)
                        
        {
            _dirPath = dirPath;
            _fullPath = $"{_dirPath}\\{_fileName}.txt";
        }

       

        public async void Log(GaTimeLineDto t)
        {
            using (var writer = File.AppendText(_fullPath))
            {
                await writer.WriteLineAsync(t.ToString());
            }
        }
    }
}
