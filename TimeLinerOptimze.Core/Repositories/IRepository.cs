using CSharp.Functional.Constructs;

namespace TimeLinerOptimze.Core.Repositories
{
    public interface IRepository
    {
        Task<Validation<List<T>>> Read<T>(string filePath);
        Task<Validation<string>> Write<T>(List<T> ts,string filePath);
    }
}
