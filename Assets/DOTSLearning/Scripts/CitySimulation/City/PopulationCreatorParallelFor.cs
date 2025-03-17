using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Random = Unity.Mathematics.Random;
using System;
using System.Threading.Tasks;

public class PopulationCreatorParallelFor : CreatorBase
{
    JobHandle jobHandle;

    bool isRunning = false;

    EntityCommandBuffer ecb;

    Entity prototype;

    ParallelLoopResult result;

    private void Start() {
        if (createOnStart) {
            CreatePopulation();
        }
    }

    public void CreatePopulation() {
        isRunning = true;

        FillBuffer();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CreatePopulation();
        }

        if (!isRunning)
            return;

        framesToRun -= 1;

        if (framesToRun < 0) {

            if (!result.IsCompleted) {
                framesToRun = 1;
                return;
            }

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

    void FillBuffer() {
        World world = World.DefaultGameObjectInjectionWorld;

        EntityManager entityManager = world.EntityManager;

        ecb = new EntityCommandBuffer(Allocator.Persistent);
        //var parallelEcb = ecb;


        prototype = entityManager.CreateEntity(new ComponentType[] {
            typeof(Status)
        });

        var random = new Random((uint)(UnityEngine.Random.value * uint.MaxValue));
        var minMaxVal = new int2(20, 100);

        result = Parallel.For(0, populationCount + 1, index => {
            var instantiatedEntity = ecb.Instantiate(prototype);

            ecb.SetComponent(instantiatedEntity, new Status {
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
        });

    }

    
}
