using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace DOTSLearningCore {
    class DatabaseAuthoring : MonoBehaviour {
        [SerializeField] GameObject unitPrefab;

        class DatabaseAuthoringBaker : Baker<DatabaseAuthoring> {
            public override void Bake(DatabaseAuthoring authoring) {
                AddComponent(GetEntity(TransformUsageFlags.None), new Database {
                    //unitPrefab = GetEntity(authoring.unitPrefab, TransformUsageFlags.None)
                    unitPrefab = new EntityPrefabReference(authoring.unitPrefab)
                });

            }
        }
    }

    struct Database : IComponentData {
        public EntityPrefabReference unitPrefab;
    }
}
