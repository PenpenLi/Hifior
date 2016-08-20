using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPG.UI;
public class PC_CompanyLogo : UPlayerController
{
    public Image Background;
    public MovieTexture MovTexture;
    public UI_StartGameMenuPanel StartGameMenu;

    private enum Phase
    {
        播放视频,
        图片,
        菜单
    }

    private float StartTimeInPressStart;
    private Phase phase;
    public override void BeginPlay()
    {
        base.BeginPlay();
        PlayMovie();
    }
    public void Awake()
    {
        Background = GameObject.Find("ImageLogo").GetComponent<Image>();
        StartGameMenu = GameObject.Find("Panel_StartMenu").GetComponent<UI_StartGameMenuPanel>();
    }
    private void PlayMovie()
    {
        phase = Phase.播放视频;
        MovTexture.Play();
    }
    void OnGUI()
    {
        //绘制电影纹理
        if (MovTexture.isPlaying)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), MovTexture, ScaleMode.StretchToFill);
    }
    public override void SetupInputComponent()
    {
        base.SetupInputComponent();

        BindAction(InputIDentifier.INPUT_A, EInputActionType.IE_Released, OnSubmit);
        BindAction(InputIDentifier.INPUT_B, EInputActionType.IE_Released, OnCancel);
        BindAction(InputIDentifier.INPUT_START, EInputActionType.IE_Released, OnStart);
        BindAction(InputIDentifier.INPUT_L1, EInputActionType.IE_Clicked, OnVertical);
    }
    private void OnSubmit()
    {
        switch (phase)
        {
            case Phase.播放视频:
                MovTexture.Stop();
                Background.GetComponent<TGE.Widget.TGEUITweenColor>().Forward();
                phase = Phase.图片;
                break;
            case Phase.图片:
                break;
            case Phase.菜单:
                break;
        }
    }
    private void OnCancel()
    {
        if (phase == Phase.菜单)
        {
            StartGameMenu.Hide();
            phase = Phase.图片;
        }
    }
    private void OnStart()
    {
        if (phase == Phase.图片)
        {
            StartGameMenu.Show();
            phase = Phase.菜单;
        }
    }
    private void OnVertical()
    {

        Log.Write("axis>0");
    }
    public const float TimeToAutoPlayMovie = 10f;
    public override void Tick(float DeltaSeconds)
    {
        base.Tick(DeltaSeconds);
        if (phase == Phase.图片)
            StartTimeInPressStart += DeltaSeconds;
        else
            StartTimeInPressStart = 0f;
        if (StartTimeInPressStart > TimeToAutoPlayMovie)
        {
            PlayMovie();
        }
    }
}
