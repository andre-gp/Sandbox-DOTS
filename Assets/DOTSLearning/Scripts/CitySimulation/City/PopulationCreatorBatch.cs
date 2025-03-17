using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Random = Unity.Mathematics.Random;
using Unity.Entities;
using Unity.Burst;
using System;
using System.Collections.Generic;
using System.Collections;

public class PopulationCreatorBatch : CreatorBase {
    bool isRunning = false;

    FillBufferResult[] buffers;

    Entity prototype;

    int entitiesPerBatch;
    int currentCount;
    int maxCount;

    string guiPopCountText;

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


        prototype = entityManager.CreateEntity(new ComponentType[] {
            typeof(Status)
        });

        buffers = new FillBufferResult[threadCount];

        entitiesPerBatch = populationCount / threadCount;
        maxCount = populationCount;

        for (int i = 0; i < threadCount; i++) {
            
            var ecb = new EntityCommandBuffer(Allocator.Persistent);

            var job = new InstantiateJob {
                length = entitiesPerBatch,
                entityPrefab = prototype,
                ecb = ecb,
                random = new Random((uint)(UnityEngine.Random.value * uint.MaxValue)),
                minMaxVal = new int2(20, 100)
            };

            var jobHandle = job.Schedule();

            buffers[i] = new FillBufferResult {
                ecb = ecb,
                handle = jobHandle
            };
        }

        
    }

    private void Update() {

        if (!isRunning)
            return;

        for (int i = 0; i < buffers.Length; i++) {
            if (!buffers[i].handle.IsCompleted)
                return;
        }

        isRunning = false;
        StartCoroutine(PlaybackECBs());
    }

    IEnumerator PlaybackECBs() {

        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int i = 0; i < buffers.Length; i++) {
            buffers[i].handle.Complete(); // Ensure every job is completed.
        }

        for (int i = 0; i < buffers.Length; i++) {
            buffers[i].ecb.Playback(entityManager);
            buffers[i].ecb.Dispose();

            yield return new WaitForSeconds(timeBetweenBatches);

            currentCount = i * entitiesPerBatch;                       
        }

        entityManager.DestroyEntity(prototype);

        if (pauseOnFinish) {
            Debug.Break();
        }

    }

    

    private void OnGUI() {

        GUI.BeginGroup(new Rect(10, 10, 800, 800));

        guiPopCountText = GUI.TextField(new Rect(0, 0, 150, 20), guiPopCountText);

        GUI.enabled = !isRunning;
        if (GUI.Button(new Rect(25f, 35, 100, 30), "Create!")) {
            populationCount = int.Parse(guiPopCountText);
            CreatePopulation();
        }
        GUI.enabled = true;

        if(maxCount != 0)
            GUI.Label(new Rect(0, 50, 300, 50), $"{currentCount.ToString("n0")}/{maxCount.ToString("n0")}");

        GUI.EndGroup();
    }

    [BurstCompile]
    private struct InstantiateJob : IJob {
        public int length;
        public Entity entityPrefab;
        public EntityCommandBuffer ecb;

        public Random random;
        public int2 minMaxVal;

        public void Execute() {
            for (int i = 0; i < length; i++) {
                var instantiatedEntity = ecb.Instantiate(entityPrefab);

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
            }            
        }
    }
}

struct FillBufferResult {
    public EntityCommandBuffer ecb;
    public JobHandle handle;
}