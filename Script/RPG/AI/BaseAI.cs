using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RPG.AI
{
    public enum EAttackPriority
    {
        Random,
        Lethal,
        LeastHP,
        MostDamage,
    }
    public enum ETargetCamp
    {
        Player,
        Enemy,
        Ally,
        PlayerAndAlly,
        All
    }
    public abstract class BaseAI
    {
        protected GameMode gameMode { get { return GameMode.Instance; } }
        public BaseAI(RPGCharacter ch)
        {
            unit = ch;
            inRangeCharacters = new List<RPGCharacter>();
        }
        public abstract string Name();
        /// <summary>
        /// 将行动组装成Sequence进行播放
        /// </summary>
        public abstract void Action();
        public virtual bool IsCureSelf() { return true; }
        public abstract void GetInActionRange();
        protected RPGCharacter unit;
        protected CharacterLogic logic { get { return unit.Logic; } }
        protected EnumCharacterCamp SelfCamp { get { return unit.GetCamp(); } }
        protected List<RPGCharacter> inRangeCharacters;
        public static List<RPGCharacter> GetIf(List<RPGCharacter> ch, System.Predicate<RPGCharacter> predicate)
        {
            List<RPGCharacter> r = new List<RPGCharacter>();
            foreach (var v in ch)
            {
                if (predicate(v))
                    r.Add(v);
            }
            return r;
        }
        /// <summary>
        /// 获取HP最少的单位
        /// </summary>
        /// <returns></returns>
        public RPGCharacter GetLeastHP()
        {
            return null;
        }
        /// <summary>
        /// 获取对自己而言最弱的单位
        /// </summary>
        /// <returns></returns>
        public RPGCharacter GetWeakest()
        {
            return null;
        }
        /// <summary>
        /// 获取重要人物会导致游戏失败的单位
        /// </summary>
        /// <returns></returns>
        public List<RPGCharacter> GetImportant()
        {
            List<RPGCharacter> r = new List<RPGCharacter>();
            return r;
        }
        /// <summary>
        /// 获取比自己弱的单位
        /// </summary>
        /// <returns></returns>
        public List<RPGCharacter> GetWeaker()
        {
            List<RPGCharacter> r = new List<RPGCharacter>();
            return r;
        }
        /// <summary>
        /// 获取比自己强的单位
        /// </summary>
        /// <returns></returns>
        public List<RPGCharacter> GetStronger()
        {
            List<RPGCharacter> r = new List<RPGCharacter>();
            return r;
        }
        public RPGCharacter GetRandomInRange()
        {
            int index = UnityEngine.Random.Range(0, inRangeCharacters.Count);
            return inRangeCharacters[index];
        }

        #region Action
        protected void CameraFollow()
        {
            gameMode.slgCamera.StartFollowTransform(unit.GetTransform(),true);
        }
        #endregion
    }
}