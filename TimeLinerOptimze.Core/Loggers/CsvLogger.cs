using TimeLinerOptimze.Core.Repositories;

namespace TimeLinerOptimze.Core.Loggers
{
    public class CsvLogger<T> : ILogger<T> where T : class
    {

        #region Private Fields

        private readonly string _dirPath;
        private readonly string _fileName = "TimeLinerOptimizeLog";
        private readonly IRepository<T> _repository;
        private readonly string _fullPath;
        #endregion

        #region Properties

        #endregion

        #region Constructors

        public CsvLogger(string dirPath,
                         IRepository<T> repository)
        {
            _dirPath = dirPath;
            _repository = repository;
            _fullPath = $"{_dirPath}\\{_fileName}.csv";
            Init();
        }

        #endregion

        #region Methods

        private async void Init()
        {
            await  _repository.Write(new List<T>(), _fullPath);
        }

        public void Log(T t) 
        {
            _repository.Append(t,_fullPath);
        }

        #endregion

    }
}
