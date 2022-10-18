namespace TimeLinerOptimze.Core.Errors
{
    public static class Errors
    {
        public static FileNotFoundError FileNotFound(string filePath)=>
            new FileNotFoundError(filePath);

        public static FileUsedByAnotherProcessError FileUsedByAnotherProcess(string filePath) =>
            new FileUsedByAnotherProcessError(filePath);

        public static CannotWriteToFileError CannotWriteToFile(string filePath) =>
            new CannotWriteToFileError(filePath);

        public static GeneticError GeneticError =>
            new GeneticError();
    }
}
