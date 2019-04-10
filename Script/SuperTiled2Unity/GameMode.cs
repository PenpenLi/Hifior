using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode :MonoSingleton<GameMode>
{
    public Camera mainCam;
    public PathShower pathShower;
    public UnitShower unitShower;
    public SLGChapter chapter;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
