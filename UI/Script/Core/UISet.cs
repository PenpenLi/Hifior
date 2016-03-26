using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.UI;
//写入加载和接触显示时的效果，所有的UI对象均继承此类，并且此类含一个全局变量用于指示当前激活可操作状态和激活了显示状态的UI标志
public class UISet : MonoBehaviour
{
    public static UISet CurrentActiveUI;
    public static List<UISet> CurrentRenderedUI;
    /*建立一个包含所有顶级Panel的静态对象保留，全部以Panel_开头在SLGLevel里面拖动到该上面，然后通过该类的init函数初始化获取所有的脚本
     */
    public static StartMainMenuPanel Panel_MainMenu;
    public static AttributePanel Panel_Attribute;//
    public static ActionStatePanel Panel_ActionState;
    public static SkillMenu Panel_Skill;
    public static BattleMainPanel Panel_BattleMain;
    public static ActionMenu Panel_ActionMenu;
    public static AttackMenu Panel_AttackMenu;
    public static AttackStatePanel Panel_AttackState;
    public static GetItemOrMoney Panel_GetItemMoney;
    public static HintsPanel Panel_Hints;
    public static TalkWithoutbg Panel_TalkWithoutBG;
    public static TalkWithbg Panel_TalkWithBG;
    public static BattleReadyPanel Panel_BattleReady;
    public static HPPanel Panel_HP;
    public static StageInfoPanel Panel_StateInfo;
    public static ExpBarPanel Panel_ExpBar;
    //public static PreparePanel Panel_Prepare;
    public static SaveAndLoadPanel Panel_SaveAndLoad;
    public static LevelUpPanel Panel_LevelUp;
    public static ConfigPanel Panel_Config;
    public static void initUI()
    {
        /*Panel_MainMenu = SLGLevel.SLG.panel_MainMenu.GetComponent<StartMainMenuPanel>();
        Panel_Attribute = SLGLevel.SLG.panel_Attribute.GetComponent<AttributePanel>();
        Panel_ActionState = SLGLevel.SLG.panel_ActionState.GetComponent<ActionStatePanel>();
        Panel_BattleMain = SLGLevel.SLG.panel_BattleMain.GetComponent<BattleMainPanel>();
        Panel_ActionMenu = SLGLevel.SLG.panel_Action.GetComponent<ActionMenu>();
        Panel_Skill = SLGLevel.SLG.panel_Skill.GetComponent<SkillMenu>();
        Panel_AttackMenu = SLGLevel.SLG.panel_AttackMenu.GetComponent<AttackMenu>();
        Panel_AttackState = SLGLevel.SLG.panel_AttackState.GetComponent<AttackStatePanel>();

        Panel_GetItemMoney = SLGLevel.SLG.panel_GetItemOrMoney.GetComponent<GetItemOrMoney>();
        Panel_Hints = SLGLevel.SLG.panel_Hits.GetComponent<HintsPanel>();
        Panel_TalkWithoutBG = SLGLevel.SLG.panel_TalkNobg.GetComponent<TalkWithoutbg>();
        Panel_TalkWithBG = SLGLevel.SLG.panel_Talkwithbg.GetComponent<TalkWithbg>();
        Panel_BattleReady = SLGLevel.SLG.panel_BattleReady.GetComponent<BattleReadyPanel>();

        Panel_HP=SLGLevel.SLG.panel_HP.GetComponent<HPPanel>();

        Panel_StateInfo = SLGLevel.SLG.panel_StageInfo.GetComponent<StageInfoPanel>();
        Panel_ExpBar = SLGLevel.SLG.panel_ExpGain.GetComponent<ExpBarPanel>();
        Panel_LevelUp = SLGLevel.SLG.panel_LevelUp.GetComponent<LevelUpPanel>();
        Panel_SaveAndLoad = SLGLevel.SLG.panel_SaveAndLoad.GetComponent<SaveAndLoadPanel>();
        Panel_Config = SLGLevel.SLG.panel_Config.GetComponent<ConfigPanel>();
        */
        Panel_Config.init();
    }
    void Awake()
    {
        Utils.MiscUtil.GetChildComponent<AbstractUI>(transform).SortUIPosition();
    }

    /// <summary>
    /// /////////////////////////////以下是UI的基类实现部分
    /// </summary>
    /// 

}
