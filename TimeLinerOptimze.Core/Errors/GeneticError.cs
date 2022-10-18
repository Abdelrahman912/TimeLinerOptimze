using CSharp.Functional.Errors;

namespace TimeLinerOptimze.Core.Errors
{
    public class GeneticError:Error
    {
        public override string Message { get; }
        public GeneticError()
        {
            Message = "Something wrong occured while executing genetic algorithim";
        }
    }
}
