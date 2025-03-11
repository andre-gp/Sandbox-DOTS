using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateBefore(typeof(ParentSystem))]
partial struct SpawnObjects : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, spawner) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Spawner>>()) {
            spawner.ValueRW.frequency -= Time.deltaTime;

            localTransform.ValueRW.Position += new float3(0, 0, 10) * Time.deltaTime;

            localTransform.ValueRW = localTransform.ValueRW.RotateY(10 * Time.deltaTime);

            if (spawner.ValueRO.frequency <= 0) {
                spawner.ValueRW.frequency = 0.01f;

                var cube = state.EntityManager.Instantiate(spawner.ValueRO.cube);
                SystemAPI.SetComponent(cube, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                
                var sphere = state.EntityManager.Instantiate(spawner.ValueRO.sphere);
                SystemAPI.SetComponent(sphere, LocalTransform.FromPosition(localTransform.ValueRO.Position + localTransform.ValueRO.Right() * 3));

                spawner.ValueRW.spawnedCount += 2;
            }           
        }
        
    }

}

