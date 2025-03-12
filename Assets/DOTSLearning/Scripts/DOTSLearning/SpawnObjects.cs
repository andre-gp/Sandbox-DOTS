using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(TransformSystemGroup))]
[UpdateBefore(typeof(ParentSystem))]
partial struct SpawnObjects : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var createdEcb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var spawnJob = new SpawnJob {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = createdEcb
        };

        spawnJob.Schedule();

    }

    [BurstCompile]
    public partial struct SpawnJob : IJobEntity {
        public float deltaTime;

        public EntityCommandBuffer ecb;

        public void Execute([EntityIndexInQuery] int entityIndexInQuery, ref LocalTransform localTransform, ref Spawner spawner) {
            spawner.frequency -= deltaTime;

            localTransform.Position += new float3(0, 0, 10) * deltaTime;

            localTransform = localTransform.RotateY(10 * deltaTime);

            if (spawner.frequency <= 0) {
                spawner.frequency = 0.01f;

                var cube = ecb.Instantiate(spawner.cube);
                ecb.SetComponent(cube, LocalTransform.FromPosition(localTransform.Position));

                var sphere = ecb.Instantiate(spawner.sphere);
                ecb.SetComponent(sphere, LocalTransform.FromPosition(localTransform.Position + localTransform.Right() * 3));

                spawner.spawnedCount += 2;
            }
        }

        
    }
}

