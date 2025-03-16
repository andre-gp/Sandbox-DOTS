using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTSLearningCore {
    readonly partial struct UnitAspect : IAspect
    {
        readonly public RefRW<LocalTransform> localTransform;
        readonly public RefRO<MoveSpeed> moveSpeed;

        public void Move(float3 pos) {

        }
    }
}
