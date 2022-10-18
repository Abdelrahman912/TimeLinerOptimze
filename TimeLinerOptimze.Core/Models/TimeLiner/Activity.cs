namespace TimeLinerOptimze.Core.Models.TimeLiner
{
    public record Activity
    {
        public int Number { get; init; }
        public string Name { get; init; }
        public string ActivityType { get; init; }
        public int LevelNumber { get; init; }
        public int Duration { get; init; }//variable
        public DateTime StartDate { get; init; }//variable
        public DateTime FinishDate { get; init; }//variable
        public string Crew { get; init; }
        public double CrewCost { get; init; }
        public int UsedGroups { get; init; }//variable
        public List<string> Predecessors { get; init; }
        public List<string> Successors { get; init; }
        public double Quantity { get; init; }

    }
}
