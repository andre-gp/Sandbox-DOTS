using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Random = Unity.Mathematics.Random;

public class PopulationCreatorParallelFor : CreatorBase
{
    JobHandle jobHandle;

    bool isRunning = false;

    EntityCommandBuffer ecb;

    Entity prototype;

    private void Start() {
        if (createOnStart) {
            CreatePopulation();
        }
    }

    public void CreatePopulation() {
        isRunning = true;

        World world = World.DefaultGameObjectInjectionWorld;

        EntityManager entityManager = world.EntityManager;

        // Using this way would automatically build the entities at the end of the frame:
        //var entityQuery = new EntityQueryBuilder(Allocator.Temp)
        //                  .WithAll<EndSimulationEntityCommandBufferSystem.Singleton>().Build(entityManager);

        //entityQuery.TryGetSingleton(out EndSimulationEntityCommandBufferSystem.Singleton singleton);
        //var ecb = singleton.CreateCommandBuffer(world.Unmanaged);

        ecb = new EntityCommandBuffer(Allocator.Persistent);


        prototype = entityManager.CreateEntity(new ComponentType[] {
            typeof(Status)
        });

        var job = new InstantiateJob {
            entityPrefab = prototype,
            ecb = ecb.AsParallelWriter(),
            random = new Random((uint)(UnityEngine.Random.value * uint.MaxValue)),
            minMaxVal = new int2(20, 100)
        };

        jobHandle = job.Schedule(populationCount, populationCount / threadCount);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CreatePopulation();
        }

        if (!isRunning)
            return;

        framesToRun -= 1;

        if (framesToRun < 0) {
            jobHandle.Complete();

            //var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //ecb.Playback(entityManager);
            ecb.Dispose();
            //entityManager.DestroyEntity(prototype);

            isRunning = false;

            if (pauseOnFinish) {
                Debug.Break();
            }
        }
    }

    [BurstCompile]
    private struct InstantiateJob : IJobParallelFor {
        public Entity entityPrefab;
        public EntityCommandBuffer.ParallelWriter ecb;

        public Random random;
        public int2 minMaxVal;

        public void Execute(int index) {
            var instantiatedEntity = ecb.Instantiate(index, entityPrefab);

            ecb.SetComponent(index, instantiatedEntity, new Status {
                hunger = random.NextInt(minMaxVal.x, minMaxVal.y),
                thirst = random.NextInt(minMaxVal.x, minMaxVal.y),
                sleep = random.NextInt(minMaxVal.x, minMaxVal.y),
                bladder = random.NextInt(minMaxVal.x, minMaxVal.y),
                hygiene = random.NextInt(minMaxVal.x, minMaxVal.y),
                energy = random.NextInt(minMaxVal.x, minMaxVal.y),
                health = random.NextInt(minMaxVal.x, minMaxVal.y),
                fun = random.NextInt(minMaxVal.x, minMaxVal.y),
                social = random.NextInt(minMaxVal.x, minMaxVal.y),
                stress = random.NextInt(minMaxVal.x, minMaxVal.y),
                temperature = random.NextInt(minMaxVal.x, minMaxVal.y),
                intoxication = random.NextInt(minMaxVal.x, minMaxVal.y),
                fear = random.NextInt(minMaxVal.x, minMaxVal.y)
            });
        }
    }
}
