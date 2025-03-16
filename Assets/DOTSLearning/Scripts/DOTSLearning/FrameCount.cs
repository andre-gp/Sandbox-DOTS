using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
public partial struct FrameCount : ISystem, ISystemStartStop
{
    int count;


    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        count += 1;

        Debug.Log($"{count} - {Time.frameCount}");
    }

    void ISystemStartStop.OnStartRunning(ref SystemState state) {
        count = 0;
    }

    public void OnStopRunning(ref SystemState state) {
        
    }
}
