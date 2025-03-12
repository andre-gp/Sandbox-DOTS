using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ParallGenerateNums : MonoBehaviour
{
    [SerializeField] int count = 100000;

    private void Start() {
        Stopwatch stopWatch = new Stopwatch();

        stopWatch.Start();
        
        NativeArray<int> generatedNums = new NativeArray<int>(count, Allocator.TempJob);

        var job = new ParallelGenerateNames {
            results = generatedNums,
            random = new Unity.Mathematics.Random((uint)(UnityEngine.Random.value * uint.MaxValue))
        };

        var jobHandle = job.Schedule(count, count / 20);

        jobHandle.Complete();

        //for (int i = 0; i < count; i++) {
        //    Debug.Log(generatedNums[i]);
        //}
      
        stopWatch.Stop();

        generatedNums.Dispose();

        Debug.Log($"({count}) Parallel Elapsed time: {stopWatch.Elapsed}");

        Debug.Break();
    }
}

public struct ParallelGenerateNames : IJobParallelFor {
    public NativeArray<int> results;
    public Unity.Mathematics.Random random;

    public void Execute(int index) {
        results[index] = random.NextInt(0, 1000);
    }
}