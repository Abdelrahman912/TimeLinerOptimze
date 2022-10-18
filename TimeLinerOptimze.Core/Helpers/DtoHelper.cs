using TimeLinerOptimze.Core.Dtos;
using TimeLinerOptimze.Core.Models.TimeLiner;

namespace TimeLinerOptimze.Core.Helpers
{
    public static class DtoHelper
    {
        public static Activity AsActivity(this ActivityDto dto)
        {
            var preds = dto.Predecessors.Split(';').ToList();
            preds = preds.Take(preds.Count - 1).ToList();

            var succs = dto.Successors.Split(';').ToList();
            succs = succs.Take(succs.Count - 1).ToList();

            return new Activity()
            {
                Number = dto.Number,
                Name = dto.Name,
                ActivityType = dto.ActivityType,
                LevelNumber = dto.LevelNumber,
                Duration = dto.Duration,
                StartDate = dto.StartDate,
                FinishDate = dto.FinishDate,
                Crew = dto.Crew,
                CrewCost = dto.CrewCost,
                UsedGroups = dto.UsedGroups,
                Predecessors = preds,
                Successors = succs,
                Quantity = dto.Quantity,
            };
        }
    }
}
