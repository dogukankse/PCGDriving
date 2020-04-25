using System.Collections;
using ProceduralToolkit.Examples.Buildings;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Scripts.BuildingGeneration
{
    public class BuildingSpace : MonoBehaviour
    {
        private GameObject proceduralBuilding;

        private void Start()
        {
            StartCoroutine(CreateBuilding());
        }

        private IEnumerator CreateBuilding()
        {
            var boxCollider = GetComponent<BoxCollider>();
            var bounds = boxCollider.bounds;
            var rect = new Rect(new Vector2(-bounds.extents.z, -bounds.extents.x),
                new Vector2(bounds.extents.z * 2, bounds.extents.x * 2));


            PolygonAsset asset = ScriptableObject.CreateInstance<PolygonAsset>();
            asset.vertices.Add(new Vector2(rect.yMin, rect.xMin));
            asset.vertices.Add(new Vector2(rect.yMin, rect.xMax));
            asset.vertices.Add(new Vector2(rect.yMax, rect.xMax));
            asset.vertices.Add(new Vector2(rect.yMax, rect.xMin));


            proceduralBuilding = GetProceduralBuilding();

            BuildingGeneratorComponent component = proceduralBuilding.GetComponent<BuildingGeneratorComponent>();
            component.foundationPolygon = asset;
            component.config.floors = Random.Range(2, 7);
            component.config.palette.wallColor = Random.ColorHSV();
            var building = component.generate().gameObject;
            building.isStatic = true;
            building.AddComponent<IOClod>().Static = true;
            yield return new WaitForSeconds(0.1f);
        }

        private GameObject GetProceduralBuilding()
        {
            var loadedObject = Resources.Load($"Prefabs/Building/BuildingGeneratorComponent");
            return Instantiate(loadedObject as GameObject, transform);
        }
    }
}