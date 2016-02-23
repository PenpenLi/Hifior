using UnityEngine;
using System.Collections;

public class NewBehaviourScript : UActor
{
    public pawn1 P;
    public UPawn DefaultPawn;
    public override void BeginPlay()
    {
        P = Instantiate<pawn1>(P);
        GetPlayerController<UPlayerController>().Possess(P);
        Invoke("ff", 10.0f);
    }
    void ff()
    {
        Debug.Log("跳转");
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        GetPlayerController<UPlayerController>().PossessOld();
    }
}
