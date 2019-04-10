using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShower : MonoBehaviour
{
    public Transform[] UnitTransform;
    public Transform GetUnitTransformRoot(EUnitType t) { return UnitTransform[(int)t]; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
