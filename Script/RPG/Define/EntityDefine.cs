using System;
using System.Collections.Generic;

public static class AnimationDef
{
	public static readonly string Idle = "Idle";
	public static readonly string Run = "Run";
	public static readonly string Die = "Die";
	public static readonly string Attack = "Attack";
	public static readonly string Attack1 = "Attack01";
	public static readonly string Attack2 = "Attack02";
	public static readonly string Attack3 = "Attack03";
	public static readonly string Spell = "Spell";
	public static readonly string Spell1 = "Skill01";
	public static readonly string Spell2 = "Skill02";
	public static readonly string Spell3 = "Skill03";
	public static readonly string Spell4 = "Skill04";
}

public class AIState
{
	public const Int32 Idle					= 0;
	public const Int32 Move					= 1;
	public const Int32 AttackEnity			= 2;
	public const Int32 Attack				= 3;
	public const Int32 Patrol				= 4;
	public const Int32 Spell				= 5;
	public const Int32 Death				= 6;
	public const Int32 SpellTarget			= 7;
	public const Int32 Charge				= 8;
	public const Int32 GoHome				= 9;
	public const Int32 Hold					= 10;
	public const Int32 Swoon				= 11;
	public const Int32 Max					= 12;
}

public enum UnitEvent
{
	Move_Start,				// 开始移动
	Move_End,				// 结束移动
	Attack_Start,			// 开始攻击
	Attack_End,				// 结束攻击
	Attack_Critiacl,		// 攻击关键点

	Spell_Start,			// 开始施法
	Spell_End,				// 结束施法
	Spell_Critiacl,			// 施法关键点

	PhysicsDmg,				// 造成物理伤害（伤害类型）
	MagicDmg,				// 造成魔法伤害（伤害类型）
	Dmg,					// 造成伤害

	PhysicsDmged,			// 被物理伤害（伤害类型）
	MagicDmged,				// 被魔法伤害（伤害类型）
	Dmged,					// 被伤害

	PhysicsJook,			// 物理攻击闪避
	MagicJook,				// 魔法攻击闪避

	Crits,					// 暴击
	Miss,					// 失误

	HP0,					// 血量为0
	Kill,					// 杀死单位
	Death,					// 死亡
	Revival,				// 复活

	Add_Faction_Viewed,		// 被某方势力看见
	Del_Faction_Viewed,		// 某方势力看不见

	View_Emerge,			// 视野有单位出现
	View_Vanish,			// 视野有单位消失
	View_Emerge_Friend,		// 视野有友方单位出现
	View_Emerge_Enemy,		// 视野有敌方单位出现
	View_Vanish_Friend,		// 视野有友方单位消失
	View_Vanish_Enemy,		// 视野有敌方单位消失

	Item_Use,				// 使用物品
	Item_Buy,				// 购买物品

	Set_Pos,				// 瞬移

	Max,
}