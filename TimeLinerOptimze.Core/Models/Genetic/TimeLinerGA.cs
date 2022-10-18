using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using GeneticSharp;
using TimeLinerOptimze.Core.Dtos;
using TimeLinerOptimze.Core.Extensions;
using TimeLinerOptimze.Core.Helpers;
using TimeLinerOptimze.Core.Loggers;
using TimeLinerOptimze.Core.Models.TimeLiner;
using static TimeLinerOptimze.Core.Errors.Errors;

namespace TimeLinerOptimze.Core.Models.Genetic
{
    public class TimeLinerGA : ITimelinerGA
    {
        #region Private Fields
        private readonly ILogger<GaTimeLineDto> _logger;
        private readonly int _noActivities;
        private readonly IDictionary<int, Activity> _databaseDict;
        private readonly IDictionary<int, List<Activity>> _predecessorsCache;
        #endregion

        #region Properties

        public TimeLine InitialTimeLine { get; }

        public GaInput Input { get; }

        #endregion

        #region Constructors

        public TimeLinerGA(TimeLine initialTimeLine,
                           GaInput input,
                           ILogger<GaTimeLineDto> logger )
        {
            InitialTimeLine = initialTimeLine;
            Input = input;
            _logger = logger;
            _noActivities = initialTimeLine.Activities.Count;
            _databaseDict = initialTimeLine.Activities.ToDictionary(act => act.Name.GetHashCode());
            _predecessorsCache = new Dictionary<int, List<Activity>>();
        }

        #endregion

        #region Methods

        private List<TimeLine> Run()
        {
            var minVals = Enumerable.Range(1, _noActivities).Select(i => 1.0).ToArray();
            var maxVals = Enumerable.Range(1, _noActivities).Select(i => 3.0).ToArray();
            var bits = Enumerable.Range(1, _noActivities).Select(i => 2).ToArray();
            var fraction = Enumerable.Range(1, _noActivities).Select(i => 0).ToArray();

            var chm = new FloatingPointChromosome(minVals, maxVals, bits, fraction);
            var population = new Population(Input.PopulationNo, Input.PopulationNo, chm);

            Func<Activity, List<Activity>> getPredecessors = (act) =>
            {
                var preds = _predecessorsCache.Lookup(act.Name.GetHashCode())
                                              .OrElse(() => GetPredecessors(act))
                                              .AsEnumerable()
                                              .Flatten()
                                              .ToList();
                return preds;
            };

            var fitness = new FuncFitness((c) =>
            {
                var fc = c as FloatingPointChromosome;

                var acts = fc!.AsActivities(InitialTimeLine, getPredecessors);
                var totalTime = acts.GetTotalTime();
                var totalCost = acts.GetTotalCost();
                var fitnessValue = totalTime * totalCost;
                return fitnessValue == 0 ? -1 : 1 / fitnessValue;
            });
            var selection = new EliteSelection();
            var crossOver = new UniformCrossover(0.5f);
            var mutation = new FlipBitMutation();
            var termination = new FitnessStagnationTermination(Input.GenerationNo);
            var ga = new GeneticAlgorithm(population, fitness, selection, crossOver, mutation);
            ga.Termination = termination;

            var timeLines = new List<TimeLine>();

            ga.GenerationRan += (sender, e) =>
            {
                var genNumber = ga.GenerationsNumber;
                var bestTimeLine = (ga.BestChromosome as FloatingPointChromosome)!
                                    .AsActivities(InitialTimeLine, getPredecessors)
                                    .AsTimeLine();
                timeLines.Add(bestTimeLine);
                var timeLineGa = new GaTimeLineDto()
                {
                    GenerationNumber = genNumber,
                    Chromosome = bestTimeLine.GetChromosomeCode(),
                    TotalCost = bestTimeLine.TotalCost,
                    TotalDuration = bestTimeLine.TotalDuration,
                    Fitness = ga.Fitness.Evaluate(ga.BestChromosome)
                };
                _logger?.Log(timeLineGa);
            };
            ga.Start();
            return timeLines;
        }

        public Validation<List<TimeLine>> RunGA()
        {
            try
            {
                var result = Run();
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return GeneticError;
            }
        }

        private List<Activity> GetPredecessors(Activity activity)
        {
            var predecessors = activity.Predecessors.Select(n=>n.GetHashCode())
                                                    .SelectMany(i => _databaseDict.Lookup(i).AsEnumerable())
                                                    .ToList();
            _predecessorsCache.TryAdd(activity.Name.GetHashCode(), predecessors);
            return predecessors;
        }

        #endregion

    }
}
