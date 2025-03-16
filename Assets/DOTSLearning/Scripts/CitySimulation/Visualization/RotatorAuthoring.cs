using Unity.Entities;
using UnityEngine;

class RotatorAuthoring : MonoBehaviour
{
    [SerializeField] private float speed = 100;
    class Baker : Baker<RotatorAuthoring> {
        public override void Bake(RotatorAuthoring authoring) {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotationData {
                rotationSpeed = authoring.speed
            });
        }
    }
}

public struct RotationData : IComponentData {
    public float rotationSpeed;
}

