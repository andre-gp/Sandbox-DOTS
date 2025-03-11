using Unity.Entities;
using UnityEngine;

class SpawnerAuthoring : MonoBehaviour {
    public GameObject sphere;
    public GameObject cube;

    class Baker : Baker<SpawnerAuthoring> {
        public override void Bake(SpawnerAuthoring authoring) {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Spawner {
                sphere = GetEntity(authoring.sphere, TransformUsageFlags.None),
                cube = GetEntity(authoring.cube, TransformUsageFlags.None),
                frequency = 0,
                spawnedCount = 0
            });
        }
    }

    
}

public struct Spawner : IComponentData {
    public Entity sphere;
    public Entity cube;

    public float frequency;

    public int spawnedCount;
}
