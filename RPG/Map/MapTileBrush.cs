using UnityEngine;
using System.Collections;

public class MapTileBrush : MonoBehaviour
{
    public bool EditorMode;
    public Texture texture2D;
    // Use this for initialization
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture =texture2D;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
