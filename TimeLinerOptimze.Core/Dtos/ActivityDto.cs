namespace TimeLinerOptimze.Core.Dtos
{
    public record ActivityDto
    {
        public int Number { get; init; }
        public string Name { get; init; }
        public string ActivityType { get; init; }
        public int LevelNumber { get; init; }
        public int Duration { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime FinishDate { get; init; }
        public string Crew { get; init; }
        public double Cost { get; init; }
        public int UsedGroups { get; init; }
        public string Predecessors { get; init; }
        public string Successors { get; init; }
        public double Quantity { get; init; }
    }
}
