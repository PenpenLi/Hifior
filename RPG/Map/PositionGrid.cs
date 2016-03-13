using UnityEngine;
using System.Collections;

public class PositionGrid : MonoBehaviour
{
    public int x;
    public int y;
    public float CellSize = 1;
    public Material[] MaterialElements;

    void Start()
    {
        //Init(x, y);
    }
    public void Init(float CellSize,int X, int Y)
    {
        this.CellSize = CellSize;
        x = X;
        y = Y;
        CreateChild();
        UpdateCells();
        //UpdateMesh(x,y);
    }
    void CreateChild()
    {
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>().mesh = CreateMesh(x,y);
    }

    void UpdateCells()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        meshRenderer.materials = MaterialElements;
        meshRenderer.useLightProbes = false;
        meshRenderer.receiveShadows = false;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
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
        Vector3 origin = new Vector3(x * CellSize + CellSize / 2, 200, z * CellSize + CellSize / 2);
        Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Buildings"));

        return hitInfo.collider == null;
    }
    Mesh CreateMesh(int x,int z)
    {
        Mesh mesh = new Mesh();
        
        mesh.name = "Grid Cell:" + x + "," + y;
        mesh.vertices = new Vector3[] {
            new Vector3(CellSize,transform.parent.GetComponent<SLGMap>().Heights[x+1, z+1] ,CellSize),
            new Vector3(CellSize,transform.parent.GetComponent<SLGMap>().Heights[x+1, z] ,0),
            new Vector3(0,transform.parent.GetComponent<SLGMap>().Heights[x, z+1] ,CellSize),
            new Vector3(0,transform.parent.GetComponent<SLGMap>().Heights[x, z] ,0) };
        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };

        return mesh;
    }

    //void UpdateMesh( int x, int z)
    //{
    //    Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
    //    mesh.vertices = new Vector3[] {
    //        new Vector3(0,transform.parent.GetComponent<SLGMap>().Heights[x, z] ,0),
    //        new Vector3(0,transform.parent.GetComponent<SLGMap>().Heights[x, z+1] ,cellSize),
    //        new Vector3(cellSize,transform.parent.GetComponent<SLGMap>().Heights[x+1, z] ,0),
    //        new Vector3(cellSize,transform.parent.GetComponent<SLGMap>().Heights[x+1, z+1] ,cellSize)
    //    };
    //}

    /*Vector3 MeshVertex(int x, int z)
    {
        return new Vector3(x * cellSize, transform.parent.GetComponent<SLGMap>().Heights[x, z] + yOffset, z * cellSize);
    }*/
}
