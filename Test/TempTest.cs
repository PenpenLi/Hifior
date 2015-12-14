using UnityEngine;
using System.Collections;

public class TempTest : MonoBehaviour {

    public WeaponDef[] weapons;
    void Start()
    {
        foreach( WeaponDef w in weapons)
        {
            Debug.Log(w.WeaponType+ w.CommonProperty.ID + "  " + w.CommonProperty.Name + "   " + w.CommonProperty.Description);
        }
    }
}
