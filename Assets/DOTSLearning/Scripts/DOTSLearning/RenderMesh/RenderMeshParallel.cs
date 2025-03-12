using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class RenderMeshParallel : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private Mesh mesh;

    [SerializeField] private int rowCount = 2;
    [SerializeField] private int columnCount = 4;
    [SerializeField] private float offset = 0.2f;

    [SerializeField] private int totalCount;

    RenderParams rp;

    private void Start() {
        rp = new RenderParams(mat);
        
    }

    private void Update() {
        totalCount = rowCount * columnCount;
        NativeArray<Matrix4x4> positions = new NativeArray<Matrix4x4>(totalCount, Allocator.TempJob);

        var job = new FillPositionsJob {
            startPos = transform.position,
            offset = this.offset,
            lenght = new int2(rowCount, columnCount),
            positions = positions
        };

        JobHandle jobHandle = job.Schedule(totalCount, totalCount / 20);

        jobHandle.Complete();
        
        //for (int x = 0; x < rowCount; x++) {
        //    for (int y = 0; y < columnCount; y++) {
        //        positions[(x * columnCount) + y] = Matrix4x4.Translate(transform.position + new Vector3(x * offset, 0, y * offset));
        //    }
        //}
        Graphics.RenderMeshInstanced(rp, mesh, 0, positions);
        //Graphics.RenderMeshInstanced(rp, mesh, 0, ));

        positions.Dispose();
        
    }

    [BurstCompile]
    struct FillPositionsJob : IJobParallelFor {
        [ReadOnly] public Vector3 startPos;
        [ReadOnly] public float offset;
        [ReadOnly] public int2 lenght;
        public NativeArray<Matrix4x4> positions;

        public void Execute(int index) {
            int x = index / lenght.y;
            int y = index - (x * lenght.y);
            positions[index] = Matrix4x4.Translate(startPos + new Vector3(x * offset, 0, y * offset));
        }
    }

}
