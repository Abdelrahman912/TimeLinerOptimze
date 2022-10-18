using System.Text;
using TimeLinerOptimze.Core.Dtos;
using TimeLinerOptimze.Core.Models.TimeLiner;

namespace TimeLinerOptimze.Core.Helpers
{
    public static class DtoHelper
    {

        public static ActivityDto AsDto(this Activity activity)
        {
            var preds = activity.Predecessors.Aggregate(new StringBuilder(), (soFar, current) => soFar.Append($"{current};"))
                                .ToString()
                                .TrimEnd(';');
            var successors = activity.Successors.Aggregate(new StringBuilder(), (soFar, current) => soFar.Append($"{current};"))
                                                  .ToString()
                                                  .TrimEnd(';');
            return new ActivityDto()
            {
                Number = activity.Number,
                Name = activity.Name,
                ActivityType = activity.ActivityType,
                LevelNumber = activity.LevelNumber,
                Duration = activity.Duration,
                StartDate = activity.StartDate,
                FinishDate = activity.FinishDate,
                Crew = activity.Crew,
                Cost = activity.CrewCost,
                UsedGroups = activity.UsedGroups,
                Predecessors = preds,
                Successors = successors,
                Quantity = activity.Quantity,
            };
        }

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
                CrewCost = dto.Cost,
                UsedGroups = dto.UsedGroups,
                Predecessors = preds,
                Successors = succs,
                Quantity = dto.Quantity,
            };
        }
    }
}
