using System.Collections.Generic;
using TimeLinerOptimze.Core.Extensions;
using TimeLinerOptimze.Core.Models.TimeLiner;
using System.Linq;

namespace TimeLinerOptimze.Core.Helpers
{
    public static class TimeLinerHelper
    {

        public static Activity AsNewActivity(this Activity initial , ActivityOptimizeValue newValue, List<Activity> predecessors)
        {
            var newDuration = (int)((initial.Duration * 1.0) / (newValue.UsedGroups));
            var newStart = new DateTime();
            var newEnd = new DateTime();    
            if(!predecessors.Any())
            {
                newStart = initial.StartDate;
            }
            else
            {
                newStart = predecessors.OrderByDescending(act => act.FinishDate).First().FinishDate.AddDays(1);
            }
            newEnd = newStart.AddDays(newDuration-1);
            var newActivity = initial with
            {
                Duration = newDuration,
                StartDate = newStart,
                FinishDate = newEnd,
                UsedGroups = newValue.UsedGroups
            };
            return newActivity;
        }

        public static double GetTotalCost(this Activity activity)
        {
            var totalCost = activity.UsedGroups * activity.CrewCost*activity.Duration;
            return totalCost;
        }

        public static double GetTotalCost(this IEnumerable<Activity> activities)=>
            activities.Sum(act=>act.GetTotalCost());
        

        public static int GetTotalTime(this List<Activity> activities)
        {
            var timeSpan = activities.Last().FinishDate - activities.First().StartDate;
            var numberOfDays = timeSpan.Days;
            return numberOfDays;
        }

        public static TimeLine AsTimeLine(this List<Activity> activities)
        {
            var timeLine = new TimeLine()
            {
                Activities = activities,
                StartDate = activities.First().StartDate,
                FinishDate = activities.Last().FinishDate,
                TotalCost = activities.GetTotalCost(),
                TotalDuration = activities.GetTotalTime()
            };
            return timeLine;
        }

    }
}
