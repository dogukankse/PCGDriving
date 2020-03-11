namespace _Scripts.BuildingGeneration.Parts
{
    public class Story
    {
        private int Level { get; }
        private Wall[] Walls { get; }

        public Story(int level, Wall[] walls)
        {
            Level = level;
            Walls = walls;
        }

        public override string ToString()
        {
            string story = "Story " + Level + ":\n\tWalls: ";
            for (int i = 0; i < Walls.Length; i++)
                story += Walls[i] + ", ";
            return story;
        }
    }
}