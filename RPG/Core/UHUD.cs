using UnityEngine;
using System.Collections.Generic;
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
    public Canvas Canvas;

    /// <summary>
    /// Debug专用Canvas
    /// </summary>
    public Canvas DebugCanvas;

    /// <summary>
    /// Debug专用Text列表
    /// </summary>
    protected List<string> DebugTextList;

    /**
	 * Toggles displaying properties of player's current ViewTarget
	 * DebugType input values supported by base engine include "AI", "physics", "net", "camera", and "collision"
	 */
    public virtual void ShowDebug(string DebugType) { }
    /**
	 * Add debug text for a specific actor to be displayed via DrawDebugTextList().  If the debug text is invalid then it will
	 * attempt to remove any previous entries via RemoveDebugText().
	 * 
	 * @param DebugText				Text to draw
	 * @param SrcActor				Actor to which this relates
	 * @param Duration				Duration to display the string
	 * @param Offset 				Initial offset to render text
	 * @param DesiredOffset 		Desired offset to render text - the text will move to this location over the given duration
	 * @param TextColor 			Color of text to render
	 * @param bSkipOverwriteCheck 	skips the check to see if there is already debug text for the given actor
	 * @param bAbsoluteLocation 	use an absolute world location
	 * @param bKeepAttachedToActor 	if this is true the text will follow the actor, otherwise it will be drawn at the location when the call was made
	 * @param InFont 				font to use
	 * @param FontScale 			scale
	 * @param bDrawShadow 			Draw shadow on this string
	 */
    public void AddDebugText(string DebugText, Vector3 Offset, Vector3 DesiredOffset, Color TextColor, UActor SrcActor = null, float Duration = 0f, bool bSkipOverwriteCheck = false, bool bAbsoluteLocation = false, bool bKeepAttachedToActor = false, Font InFont = null, float FontScale = 1.0f, bool bDrawShadow = false)
    { }

    /**
	 * Remove all debug strings added via AddDebugText
	 */
    public void RemoveAllDebugStrings() { }

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

    /**
	 * Draws a 2D line on the HUD.
	 * @param StartScreenX		Screen-space X coordinate of start of the line.
	 * @param StartScreenY		Screen-space Y coordinate of start of the line.
	 * @param EndScreenX		Screen-space X coordinate of end of the line.
	 * @param EndScreenY		Screen-space Y coordinate of end of the line.
	 * @param LineColor			Color to draw line
	 */
    public void DrawLine(float StartScreenX, float StartScreenY, float EndScreenX, float EndScreenY, Color LineColor)
    {

    }

    /**
	 * Draws a colored untextured quad on the HUD.
	 * @param RectColor			Color of the rect. Can be translucent.
	 * @param ScreenX			Screen-space X coordinate of upper left corner of the quad.
	 * @param ScreenY			Screen-space Y coordinate of upper left corner of the quad.
	 * @param ScreenW			Screen-space width of the quad (in pixels).
	 * @param ScreenH			Screen-space height of the quad (in pixels).
	 */
    public void DrawRect(Color RectColor, float ScreenX, float ScreenY, float ScreenW, float ScreenH)
    {

    }

    /**
	 * Draws a textured quad on the HUD. Assumes 1:1 texel density.
	 * @param Texture			Texture to draw.
	 * @param ScreenX			Screen-space X coordinate of upper left corner of the quad.
	 * @param ScreenY			Screen-space Y coordinate of upper left corner of the quad.
	 * @param Scale				Scale multiplier to control size of the text.
	 * @param bScalePosition	Whether the "Scale" parameter should also scale the position of this draw call.
	 */
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