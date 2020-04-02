using System.Collections.Generic;
using UnityEngine;

public class EmptyNodes : MonoBehaviour
{
    #region Car Nodes
    [Header("Road")]

    public TrafficSystemNode topLeft;
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

    
    public PedestrianNode topLeftP;
    public PedestrianNode topRightP;
    public PedestrianNode bottomLeftP;
    public PedestrianNode bottomRightP;
    public PedestrianNode startLeftP;
    public PedestrianNode startRightP;
    public PedestrianNode endLeftP;
    public PedestrianNode endRightP;

    #endregion
}