using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts
{
    public class MapGenerator
    {
        private Map _map;
        private int _size;
        private TextMeshProUGUI text;
        private Tile[] tiles;
        private GridLayoutGroup grid;
        private Texture2D[] textures;
        private GameObject[,] roads;

        private GameObject _trafficSystem;
        float tileSize = 24f;


        public UnityAction AdjustRoadLamps;

        public MapGenerator(int seed, int size)
        {
            _size = size;
            if (seed != 0)
                Random.InitState(seed);
            _map = new Map(_size);

            _trafficSystem = Object.FindObjectOfType<TrafficSystem>().gameObject;

            JSONToTiles();
            _map.Generate(tiles);
            // PrintMap(_map.map);
            // DrawMap(_map.map);
        }

        private void JSONToTiles()
        {
            string json = File.ReadAllText("./roads.json");
            tiles = JsonHelper.FromJsonGetArray<Tile>(json);
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

        private void DrawMap(MapTile[,] map)
        {
            grid.cellSize = new Vector2(grid.GetComponent<RectTransform>().sizeDelta.x / _size,
                grid.GetComponent<RectTransform>().sizeDelta.y / _size);
            grid.GetComponent<RectTransform>().anchoredPosition = (grid.GetComponent<RectTransform>().sizeDelta / 2 +
                                                                   new Vector2(100, 100)) * -1;
            grid.constraintCount = _size;

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
            var loadedObject = Resources.Load($"Prefabs/Roads/{id}");
            if (loadedObject == null)
            {
                return Resources.Load("Prefabs/Roads/0") as GameObject;
            }

            return Object.Instantiate(loadedObject as GameObject);
        }

        internal List<TrafficSystemNode> DrawRoads()
        {
            List<TrafficSystemNode> connectors = new List<TrafficSystemNode>();

            for (int y = 0; y < _map.map.GetLength(0); y++)
            {
                for (int x = 0; x < _map.map.GetLength(1); x++)
                {
                    Tile tile = _map.map[x, y].tile;

                    GameObject road = GetRoadPrefab(tile.id);

                    _map.map[x, y].road = road;
                    
                    road.transform.position = new Vector3(x * tileSize, 0, y * -tileSize);
                    road.transform.SetParent(_trafficSystem.transform);
                    connectors.AddRange(road.GetComponentsInChildren<TrafficSystemNode>().Where(p=>p.m_roadType == TrafficSystem.RoadType.LANES_2));
                }
            }

            _map = drawSquareRoad();
            _size = _map.map.GetLength(0);

            RoadConnector roadConnector = new RoadConnector(_map.map);
            roadConnector.ConnectRoads();
            AdjustRoadLamps();

           CombineMeshes();
            
            return connectors;
        }

        private void CombineMeshes()
        {
            var pieces = _trafficSystem.GetComponentsInChildren<TrafficSystemPiece>();

            var meshes = new List<MeshFilter>();
            foreach (var piece in pieces)
            {
               var model= piece.transform.Find("Models");
               var mesh = model.gameObject.GetComponentsInChildren<MeshFilter>();
               meshes.AddRange(mesh);
            }
            
           

            var mergedRoad = _trafficSystem;
            
            var road_mesh = mergedRoad.AddComponent<MeshFilter>();
            road_mesh.mesh = new Mesh();
            road_mesh.mesh.CombineMeshes(Combine(meshes.ToArray()),true,true);
            mergedRoad.GetComponent<Renderer>().enabled = false;
            mergedRoad.gameObject.SetActive(true);
            var meshCollider = mergedRoad.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = road_mesh.mesh;
            

        }


        public Rect[] findEmptySpaces()
        {
            var collider = _trafficSystem.GetComponent<MeshCollider>();
        }

        private CombineInstance[] Combine(MeshFilter[] filters)
        {
            List<CombineInstance> combine = new List<CombineInstance> ();
 
            for (int i = 0; i < filters.Length; i++)
            {
                // skip the empty parent GO
                if (filters[i].sharedMesh == null)
                    continue;
 
                // combine submeshes
                for (int j = 0; j < filters[i].sharedMesh.subMeshCount; j++)
                {
                    CombineInstance ci = new CombineInstance ();
 
                    ci.mesh = filters[i].sharedMesh;
                    ci.subMeshIndex = j;
                    ci.transform = filters[i].transform.localToWorldMatrix;
 
                    combine.Add (ci);
                }                
 
                // disable child mesh GO-s
                //filters[i].gameObject.SetActive (false);
            }

            return combine.ToArray();

        }

        private int getIdOfMap(int x, int y)
        {
            return _map.map[x, y].tile.id;
        }
        
        private MapTile getMapTile(int x, int y)
        {
            return _map.map[x, y];
        }

        private Map drawSquareRoad()
        {
            Map map = new Map(_size + 2);
            map.map = new MapTile[_size + 2, _size + 2];
            var mapTile = map.map;

            int[] tconnectTiles = new[] {2, 3, 6, 7, 8, 9, 10};
            for (int x = -1; x < _size + 1; x++)
            {
                int id = 1;
                if (x == -1) id = 4;
                if (x == _size) id = 5;
                if (x >= 0 && x < _size && tconnectTiles.Contains(getIdOfMap(x, 0))) id = 11;
                
                GameObject road = GetRoadPrefab(id);

                road.transform.position = new Vector3(tileSize * x, 0, tileSize);
                road.transform.SetParent(_trafficSystem.transform);
                
                mapTile[x + 1, 0] = new MapTile(tiles[id],road);
            }

            tconnectTiles = new[] {2, 3, 4, 5, 8, 10, 11};
            for (int x = -1; x < _size + 1; x++)
            {
                int id = 1;
                if (x == -1) id = 7;
                if (x == _size) id = 6;
                if (x >= 0 && x < _size && tconnectTiles.Contains(getIdOfMap(x, _size - 1))) id = 9;
                
            

                GameObject road = GetRoadPrefab(id);

                road.transform.position = new Vector3(tileSize * x, 0, -tileSize * _size);
                road.transform.SetParent(_trafficSystem.transform);
                
                mapTile[x + 1, _size+1] = new MapTile(tiles[id],road);
            }

            tconnectTiles = new[] {1, 3, 5, 6, 8, 9, 11};
            for (int x = 0; x < _size; x++)
            {
                int id = 2;
                if (tconnectTiles.Contains(getIdOfMap(0, x))) id = 10;

            
                GameObject road = GetRoadPrefab(id);
                road.transform.position = new Vector3(-tileSize, 0, -tileSize * x);
                road.transform.SetParent(_trafficSystem.transform);
                
                mapTile[0, x+1] = new MapTile(tiles[id],road);
                
            }

            tconnectTiles = new[] {1, 3, 4, 7, 9, 10, 11};
            for (int x = 0; x < _size; x++)
            {
                int id = 2;
                if (tconnectTiles.Contains(getIdOfMap(_size - 1, x))) id = 8;
               

                GameObject road = GetRoadPrefab(id);
                road.transform.position = new Vector3(tileSize * _size, 0, -tileSize * x);
                road.transform.SetParent(_trafficSystem.transform);
                
                mapTile[_size+1, x+1] = new MapTile(tiles[id],road);
            }

            for (int x = 1; x < mapTile.GetLength(0)-1; x++)
            {
                for (int y = 1; y < mapTile.GetLength(0)-1; y++)
                {
                    mapTile[x, y] = getMapTile(x-1,y-1);
                }
            }

            return map;
        }

        private void PrintMap(MapTile[,] map)
        {
            string rows = "";
            for (int x = 0; x < map.GetLength(0); x++)
            {
                string row = "";
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    string value = "0";
                    if (map[x, y] != null)
                    {
                        value = map[x, y].ToString();
                    }

                    row = row + $"{value,3}";
                }

                rows = rows + row + "\n";
            }

            Debug.Log(rows);
        }
    }
}