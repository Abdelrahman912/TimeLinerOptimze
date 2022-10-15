using CSharp.Functional.Errors;

namespace TimeLinerOptimze.Core.Errors
{
    public class CannotWriteToFileError:Error
    {
        public override string Message { get; }
        public CannotWriteToFileError(string filePath)
        {
            Message = $"Cannot write to file: \"{filePath}\"";
        }
    }
}
