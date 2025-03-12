using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Debug = UnityEngine.Debug;

namespace DOTSLearningCore {
    public partial struct UnitMoverSystem : ISystem , ISystemStartStop{
        public void OnCreate(ref SystemState state) {
            
        }

        public void OnStartRunning(ref SystemState state) {
            Debug.Log("Started Running!");
        }

        public void OnStopRunning(ref SystemState state) {
            Debug.Log("Stopped Running!");
        }

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
