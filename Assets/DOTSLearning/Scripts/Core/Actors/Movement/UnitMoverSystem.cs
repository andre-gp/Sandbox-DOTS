using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DOTSLearningCore {
    partial struct UnitMoverSystem : ISystem {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            UnitMoverJob unitMoverJob = new UnitMoverJob {
                deltaTime = SystemAPI.Time.DeltaTime
            };

            unitMoverJob.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity {

        public float deltaTime;

        public void Execute(ref LocalTransform localTransform, in MoveSpeed moveSpeed, in EnableMovement enableMovement) {
            localTransform.Position = localTransform.Position + new float3(moveSpeed.moveSpeed, 0, 0) * deltaTime;
        }

    }
}
