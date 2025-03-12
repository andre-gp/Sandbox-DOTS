using UnityEngine;

public class RenderMeshSingleThread : MonoBehaviour
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
        totalCount = rowCount * columnCount;
    }

    private void Update() {
        Matrix4x4[] positions = new Matrix4x4[rowCount * columnCount];

        for (int x = 0; x < rowCount; x++) {
            for (int y = 0; y < columnCount; y++) {
                positions[(x * columnCount) + y] = Matrix4x4.Translate(transform.position + new Vector3(x * offset, 0, y * offset));
            }
        }
        Graphics.RenderMeshInstanced(rp, mesh, 0, positions);
        //Graphics.RenderMeshInstanced(rp, mesh, 0, ));

    }
}
