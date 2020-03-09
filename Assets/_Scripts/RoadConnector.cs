using System;
using UnityEngine;
using static _Scripts.Utils;
using Object = UnityEngine.Object;


namespace _Scripts
{
    public class RoadConnector
    {
        private readonly MapTile[,] _map;
        private readonly GameObject[,] _roads;

        public RoadConnector(MapTile[,] map, GameObject[,] roads)
        {
            _map = map;
            _roads = roads;
        }

        internal void ConnectRoads()
        {
            int height = _map.GetLength(0);
            int width = _map.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject currentRoad = _roads[x, y];
                    int roadType = GetRoadType(currentRoad);
                    EmptyNodes emptyNodes = currentRoad.GetComponent<EmptyNodes>();


                    GameObject rNeighborRoad;
                    EmptyNodes rNeighborEmptyNodes;
                    GameObject bNeighborRoad;
                    EmptyNodes bNeighborEmptyNodes;
                    if (roadType == 1 && x != width - 1)
                    {
                        rNeighborRoad = _roads[x + 1, y];
                        rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                        if (IsSpecial(rNeighborRoad, new[] {4, 6, 7, 9, 8}))
                            ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                        else
                            ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                rNeighborEmptyNodes.startRight, emptyNodes.endRight);
                    }
                    else if (roadType == 2 && y != height - 1)
                    {
                        bNeighborRoad = _roads[x, y + 1];
                        bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                        if (IsSpecial(bNeighborRoad, new[] {7, 10}))
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                        else
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);
                    }
                    else if (roadType == 3)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _roads[x + 1, y];
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                            if (IsSpecial(rNeighborRoad, new[] {4, 6, 7, 9, 8}))
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                    rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                            else
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                    rNeighborEmptyNodes.startRight, emptyNodes.endRight);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _roads[x, y + 1];
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            if (IsSpecial(bNeighborRoad, new[] {7}))
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                            else
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);
                        }
                    }
                    else if (roadType == 4)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _roads[x + 1, y];
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                            if (IsSpecial(rNeighborRoad, new[] {8, 6, 9}))
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                    rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                            else
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                    rNeighborEmptyNodes.startRight, emptyNodes.endRight);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _roads[x, y + 1];
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            if (IsSpecial(bNeighborRoad, new[] {3, 6, 2, 8, 9}))
                            {
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomLeft);
                            }
                            else
                            {
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomLeft);
                            }
                        }
                    }
                    else if (roadType == 5 && y != height - 1)
                    {
                        bNeighborRoad = _roads[x, y + 1];
                        bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                        if (IsSpecial(bNeighborRoad, new[] {7}))
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                        else
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                bNeighborEmptyNodes.topRight,
                                emptyNodes.bottomRight);
                    }
                    else if (roadType == 7 && x != width - 1)
                    {
                        rNeighborRoad = _roads[x + 1, y];
                        rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();
                        if (IsSpecial(rNeighborRoad, new[] {1, 5, 3}))
                            ConnectNodes(rNeighborEmptyNodes.startRight, emptyNodes.endLeft,
                                emptyNodes.endRight, rNeighborEmptyNodes.startLeft);
                        else
                            ConnectNodes(emptyNodes.endRight, rNeighborEmptyNodes.startRight,
                                rNeighborEmptyNodes.startLeft, emptyNodes.endLeft);
                    }
                    else if (roadType == 8 && y != height - 1)
                    {
                        bNeighborRoad = _roads[x, y + 1];
                        bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                        if (IsSpecial(bNeighborRoad, new[] {7}))
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);
                        else
                            ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);
                    }
                    else if (roadType == 9 && x != width - 1)
                    {
                        rNeighborRoad = _roads[x + 1, y];
                        rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();

                        if (IsSpecial(rNeighborRoad, new[] {1, 3, 5}))
                            ConnectNodes(emptyNodes.endRight, rNeighborEmptyNodes.startLeft,
                                rNeighborEmptyNodes.startRight, emptyNodes.endLeft);
                        else
                            ConnectNodes(emptyNodes.endRight, rNeighborEmptyNodes.startRight,
                                rNeighborEmptyNodes.startLeft, emptyNodes.endLeft);
                    }
                    else if (roadType == 10)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _roads[x + 1, y];
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();
                            if (IsSpecial(rNeighborRoad, new[] {9, 6}))
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startRight,
                                    rNeighborEmptyNodes.startLeft, emptyNodes.endRight);
                            else
                                ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                    rNeighborEmptyNodes.startRight, emptyNodes.endRight);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _roads[x, y + 1];
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();

                            if (IsSpecial(bNeighborRoad, new[] {2, 3}))
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomLeft);
                            else
                                ConnectNodes(emptyNodes.bottomRight, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomLeft);
                        }
                    }
                    else if (roadType == 11)
                    {
                        if (x != width - 1)
                        {
                            rNeighborRoad = _roads[x + 1, y];
                            rNeighborEmptyNodes = rNeighborRoad.GetComponent<EmptyNodes>();
                            ConnectNodes(emptyNodes.endLeft, rNeighborEmptyNodes.startLeft,
                                rNeighborEmptyNodes.startRight, emptyNodes.endRight);
                        }

                        if (y != height - 1)
                        {
                            bNeighborRoad = _roads[x, y + 1];
                            bNeighborEmptyNodes = bNeighborRoad.GetComponent<EmptyNodes>();
                            if (IsSpecial(bNeighborRoad, new[] {7}))

                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topRight,
                                    bNeighborEmptyNodes.topLeft, emptyNodes.bottomRight);

                            else
                                ConnectNodes(emptyNodes.bottomLeft, bNeighborEmptyNodes.topLeft,
                                    bNeighborEmptyNodes.topRight, emptyNodes.bottomRight);
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
                    Object.Destroy(_roads[x, y].GetComponent<EmptyNodes>());
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


        private bool IsSpecial(GameObject neighbor, int[] cases)
        {
            return Array.Exists(cases, e => e == GetRoadType(neighbor));
        }
    }

  
}