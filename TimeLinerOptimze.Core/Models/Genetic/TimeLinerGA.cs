using CSharp.Functional.Extensions;
using GeneticSharp;
using TimeLinerOptimze.Core.Helpers;
using TimeLinerOptimze.Core.Loggers;
using TimeLinerOptimze.Core.Models.TimeLiner;

namespace TimeLinerOptimze.Core.Models.Genetic
{
    public class TimeLinerGA : ITimelinerGA
    {
        #region Private Fields
        private readonly ILogger _logger;
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
                           ILogger logger)
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

        public List<TimeLine> RunGA()
        {
            var minVals = Enumerable.Range(1, _noActivities).Select(i => 0.0).ToArray();
            var maxVals = Enumerable.Range(1, _noActivities).Select(i => 3.0).ToArray();
            var bits = Enumerable.Range(1, _noActivities).Select(i => 2).ToArray();
            var fraction = Enumerable.Range(1, _noActivities).Select(i => 0).ToArray();

            var chm = new FloatingPointChromosome(minVals,maxVals,bits,fraction);
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

                var acts = fc.ToFloatingPoints()
                               .Select((v,i)=>Tuple.Create(InitialTimeLine.Activities[i], new ActivityOptimizeValue() { Name = InitialTimeLine.Activities[i].Name,UsedGroups = (int)v }))
                               .Select(tup => tup.Item1.AsNewActivity(tup.Item2,getPredecessors(tup.Item1)))
                               .ToList();
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
            ga.GenerationRan += (sender, e) =>
            {

            };
            ga.Start();
            throw new NotImplementedException();
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
