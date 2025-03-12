using DOTSLearningCore;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(MySystemGroup))]
partial struct EntityAccessSystemTest : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var buffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        //EntityCommandBuffer buffer = new EntityCommandBuffer(Allocator.Temp);



        foreach (var (mSpeed, entity) in SystemAPI.Query<RefRO<MoveSpeed>>().WithEntityAccess()) {
            if(mSpeed.ValueRO.moveSpeed >= 20) {
                buffer.DestroyEntity(entity);
            }
        }

        // When creating a buffer through the "EndSimulationEntityCommandBufferSystem -> CreateCommandBuffer"
        // The buffer doesn't need to be manually called using .Playback
        // It will be automatically run at the end of the simulation.
        //buffer.Playback(state.EntityManager);

    }
}
