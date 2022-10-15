using CSharp.Functional.Errors;

namespace TimeLinerOptimze.Core.Errors
{
    public class FileNotFoundError:Error
    {
        public override string Message { get; }

        public FileNotFoundError(string filePath)
        {
            Message = $"File: \"{filePath}\" is not Found!";
        }
    }
}
