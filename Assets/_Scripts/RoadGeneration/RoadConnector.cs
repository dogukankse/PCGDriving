using System;
using _Scripts.RoadGeneration;
using UnityEngine;
using static _Scripts.Utils;
using Object = UnityEngine.Object;


namespace _Scripts
{
    public class RoadConnector
    {
        private readonly MapTile[,] _map;

        public RoadConnector(MapTile[,] map)
        {
            _map = map;
        }

        internal void ConnectRoads()
        {
            int height = _map.GetLength(0);
            int width = _map.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject currentRoad = _map[x, y].road;
                    int roadType = GetRoadType(currentRoad);
                    EmptyNodes emptyNodes = currentRoad.GetComponent<EmptyNodes>();


                    GameObject rNeighborRoad;
                    EmptyNodes rNeighborEmptyNodes;
                    GameObject bNeighborRoad;
                    EmptyNodes bNeighborEmptyNodes;

                    if (roadType == 1)
                    {
                        if (x == width - 1) return;
                        rNeighborRoad = _map[x + 1, y].road;
                        rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                        //road connect
                        if (IsSpecial(rNeighborRoad, new[] {4, 6, 7, 9}))
                            ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                        else
                            ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                rNeighborEmptyNodes.startRight, emptyNodes.endRight);


                        //Pedestrian connect
                        if (IsSpecial(rNeighborRoad, new[] {5}))
                            ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startRightP,
                                rNeighborEmptyNodes.startLeftP, emptyNodes.endRightP);
                        else
                            ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startLeftP,
                                rNeighborEmptyNodes.startRightP, emptyNodes.endRightP);
                    }
                    else if (roadType == 2)
                    {
                        if (y == height - 1) return;
                        bNeighborRoad = _map[x, y + 1].road;
                        bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                        //road connect
                        if (IsSpecial(bNeighborRoad, new[] {7, 10}))
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                        else
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);

                        //pedestrian connect
                        if (IsSpecial(bNeighborRoad, new[] {6, 8, 3, 2, 10}))
                            ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topLeftP,
                                bNeighborEmptyNodes.topRightP, emptyNodes.bottomRightP);
                        else
                            ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topRightP,
                                bNeighborEmptyNodes.topLeftP, emptyNodes.bottomRightP);
                    }
                    else if (roadType == 3)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _map[x + 1, y].road;
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(rNeighborRoad, new[] {4, 6, 7, 9, 8}))
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                    rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                            else
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                    rNeighborEmptyNodes.startRight, emptyNodes.endRight);

                            //pedestrian connect
                            if (IsSpecial(rNeighborRoad, new[] {5}))
                                ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startRightP,
                                    rNeighborEmptyNodes.startLeftP, emptyNodes.endRightP);
                            else
                                ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startLeftP,
                                    rNeighborEmptyNodes.startRightP, emptyNodes.endRightP);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _map[x, y + 1].road;
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(bNeighborRoad, new[] {7}))
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                            else
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);

                            //pedestrian connect
                            if (IsSpecial(bNeighborRoad, new[] {7}))
                                ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topRightP,
                                    bNeighborEmptyNodes.topLeftP, emptyNodes.bottomRightP);
                            else
                                ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topLeftP,
                                    bNeighborEmptyNodes.topRightP, emptyNodes.bottomRightP);
                        }
                    }
                    else if (roadType == 4)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _map[x + 1, y].road;
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(rNeighborRoad, new[] {6, 9}))
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                    rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                            else
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                    rNeighborEmptyNodes.startRight, emptyNodes.endRight);

                            //pedestrian connect
                            if (IsSpecial(rNeighborRoad, new[] {3, 8, 11, 6, 1}))
                                ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startLeftP,
                                    rNeighborEmptyNodes.startRightP, emptyNodes.endRightP);
                            else
                                ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startRightP,
                                    rNeighborEmptyNodes.startLeftP, emptyNodes.endRightP);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _map[x, y + 1].road;
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(bNeighborRoad, new[] {3, 6, 2, 8, 9}))
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomLeft);
                            else
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomLeft);

                            //pedestrian connect
                            if (IsSpecial(bNeighborRoad, new[] {3}))
                                ConnectPedestrianNodes(emptyNodes.bottomRightP, bNeighborEmptyNodes.topLeftP,
                                    bNeighborEmptyNodes.topRightP, emptyNodes.bottomLeftP);
                            else
                                ConnectPedestrianNodes(emptyNodes.bottomRightP, bNeighborEmptyNodes.topRightP,
                                    bNeighborEmptyNodes.topLeftP, emptyNodes.bottomLeftP);
                        }
                    }
                    else if (roadType == 5)
                    {
                        if (y == height - 1) return;
                        bNeighborRoad = _map[x, y + 1].road;
                        bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();


                        //road connect
                        if (IsSpecial(bNeighborRoad, new[] {7}))
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                        else
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);

                        //pedestrian connect
                        if (IsSpecial(bNeighborRoad, new[] {7}))
                            ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topRightP,
                                bNeighborEmptyNodes.topLeftP, emptyNodes.bottomRightP);
                        else
                            ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topLeftP,
                                bNeighborEmptyNodes.topRightP, emptyNodes.bottomRightP);
                    }
                    else if (roadType == 6)
                    {
                    }
                    else if (roadType == 7)
                    {
                        if (x == width - 1) return;
                        rNeighborRoad = _map[x + 1, y].road;
                        rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                        //road connect
                        if (IsSpecial(rNeighborRoad, new[] {1, 5, 3, 8}))
                            ConnectNodes(rNeighborEmptyNodes.startRight, emptyNodes.endLeft,
                                emptyNodes.endRight, rNeighborEmptyNodes.startLeft);
                        else
                            ConnectNodes(emptyNodes.endRight, rNeighborEmptyNodes.startRight,
                                rNeighborEmptyNodes.startLeft, emptyNodes.endLeft);

                        //pedestrian connect
                        if (IsSpecial(rNeighborRoad, new[] {1, 5, 3, 8}))
                            ConnectPedestrianNodes(rNeighborEmptyNodes.startRightP, emptyNodes.endLeftP,
                                emptyNodes.endRightP, rNeighborEmptyNodes.startLeftP);
                        else
                            ConnectPedestrianNodes(emptyNodes.endRightP, rNeighborEmptyNodes.startRightP,
                                rNeighborEmptyNodes.startLeftP, emptyNodes.endLeftP);
                    }
                    else if (roadType == 8)
                    {
                        if (y == height - 1) return;
                        bNeighborRoad = _map[x, y + 1].road;
                        bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                        //road connect
                        if (IsSpecial(bNeighborRoad, new[] {7}))
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                        else
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);
                        //pedestrian connect
                        if (IsSpecial(bNeighborRoad, new[] {0}))
                            ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topRightP,
                                bNeighborEmptyNodes.topLeftP, emptyNodes.bottomRightP);
                        else
                            ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topLeftP,
                                bNeighborEmptyNodes.topRightP, emptyNodes.bottomRightP);
                    }
                    else if (roadType == 9)
                    {
                        if (x == width - 1) return;
                        rNeighborRoad = _map[x + 1, y].road;
                        rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                        //road connect
                        if (IsSpecial(rNeighborRoad, new[] {1, 3, 5, 8}))
                            ConnectNodes(emptyNodes.endRight, rNeighborEmptyNodes.startLeft,
                                rNeighborEmptyNodes.startRight, emptyNodes.endLeft);
                        else
                            ConnectNodes(emptyNodes.endRight, rNeighborEmptyNodes.startRight,
                                rNeighborEmptyNodes.startLeft, emptyNodes.endLeft);

                        //pedestrian connect
                        if (IsSpecial(rNeighborRoad, new[] {0}))
                            ConnectPedestrianNodes(emptyNodes.endRightP, rNeighborEmptyNodes.startLeftP,
                                rNeighborEmptyNodes.startRightP, emptyNodes.endLeftP);
                        else
                            ConnectPedestrianNodes(emptyNodes.endRightP, rNeighborEmptyNodes.startRightP,
                                rNeighborEmptyNodes.startLeftP, emptyNodes.endLeftP);
                    }
                    else if (roadType == 10)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _map[x + 1, y].road;
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(rNeighborRoad, new[] {9, 6}))
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                    rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                            else
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                    rNeighborEmptyNodes.startRight, emptyNodes.endRight);

                            //pedestrian connect
                            if (IsSpecial(rNeighborRoad, new[] {9, 6, 1}))
                                ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startLeftP,
                                    rNeighborEmptyNodes.startRightP, emptyNodes.endRightP);
                            else
                                ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startRightP,
                                    rNeighborEmptyNodes.startLeftP, emptyNodes.endRightP);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _map[x, y + 1].road;
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(bNeighborRoad, new[] {2, 3}))
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomLeft);
                            else
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomLeft);
                            //pedestrian connect
                            if (IsSpecial(bNeighborRoad, new[] {2, 3, 10, 7}))
                                ConnectPedestrianNodes(emptyNodes.bottomRightP, bNeighborEmptyNodes.topRightP,
                                    bNeighborEmptyNodes.topLeftP, emptyNodes.bottomLeftP);
                            else
                                ConnectPedestrianNodes(emptyNodes.bottomRightP, bNeighborEmptyNodes.topLeftP,
                                    bNeighborEmptyNodes.topRightP, emptyNodes.bottomLeftP);
                        }
                    }
                    else if (roadType == 11)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _map[x + 1, y].road;
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();
                            //road connect
                            ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                rNeighborEmptyNodes.startRight, emptyNodes.endRight);
                            //pedestrian connect
                            ConnectPedestrianNodes(emptyNodes.endLeftP, rNeighborEmptyNodes.startLeftP,
                                rNeighborEmptyNodes.startRightP, emptyNodes.endRightP);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _map[x, y + 1].road;
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            //road connect
                            if (IsSpecial(bNeighborRoad, new[] {7}))
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                            else
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);

                            //pedestrian connect
                            if (IsSpecial(bNeighborRoad, new[] {0}))
                                ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topRightP,
                                    bNeighborEmptyNodes.topLeftP, emptyNodes.bottomRightP);
                            else
                                ConnectPedestrianNodes(emptyNodes.bottomLeftP, bNeighborEmptyNodes.topLeftP,
                                    bNeighborEmptyNodes.topRightP, emptyNodes.bottomRightP);
                        }
                    }
                }
            }

            ClearEmptyNodes(height, width);
        }

        private void ClearEmptyNodes(int height, int width)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Object.Destroy(_map[x, y].road.GetComponent<EmptyNodes>());
                }
            }
        }


        /// <summary>
        /// Connection dirs:
        /// a=>b
        /// c=>d
        /// </summary>
        /// <param name="a">base node to connect to b</param>
        /// <param name="b">get connection from a</param>
        /// <param name="c">base node to connect to d</param>
        /// <param name="d">get connection from c</param>
        private void ConnectNodes(TrafficSystemNode a, TrafficSystemNode b, TrafficSystemNode c, TrafficSystemNode d)
        {
            a.m_connectedLocalNode = b;
            c.m_connectedLocalNode = d;
        }

        /// <summary>
        /// Connects a <=> b and c <=> d
        /// </summary>
        private void ConnectPedestrianNodes(AIPoint a, AIPoint b, AIPoint c, AIPoint d)
        {
            a.AddNode(b);
            b.AddNode(a);

            c.AddNode(d);
            d.AddNode(c);
        }


        private bool IsSpecial(GameObject neighbor, int[] cases)
        {
            return Array.Exists(cases, e => e == GetRoadType(neighbor));
        }
    }
}