using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    public class BuildingGeneratorComponent : MonoBehaviour
    {
        [SerializeField]
        private FacadePlanningStrategy facadePlanningStrategy;
        [SerializeField]
        private FacadeConstructionStrategy facadeConstructionStrategy;
        [SerializeField]
        private RoofPlanningStrategy roofPlanningStrategy;
        [SerializeField]
        private RoofConstructionStrategy roofConstructionStrategy;
        [SerializeField]
        public PolygonAsset foundationPolygon;
        [SerializeField]
        public BuildingGenerator.Config config = new BuildingGenerator.Config();

        private BuildingGenerator generator;
        
        public Transform generate()
        {
            generator = new BuildingGenerator();
            generator.SetFacadePlanningStrategy(facadePlanningStrategy);
            generator.SetFacadeConstructionStrategy(facadeConstructionStrategy);
            generator.SetRoofPlanningStrategy(roofPlanningStrategy);
            generator.SetRoofConstructionStrategy(roofConstructionStrategy);
            return generator.Generate(foundationPolygon.vertices, config, transform);
        }

    }
}
