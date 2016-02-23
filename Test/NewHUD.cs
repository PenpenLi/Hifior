using UnityEngine;
using System.Collections;

public class NewHUD : UHUD {
    public override void DrawHUD()
    {
        DrawText("new HUD", Color.black, new Rect(0, 0, 500, 500));
    }
}
