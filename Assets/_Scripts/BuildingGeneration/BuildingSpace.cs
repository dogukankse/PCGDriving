
using ProceduralToolkit.Examples.Buildings;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class BuildingSpace : MonoBehaviour
    {
        Bounds bound = new Bounds();
        private GameObject proceduralBuilding;

        private void Start()
        {
            var collider = GetComponent<BoxCollider>();
            var bounds = collider.bounds;
            var rect = new Rect(new Vector2(-bounds.extents.z, -bounds.extents.x),
                new Vector2(bounds.extents.z * 2, bounds.extents.x * 2));


            PolygonAsset asset = ScriptableObject.CreateInstance<PolygonAsset>();
            asset.vertices.Add(new Vector2(rect.top, rect.left));
            asset.vertices.Add(new Vector2(rect.top, rect.right));
            asset.vertices.Add(new Vector2(rect.bottom, rect.right));
            asset.vertices.Add(new Vector2(rect.bottom, rect.left));


            proceduralBuilding = GetProceduralBuilding();

            BuildingGeneratorComponent component = proceduralBuilding.GetComponent<BuildingGeneratorComponent>();
            component.foundationPolygon = asset;
            component.config.floors = Random.Range(2, 7);
            component.config.palette.wallColor = Random.ColorHSV();
            var building = component.generate().gameObject;
            building.AddComponent<IOClod>().Static = true;
        }


        private GameObject GetProceduralBuilding()
        {
            var loadedObject = Resources.Load($"Prefabs/Building/BuildingGeneratorComponent");
            return Object.Instantiate(loadedObject as GameObject, transform);
        }
    }
}