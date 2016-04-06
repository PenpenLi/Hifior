using UnityEngine;
using System.Collections.Generic;
using RPG.UI;
/// <summary>
/// 控制界面上2D UI的显示
/// </summary>
public class UHUD : UActor
{
    [Tooltip("如果不重写DrawHUD()这个则默认显示这个文本")]
    public string DefaultHUDText;
    /// <summary>
    /// 属于这个HUD的PlayerController
    /// </summary>
    public UPlayerController PlayerOwner;

    /// <summary>
    /// 是否对HUD进行绘制
    /// </summary>
    public bool bShowHUD = true;

    /// <summary>
    /// 是否显示Debug信息
    /// </summary>
    public bool bShowDebugInfo;

    /// <summary>
    /// 需要调用PostRender()的Actors
    /// </summary>
    List<UActor> PostRenderedActors;

    /// <summary>
    /// 距离上一次HUD的渲染时间
    /// </summary>
    public float LastHUDRenderTime;

    /// <summary>
    /// HUD帧渲染时间
    /// </summary>
    public float RenderDelta;

    /// <summary>
    /// 一个列表用于决定哪些Debug信息需要显示
    /// </summary>
    List<string> DebugDisplay;

    /// <summary>
    /// 当前HUD下UI的Canvas
    /// </summary>
    public UIController Canvas;

    /// <summary>
    /// Debug专用Text列表
    /// </summary>
    protected Dictionary<string, DebugText> DebugTextList = new Dictionary<string, DebugText>();
    public struct DebugText
    {
        public string Content;
        public Rect Bound;
        public Color FontColor;
        public float FontSize;
        public float Duration;

        private bool m_show;
        private float m_lifeSpan;

        public DebugText(string content, float duration, Rect bound, Color fontColor, float fontSize)
        {
            Content = content;
            Bound = bound;
            FontColor = fontColor;
            FontSize = fontSize;
            Duration = duration;
            m_lifeSpan = Duration;
            m_show = false;
        }
        public float GetLifeSpan()
        {
            return m_lifeSpan;
        }
        public void Show()
        {
            m_lifeSpan = Duration;
            m_show = true;
        }
        public void Hide()
        {
            m_show = false;
        }
        public bool Visible()
        {
            return m_show;
        }
        public void Draw()
        {
            if (!m_show)
                return;
            GUI.Label(Bound, Content);
            m_lifeSpan -= Time.deltaTime;
            if (m_lifeSpan < 0f)
            {
                m_lifeSpan = 0.0f;
                m_show = false;
            }
        }
    }
    void Awake()
    {
        Canvas = UIController.Instance;
    }
    public override void BeginPlay()
    {
        base.BeginPlay();
    }
    /**
	 * Toggles displaying properties of player's current ViewTarget
	 * DebugType input values supported by base engine include "AI", "physics", "net", "camera", and "collision"
	 */
    public virtual void ShowDebug(string DebugType)
    {
        if (DebugTextList.ContainsKey(DebugType))
        {
            DebugTextList[DebugType].Show();
        }
    }
    public void AddDebugText(string DebugType, string Content, Rect Bound, Color TextColor, float Duration = 0f, bool bKeepAttachedToActor = false, float FontSize = 1.0f)
    {
        DebugTextList.Add(DebugType, new DebugText(Content, Duration, Bound, TextColor, FontSize));
    }

    /// <summary>
    /// Remove all debug strings added via AddDebugText
    /// </summary>
    public void RemoveAllDebugStrings()
    {
        DebugTextList.Clear();
    }

    /**
	 * Remove debug strings for the given actor
	 *
	 * @param	SrcActor			Actor whose string you wish to remove
	 * @param	bLeaveDurationText	when true text that has a finite duration will be removed, otherwise all will be removed for given actor
	 */
    public void RemoveDebugText(UActor SrcActor, bool bLeaveDurationText = false) { }

    /** Hook to allow blueprints to do custom HUD drawing. @see bSuppressNativeHUD to control HUD drawing in base class. */
    public void ReceiveDrawHUD(int SizeX, int SizeY) { }

    //=============================================================================
    // Kismet API for simple HUD drawing.

    /**
	 * Draws a string on the HUD.
	 * @param Text				String to draw
	 * @param TextColor			Color to draw string
	 * @param ScreenX			Screen-space X coordinate of upper left corner of the string.
	 * @param ScreenY			Screen-space Y coordinate of upper left corner of the string.
	 * @param Font				Font to draw text.  If NULL, default font is chosen.
	 * @param Scale				Scale multiplier to control size of the text.
	 * @param bScalePosition	Whether the "Scale" parameter should also scale the position of this draw call.
	 */
    public void DrawText(string Text, Color TextColor, Rect Rect, float Scale = 1.0f, bool bScalePosition = false)
    {
        GUI.Label(Rect, Text);
    }

    public void DrawTextureSimple(Texture2D Texture, Rect Rect, float Scale = 1.0f, bool bScalePosition = false)
    {
        GUI.DrawTexture(Rect, Texture);
    }

    protected UPlayerController GetOwningPlayerController() { return base.GetPlayerController<UPlayerController>(); }
    protected UPawn GetOwningPawn() { return base.GetPawn<UPawn>(); }

    /**
	 * Draws a colored line between two points
	 * @param Start - start of line
	 * @param End - End if line
	 * @param LineColor
	 */
    public void Draw3DLine(Vector3 Start, Vector3 End, Color LineColor)
    {

    }

    /**
     * Draws a colored line between two points
     * @param X1 - start of line x
     * @param Y1 - start of line y
     * @param X2 - end of line x
     * @param Y3 - end of line y
     * @param LineColor
     */
    public void Draw2DLine(int X1, int Y1, int X2, int Y2, Color LineColor) { }

    public void SetCanvas(Canvas InCanvas, Canvas InDebugCanvas)
    {

    }

    /** draw overlays for actors that were rendered this tick and have added themselves to the PostRenderedActors array	*/
    public virtual void DrawActorOverlays(Vector3 Viewpoint, Quaternion ViewRotation) { }

    /** Called in PostInitializeComponents or postprocessing chain has changed (happens because of the worldproperties can define it's own chain and this one is set late). */
    public virtual void NotifyBindPostProcessEffects() { }

    /************************************************************************************************************
     Actor Render - These functions allow for actors in the world to gain access to the hud and render
     information on it.
    ************************************************************************************************************/

    /** remove an actor from the PostRenderedActors array */
    public virtual void RemovePostRenderedActor(UActor A) { }

    /** add an actor to the PostRenderedActors array */
    public virtual void AddPostRenderedActor(UActor A) { }

    /** 
     * Entry point for basic debug rendering on the HUD.  Activated and controlled via the "showdebug" console command.  
     * Can be overridden to display custom debug per-game. 
     */
    public virtual void ShowDebugInfo(float YL, float YPos) { }

    /** The Main Draw loop for the hud.  Gets called before any messaging.  Should be subclassed */
    public virtual void DrawHUD()
    {
        GUI.Label(new Rect(0, 0, 500, 500), DefaultHUDText);
    }

    //=============================================================================
    // Messaging.
    //=============================================================================
    /**
     *	Pauses or unpauses the game due to main window's focus being lost.
     *	@param Enable - tells whether to enable or disable the pause state
     */
    public virtual void OnLostFocusPause(bool bEnable) { }

    /**
     * Iterate through list of debug text and draw it over the associated actors in world space.
     * Also handles culling null entries, and reducing the duration for timed debug text.
     */
    public void DrawDebugTextList()
    {
        foreach (KeyValuePair<string, DebugText> p in DebugTextList)
        {
            p.Value.Draw();
        }
    }

    // GetResolution
    public Vector2 GetScreenResolution() { return new Vector2(Screen.width, Screen.height); }

    public void OnGUI()
    {
        if (!bShowHUD)
            return;
        DrawHUD();
        DrawDebugTextList();
    }
}