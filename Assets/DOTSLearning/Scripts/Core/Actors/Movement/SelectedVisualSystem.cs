using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace DOTSLearningCore
{
    partial struct SelectedVisualSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var enableMovement in SystemAPI.Query<RefRO<EnableMovement>>()) {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(enableMovement.ValueRO.enabledGraphics, true);                
            }

            foreach (var enableMovement in SystemAPI.Query<RefRO<EnableMovement>>().WithDisabled<EnableMovement>()) {
                SystemAPI.SetComponentEnabled<MaterialMeshInfo>(enableMovement.ValueRO.enabledGraphics, false);
            }

        }
    }

    partial struct SelectedVisualJob : IJobEntity {
        void Execute(in EnableMovement enableMovement) {

        }

    }
}
