using UnityEngine;

namespace _Scripts.RoadGeneration
{
    public class EmptyNodes : MonoBehaviour
    {
        #region Car Nodes

        [Header("Road")] public TrafficSystemNode topLeft;
        public TrafficSystemNode topRight;
        public TrafficSystemNode bottomLeft;
        public TrafficSystemNode bottomRight;
        public TrafficSystemNode startLeft;
        public TrafficSystemNode startRight;
        public TrafficSystemNode endLeft;
        public TrafficSystemNode endRight;

        #endregion

        #region Pedestrian Nodes

        [Header("Pedestrian")] 
        public AIPoint topLeftP;
        public AIPoint topRightP;
        public AIPoint bottomLeftP;
        public AIPoint bottomRightP;
        public AIPoint startLeftP;
        public AIPoint startRightP;
        public AIPoint endLeftP;
        public AIPoint endRightP;

        #endregion
    }
}