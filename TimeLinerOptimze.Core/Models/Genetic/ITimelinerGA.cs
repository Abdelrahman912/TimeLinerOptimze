using CSharp.Functional.Constructs;
using TimeLinerOptimze.Core.Models.TimeLiner;

namespace TimeLinerOptimze.Core.Models.Genetic
{
    public interface ITimelinerGA
    {
        public TimeLine InitialTimeLine { get; }
        public GaInput Input { get; }
        Validation<List<TimeLine>> RunGA();
    }
}
