using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLinerOptimze.Core.Models.Genetic
{
    public record GaInput
    {
        public int PopulationNo { get; init; }
        public int GenerationNo { get; init; }
    }
}
