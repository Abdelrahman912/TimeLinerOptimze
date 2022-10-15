namespace TimeLinerOptimze.Core.Models.TimeLiner
{
    public record TimeLine
    {
        public List<Activity> Activities { get; init; }

        public int TotalDuration { get; init; }

        public int TotalCost { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime FinishDate { get; init; }

    }
}
