using TimeLinerOptimze.Core.Loggers;
using TimeLinerOptimze.Core.Models.TimeLiner;

namespace TimeLinerOptimze.Core.Models.Genetic
{
    public class TimeLinerGA : ITimelinerGA
    {
        #region Private Fields
        private readonly ILogger _logger;
        #endregion

        #region Properties

        public TimeLine InitialTimeLine { get; }

        public GaInput Input { get; }

        #endregion

        #region Constructors

        public TimeLinerGA(TimeLine initialTimeLine,
                           GaInput input,
                           ILogger logger)
        {
            InitialTimeLine = initialTimeLine;
            Input = input;
            _logger = logger;
        }

        #endregion

        #region Methods

        public List<TimeLine> RunGA()
        {
            throw new NotImplementedException();
        }

        #endregion




    }
}
