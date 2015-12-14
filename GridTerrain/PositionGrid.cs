using UnityEngine;
using System.Collections;

public class PositionGrid : MonoBehaviour
{
    public int x;
    public int y;
    public float cellSize = 1;
    public Material cellMaterialValid;
    public Material cellMaterialInvalid;

    void Start()
    {
        //Init(x, y);
    }
    public void Init(int X, int Y)
    {
        x = X;
        y = Y;
        CreateChild();
        UpdateCells();
    }
    void CreateChild()
    {
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>().mesh = CreateMesh();
    }

    void UpdateCells()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
   
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        meshRenderer.material = IsCellValid(x, y) ? cellMaterialValid : cellMaterialInvalid;
        meshRenderer.useLightProbes = false;
        meshRenderer.receiveShadows = false;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        UpdateMesh(meshFilter.sharedMesh, x, y);
    }
    /// <summary>
    /// 通过检测Layer判断是否属于Building，如果是，则返回false，否则返回true
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    bool IsCellValid(int x, int z)
    {
        RaycastHit hitInfo;
        Vector3 origin = new Vector3(x * cellSize + cellSize / 2, 200, z * cellSize + cellSize / 2);
        Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Buildings"));

        return hitInfo.collider == null;
    }
    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.name = "Grid Cell:" + x + "," + y;
        mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };

        return mesh;
    }

    void UpdateMesh(Mesh mesh, int x, int z)
    {
        mesh.vertices = new Vector3[] {
            new Vector3(0,transform.parent.GetComponent<GetTerrainHeight>().Heights[x, z] ,0),
            new Vector3(0,transform.parent.GetComponent<GetTerrainHeight>().Heights[x, z+1] ,cellSize),
            new Vector3(cellSize,transform.parent.GetComponent<GetTerrainHeight>().Heights[x+1, z] ,0),
            new Vector3(cellSize,transform.parent.GetComponent<GetTerrainHeight>().Heights[x+1, z+1] ,cellSize),
           // MeshVertex(x, z),
           // MeshVertex(x, z + 1),
           // MeshVertex(x + 1, z),
           // MeshVertex(x + 1, z + 1),
        };
    }

    /*Vector3 MeshVertex(int x, int z)
    {
        return new Vector3(x * cellSize, transform.parent.GetComponent<GetTerrainHeight>().Heights[x, z] + yOffset, z * cellSize);
    }*/
}
