using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TimeLinerOptimze.Core.Models.TimeLiner
{
    public record GaTimeLine
    {
        public TimeLine TimeLine { get; init; }
        public int GenerationNumber { get; init; }
        public double Fitness { get; init; }

        public override string ToString()
        {
            var chm = TimeLine.Activities
                     .Aggregate(new StringBuilder(), (soFar, current) => soFar.Append($"{current.UsedGroups},"))
                     .ToString()
                     .TrimEnd(',');
            return $"GenNo:{GenerationNumber}, Chm:[{chm}], TotalCost:{TimeLine.TotalCost}, TotalTime:{TimeLine.TotalDuration}, Fitness:{Fitness}";
        }
    }
}
