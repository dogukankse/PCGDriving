using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        private Map _map;
        public int size;
        public TextMeshProUGUI text;
        public Tile[] tiles;
        public int seed = 0;
        public GridLayoutGroup grid;
        public Texture2D[] textures;
        public GameObject[,] roads;
        public GameObject automobile;

        public Tile[] deneme;

        public UnityAction AdjustRoadLamps;

        private void Start()
        {
            if (seed != 0)
                Random.InitState(seed);
            _map = new Map(size);
            foreach (Tile tile in tiles)
            {
                foreach (string s in tile.downS.Split(' ').ToList())
                {
                    tile.down.Add(int.Parse(s));
                }

                foreach (string s in tile.leftS.Split(' ').ToList())
                {
                    tile.left.Add(int.Parse(s));
                }
            }

            _map.Generate(tiles);
            PrintMap(_map.map);
            DrawMap(_map.map);
            DrawRoads(_map.map);

            //TilesToJSON();
            JSONToTiles();
        }

        private void JSONToTiles()
        {
            string json = File.ReadAllText("./roads.json");
            deneme = JsonHelper.FromJsonGetArray<Tile>(json);
        }

        private void TilesToJSON()
        {
            string fileText = "[";
            for (int i = 0; i < tiles.Length; i++)
            {
                string json = JsonUtility.ToJson(tiles[i]);
                if (i != tiles.Length - 1)
                    fileText += json + ",";
                else fileText += json + "]";
            }
            File.WriteAllText("./roads.json", fileText);
        }

        /* public void drawTest()
         {
             GameObject road = GetRoadPrefab(6);
             GameObject road2 = GetRoadPrefab(6);
 
             road2.gameObject.name = "changed";
             road2.transform.position += new Vector3(0, 0, 25f);
             var a = road2.GetComponentsInChildren<TrafficSystemNode>().Where(p => p.m_connectedNodes.Count == 0)
                 .ToList()[0];
             ChangeDriverSide(a);
         }*/

        private void DrawMap(MapTile[,] map)
        {
            grid.cellSize = new Vector2(grid.GetComponent<RectTransform>().sizeDelta.x / size,
                grid.GetComponent<RectTransform>().sizeDelta.y / size);
            grid.GetComponent<RectTransform>().anchoredPosition = (grid.GetComponent<RectTransform>().sizeDelta / 2 +
                                                                   new Vector2(100, 100)) * -1;
            grid.constraintCount = size;

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Tile tile = map[x, y].tile;
                    GameObject go = new GameObject(tile.id.ToString()) {layer = 5};
                    Image image = go.AddComponent<Image>();
                    Texture2D t = textures[tile.id];
                    image.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
                    go.transform.SetParent(grid.transform);
                }
            }
        }

        private GameObject GetRoadPrefab(int id)
        {
            var loadedObject = Resources.Load($"Prefabs/{id}");
            if (loadedObject == null)
            {
                return Resources.Load("Prefabs/0") as GameObject;
            }

            return Instantiate(loadedObject as GameObject);
        }

        private void DrawRoads(MapTile[,] map)
        {
            List<TrafficSystemNode> intersectionNodes = new List<TrafficSystemNode>();
            List<TrafficSystemNode> emptyNodes = new List<TrafficSystemNode>();
            roads = new GameObject[map.GetLength(0), map.GetLength(1)];
            float tileSize = 24f;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Tile tile = map[x, y].tile;

                    GameObject road = GetRoadPrefab(tile.id);

                    road.transform.position = new Vector3(x * tileSize, 0, y * -tileSize);
                    road.transform.SetParent(transform);
                    roads[x, y] = road;

                    /*var nodes = road.GetComponentsInChildren<TrafficSystemNode>().Where(
                    p => p.m_roadType == TrafficSystem.RoadType.LANES_2 && p.m_connectedNodes.Count == 0).ToList();
                if (tile.id == 3 || tile.id == 8 || tile.id == 9 || tile.id == 10 || tile.id == 11)
                {
                    intersectionNodes.AddRange(nodes);
                }

                emptyNodes.AddRange(nodes);*/
                }
            }

            //connect intersections
            /*foreach (var node in intersectionNodes)
        {
            var nearestNode = getNearestNode(node);
            if (!nearestNode)
            {
                continue;
            }

            if (node.m_driveSide != nearestNode.m_driveSide)
            {
                ChangeDriverSide(nearestNode);
            }

            if (nearestNode.m_connectedNodes.Count == 0)
            {
                nearestNode.AddConnectedNode(node);
            }
            else
            {
                node.AddConnectedNode(nearestNode);
            }
        }*/

            RoadConnector roadConnector = new RoadConnector(map, roads);
            roadConnector.ConnectRoads();
            AdjustRoadLamps();
        }

        /*  private void ChangeDriverSide(TrafficSystemNode node)
          {
              GameObject parent = node.gameObject;
              while (parent.name != "Road")
              {
                  parent = parent.transform.parent.gameObject;
              }
  
              var nodes = parent.GetComponentsInChildren<TrafficSystemNode>();
  
              foreach (var n in nodes)
              {
                  if (!n.m_isPrimary)
                  {
                      Destroy(n.gameObject);
                  }
              }
  
              var endNodes = nodes.Where(p => p.m_connectedNodes.Count == 0).ToList();
              var searchingNodes = new List<TrafficSystemNode>(nodes);
              searchingNodes.RemoveAll(p => endNodes.Contains(p));
  
              List<List<TrafficSystemNode>> paths = new List<List<TrafficSystemNode>>();
              foreach (var n in endNodes)
              {
                  List<TrafficSystemNode> path = new List<TrafficSystemNode>();
                  TrafficSystemNode start = n;
                  path.Add(start);
                  var who = searchingNodes
                      .Where(p => p.m_connectedNodes.Contains(start) || p.m_connectedLocalNode == start)
                      .ToList();
                  while (who.Any())
                  {
                      start = who[0];
                      path.Add(start);
                      who = searchingNodes.Where(p =>
                              p.m_connectedNodes.Contains(start) || p.m_connectedLocalNode == start)
                          .ToList();
                  }
  
                  paths.Add(path);
              }
  
              foreach (var path in paths)
              {
                  for (int i = 0; i < path.Count; i++)
                  {
                      path[i].m_connectedLocalNode = null;
                      path[i].m_connectedNodes.Clear();
                      if (i + 1 < path.Count)
                      {
                          var newConnect = path[i + 1];
                          path[i].AddConnectedNode(newConnect);
                      }
                  }
              }
  
              foreach (var trafficSystemNode in nodes)
              {
                  if (trafficSystemNode.m_driveSide == TrafficSystem.DriveSide.LEFT)
                  {
                      trafficSystemNode.m_driveSide = TrafficSystem.DriveSide.RIGHT;
                  }
                  else
                  {
                      trafficSystemNode.m_driveSide = TrafficSystem.DriveSide.LEFT;
                  }
  
                  trafficSystemNode.RefreshNodeMaterial();
              }
  
              Debug.Log("");
          }
  
          private TrafficSystemNode getNearestNode(TrafficSystemNode n, bool sameDirection = false)
          {
              List<TrafficSystemNode> nodes = GetComponentsInChildren<TrafficSystemNode>().Where(p =>
                  p.m_roadType == TrafficSystem.RoadType.LANES_2 &&
                  p.transform.position != n.transform.position &&
                  p.m_isPrimary &&
                  p.transform.parent.parent.parent.gameObject != n.transform.parent.parent.parent.gameObject
              ).ToList();
  
              if (sameDirection)
              {
                  nodes = nodes.Where(p => p.m_driveSide == n.m_driveSide).ToList();
              }
  
              TrafficSystemNode bestNode = nodes[0];
              float bestDistance = 99999;
              foreach (var node in nodes)
              {
                  var distance = Vector3.Distance(node.transform.position, n.transform.position);
                  if (distance < bestDistance)
                  {
                      bestDistance = distance;
                      bestNode = node;
                  }
              }
  
              if (bestDistance > 12) return null;
              return bestNode;
          }
  */

        private void PrintMap(MapTile[,] map)
        {
            for (int y = 0;
                y < map.GetLength(0);
                y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    text.text += map[x, y].tile.id + " ";
                }

                text.text += "\n";
            }
        }
    }
}