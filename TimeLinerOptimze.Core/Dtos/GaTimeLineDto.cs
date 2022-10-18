using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLinerOptimze.Core.Dtos
{
    public record GaTimeLineDto
    {
        public int GenerationNumber { get; init; }
        public string Chromosome { get; init; }
        public double TotalCost { get; init; }
        public double TotalDuration { get; init; }
        public double Fitness { get; init; }


        public override string ToString() =>
            $"GenNo:{GenerationNumber}, Chm:{Chromosome}, TotalCost:{TotalCost}, TotalDuration:{TotalDuration}, Fitness:{Fitness}";
        
    }
}
