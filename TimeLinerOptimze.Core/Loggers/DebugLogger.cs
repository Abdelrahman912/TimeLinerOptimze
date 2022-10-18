using System.Diagnostics;

namespace TimeLinerOptimze.Core.Loggers
{
    public class DebugLogger<T> : ILogger<T> where T : class
    {
        public void Log(T t) 
        {
            Debug.WriteLine(t.ToString());
        }
    }
}
