namespace TimeLinerOptimze.Core.Loggers
{
    public interface ILogger<T> where T : class
    {
        void Log(T t);
    }
}
