using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SingleThreadGenerateNames : MonoBehaviour
{
    [SerializeField] int count = 100000;
    private void Start() {
        Stopwatch stopWatch = new Stopwatch();

        stopWatch.Start();

        NativeArray<int> generatedNums = new NativeArray<int>(count, Allocator.Temp);

        for (int i = 0; i < count; i++) {
            generatedNums[i] = Random.Range(0, 1000);
        }

        stopWatch.Stop();

        Debug.Log($"({count}) Single-Thread Elapsed time: {stopWatch.Elapsed}");

        Debug.Break();
    }

}
