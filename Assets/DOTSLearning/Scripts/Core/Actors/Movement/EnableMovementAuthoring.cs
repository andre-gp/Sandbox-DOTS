using Unity.Entities;
using UnityEngine;

namespace DOTSLearningCore
{
    class EnableMovementAuthoring : MonoBehaviour
    {
        public GameObject enabledGraphics;
        public bool startEnabled = false;

        class Baker : Baker<EnableMovementAuthoring> {
            public override void Bake(EnableMovementAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnableMovement {
                    enabledGraphics = GetEntity(authoring.enabledGraphics, TransformUsageFlags.None)
                });
                SetComponentEnabled<EnableMovement>(entity, authoring.startEnabled);
            }
        }
    }

    public struct EnableMovement : IComponentData, IEnableableComponent {
        public Entity enabledGraphics;
    }
}
