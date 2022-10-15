namespace TimeLinerOptimze.Core.Loggers
{
    public interface ILogger
    {
        void Log<T>(T t) where T : class;
    }
}
