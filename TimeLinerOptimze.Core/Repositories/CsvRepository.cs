using CSharp.Functional.Constructs;
using CsvHelper;
using System.Globalization;
using static CSharp.Functional.Extensions.ValidationExtension;
using static TimeLinerOptimze.Core.Errors.Errors;

namespace TimeLinerOptimze.Core.Repositories
{
    public class CsvRepository : IRepository
    {
        public Task<Validation<List<T>>> Read<T>(string filePath)
        {
            return Task.Run(() =>
             {
                 if (!File.Exists(filePath))
                     return FileNotFound(filePath);

                 try
                 {
                     using (var streamReader = new StreamReader(filePath))
                     using (var csvRedaer = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                     {
                         return Valid(csvRedaer.GetRecords<T>().ToList());
                     }
                 }
                 catch (IOException)
                 {
                     return FileUsedByAnotherProcess(filePath);
                 }
             });
        }

        public async Task<Validation<string>> Write<T>(List<T> ts, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csv.WriteRecordsAsync(ts);
                }

                return filePath;
            }
            catch (System.Exception e)
            {
                return CannotWriteToFile(filePath);
            }
        }

    }
}
