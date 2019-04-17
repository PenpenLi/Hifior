using UnityEngine;
public enum EMoveSpeed
{
    Normal,
    Fast,
    Slow
}
public class ConstTable
{
    #region Resource文件夹 Prefab 路径
    public const string PREFAB_CANVAS = "Prefab/Canvas";
    /*public const string PREFAB_UI_ACTION_STATE = "Prefab/UIElement/Action_State";
    public const string PREFAB_UI_START_MENU = "Prefab/UIElement/Panel_StartMenu";*/
    public const string PREFAB_UI_ACTIONMENU_BUTTON = "Prefab/UIElement/ActionMenuButton";
    #endregion

    //包含 游戏中的设置和 相应的固定的信息
    public const int CONST_SAVE_MAXCOUNT = 6;
    public const int TEAM_COUNT = 3;
    public const int LEADER_0 = 0;
    public const int LEADER_1 = 1;
    public const int LEADER_2 = 2;

    public const int CONST_AI_CURE_DISTANCE = 0;
    public const int CONST_AI_DAMAGE_HP_WEIGHT = 0;
    public const int CONST_AI_NOT_HIT_BACK_ADDITION = 0;
    public const int CONST_AI_TERRAIN_AVOID = 0;
    public const int CONST_AI_TERRAIN_DEFENCE = 0;
    public const int CONST_ATTACK_ORDER_DECREASE_VALUE = 0;
    public const int CONST_CHAR_LV_LIMIT = 0;
    public const int CONST_CHAR_PROPERTY_LIMIT = 0;
    public const int CONST_CHAR_TRANSFER_LV = 0;
    public const int CONST_CRT_MULTIPLER = 0;
    public const int CONST_EXP_ENEMY_INJURE_MIN = 0;
    public const int CONST_EXP_ENEMY_INJURE_MULTIPLE = 0;
    public const int CONST_EXP_ENEMY_KILL_MIN = 0;
    public const int CONST_EXP_ENEMY_KILL_MULTIPLE = 0;
    public const int CONST_EXP_ENEMY_NONE_MIN = 0;
    public const int CONST_EXP_ENEMY_NONE_MULTIPLE = 0;
    public const int CONST_HEAL_MOD = 0;
    public const int CONST_HEAL_XP = 0;
    public const int CONST_REVIVE_PRICE_BASE = 0;
    public const int CONST_REVIVE_PRICE_LV_ADDITION = 0;
    public const int CONST_M_COUNT = 6;
    public const int CONST_PASSIVEITEM_COUNT = 2;

    //显示
    public const float CONST_TURN_IMAGE_ANIMATION_DURATION = 0.5f;
    public const float CONST_TURN_IMAGE_ANIMATION_WAITTIME = 1.0f;
    public const float CONST_NORMAL_UNIT_MOVE_SPEED = 4.0f;
    public const float CONST_FAST_UNIT_MOVE_SPEED = 6.0f;
    public const float CONST_SLOW_UNIT_MOVE_SPEED = 2.0f;
    public static Color CAMP_COLOR(EnumCharacterCamp camp)
    {
        switch (camp)
        {
            case EnumCharacterCamp.Player:
                return Color.blue;
            case EnumCharacterCamp.Enemy:
                return Color.red;
            case EnumCharacterCamp.Ally:
                return Color.cyan;
            case EnumCharacterCamp.NPC:
                return Color.green;
        }
        return Color.white;
    }
    public static float UNIT_MOVE_SPEED() { return CONST_NORMAL_UNIT_MOVE_SPEED; }
    public static float UNIT_MOVE_SPEED(EMoveSpeed speed)
    {
        switch (speed)
        {
            case EMoveSpeed.Fast: return CONST_FAST_UNIT_MOVE_SPEED;
            case EMoveSpeed.Slow: return CONST_SLOW_UNIT_MOVE_SPEED;
        }
        return CONST_NORMAL_UNIT_MOVE_SPEED;
    }
}