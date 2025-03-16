using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct RotatorSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, rotationData) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationData>>()) {
            localTransform.ValueRW = localTransform.ValueRW.RotateY(rotationData.ValueRO.rotationSpeed * SystemAPI.Time.DeltaTime);
        }        
    }
}
