using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIndicator : MonoBehaviour
{
    public GameObject tileIndicator;
    public bool flashIndicator;
    // Start is called before the first frame update
    void Start()
    {
        tileIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (flashIndicator)
        {
            Utils.GameUtil.UpdateSwitch(ConstTable.CONST_INDIATOR_FLASH_INTERVAL,
                () => { tileIndicator.transform.localScale = Vector3.one * 0.75f; },
                 () => { tileIndicator.transform.localScale = Vector3.one; }
            );
        }
    }
    public void ShowTileIndicator(Vector2Int tilePos, bool flash = true)
    {
        PositionMath.SetUnitLocalPosition(tileIndicator.transform, tilePos);
        tileIndicator.SetActive(true);
        flashIndicator = flash;
    }
    public void HideTileIndicator()
    {
        tileIndicator.SetActive(false);
    }
}
