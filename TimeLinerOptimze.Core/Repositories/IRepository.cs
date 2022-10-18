using CSharp.Functional.Constructs;
using System.Security.Cryptography.X509Certificates;

namespace TimeLinerOptimze.Core.Repositories
{
    public interface IRepository<T>
    {
        Task<Validation<List<T>>> Read(string filePath);
        Task<Validation<string>> Write(List<T> ts,string filePath);
        Task Append(T t, string filePath);
    }
}
