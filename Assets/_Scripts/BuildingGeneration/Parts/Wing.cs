using UnityEngine;

namespace _Scripts.BuildingGeneration.Parts
{
    public class Wing
    {
        private RectInt Bounds { get; }
        private Story[] Stories { get; }
        private Roof Roof { get; }

        public Wing(RectInt bounds)
        {
            Bounds = bounds;
        }

        public Wing(RectInt bounds, Story[] stories, Roof roof)
        {
            Bounds = bounds;
            Stories = stories;
            Roof = roof;
        }

        public override string ToString()
        {
            string wing = "Wing(" + Bounds + "):\n";
            for (int i = 0; i < Stories.Length; i++)
                wing += "\t" + Stories[i] + "\n";
            wing += "\t" + Roof + "\n";
            return wing;
        }
    }
}