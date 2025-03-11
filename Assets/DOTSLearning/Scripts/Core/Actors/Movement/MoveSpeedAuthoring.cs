using UnityEngine;
using Unity.Entities;

namespace DOTSLearningCore
{
    public class MoveSpeedAuthoring : MonoBehaviour
    {
        public float moveSpeed;

        public class Baker : Baker<MoveSpeedAuthoring> {
            public override void Bake(MoveSpeedAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new MoveSpeed {
                    moveSpeed = authoring.moveSpeed
                });
            }
        }
    }

    public struct MoveSpeed : IComponentData {
        public float moveSpeed;

    }
}

