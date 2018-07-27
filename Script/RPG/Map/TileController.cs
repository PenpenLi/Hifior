using UnityEngine;
using UnityEngine.Tilemaps;
public class TileController : MonoBehaviour
{
    Tilemap tileEntity;
   public Vector3Int ori;
    public Vector3Int des;

    void Awake()
    {
        tileEntity = GetComponent<Tilemap>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            var t = tileEntity.GetTile(ori + tileEntity.origin);
            tileEntity.SetTile(des + tileEntity.origin, t);
        }
    }
}
