using CSharp.Functional.Constructs;
using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;
using static CSharp.Functional.Extensions.ValidationExtension;
using static TimeLinerOptimze.Core.Errors.Errors;

namespace TimeLinerOptimze.Core.Repositories
{
    public class CsvRepository<T> : IRepository<T>
    {
        public Task Append(T t, string filePath)
        {
            return Task.Run(() =>
             {
                 try
                 {
                     var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                     {
                         // Don't write the header again.
                         HasHeaderRecord = false,
                     };
                     using (var stream = File.Open(filePath, FileMode.Append))
                     using (var writer = new StreamWriter(stream))
                     using (var csv = new CsvWriter(writer, config))
                     {
                         csv.WriteRecords(new List<T>() { t });
                     }
                 }
                 catch (Exception e)
                 {
                     Debug.WriteLine(e.Message);
                     //csv.WriteRecords(new List<T>() { t });
                 }
             });
        }

        public Task<Validation<List<T>>> Read(string filePath)
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

        public async Task<Validation<string>> Write(List<T> ts, string filePath)
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
