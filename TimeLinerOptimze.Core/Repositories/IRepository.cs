using CSharp.Functional.Constructs;
using System.Security.Cryptography.X509Certificates;

namespace TimeLinerOptimze.Core.Repositories
{
    public interface IRepository
    {
        Task<Validation<List<T>>> Read<T>(string filePath);
        Task<Validation<string>> Write<T>(List<T> ts,string filePath);
        Task Append<T>(T t, string filePath);
    }
}
