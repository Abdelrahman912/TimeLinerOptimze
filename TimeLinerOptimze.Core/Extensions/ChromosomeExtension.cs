using GeneticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TimeLinerOptimze.Core.Helpers;
using TimeLinerOptimze.Core.Models.TimeLiner;

namespace TimeLinerOptimze.Core.Extensions
{
    public static class ChromosomeExtension
    {
        public static List<Activity> AsActivities(this FloatingPointChromosome chm,TimeLine initialTimeLine , Func<Activity, List<Activity>> getPredecessors)
        {
            var acts = chm.ToFloatingPoints()
                               .Select((v, i) => Tuple.Create(initialTimeLine.Activities[i], new ActivityOptimizeValue() { Name = initialTimeLine.Activities[i].Name, UsedGroups = (int)v }))
                               .Select(tup => tup.Item1.AsNewActivity(tup.Item2, getPredecessors(tup.Item1)))
                               .ToList();
            return acts;
        }
    }
}
