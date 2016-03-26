using UnityEngine;
using System.Collections.Generic;

namespace RPG.UI
{
    public class ActionMenu : IPanel
    {
        /// <summary>
        /// 操作是否进行了
        /// </summary>
        static bool afterMove;
        static bool afterAttack;
        static bool afterSkill;
        static bool afterUseItem;
        /*
        public static void AfterMove()
        {
            afterMove = true;
        }
        public static void AfterAttack()
        {
            afterAttack = true;
        }
        static void AfterSkill()
        {
            afterSkill = true;
        }
        static void AfteUseItem()
        {
            afterUseItem = true;
        }

        public UnityEngine.UI.Button leave;
        public UnityEngine.UI.Button opendoor;
        public UnityEngine.UI.Button treasure;
        public UnityEngine.UI.Button village;
        public UnityEngine.UI.Button seize;
        public UnityEngine.UI.Button talk;
        public UnityEngine.UI.Button attack;
        public UnityEngine.UI.Button wand;
        public UnityEngine.UI.Button skill;
        public UnityEngine.UI.Button item;
        public UnityEngine.UI.Button wait;
        public UnityEngine.UI.Button remove;
        public UnityEngine.UI.Button steal;
        public UnityEngine.UI.Button dance;
        public UnityEngine.UI.Button defence;
        public UnityEngine.UI.Button hide;
        public UnityEngine.UI.Button dishide;
        void Awake()
        {
            gameObject.SetActive(false);
            leave.onClick.AddListener(Button_Leave);
            opendoor.onClick.AddListener(Button_OpenDoor);
            treasure.onClick.AddListener(Button_Treasure);
            village.onClick.AddListener(Button_Village);
            seize.onClick.AddListener(Button_Seize);
            talk.onClick.AddListener(Button_Talk);
            attack.onClick.AddListener(Button_Attack);
            wand.onClick.AddListener(Button_Wand);
            skill.onClick.AddListener(Button_Skill);
            item.onClick.AddListener(Button_Item);
            wait.onClick.AddListener(Button_Wait);
            remove.onClick.AddListener(Button_Remove);
            steal.onClick.AddListener(Button_Steal);
            dance.onClick.AddListener(Button_Dance);
            defence.onClick.AddListener(Button_Defence);
            hide.onClick.AddListener(Button_Hide);
            dishide.onClick.AddListener(Button_DisHide);
        }
        
        public void ShowActionMenu(bool isItem, bool isSkill, bool isAttack, bool isTalk,
            bool isSeize, bool isViliage, bool isTreasure, bool isLeave, bool isOpenDoor,
            bool isWand, bool isReMove, bool isSteal, bool isDance,
              bool isDenfence, bool isHide, bool isDisHide, bool isGoodHeart)
        {
            gameObject.SetActive(true);
            item.gameObject.SetActive(isItem);
            skill.gameObject.SetActive(isSkill);
            attack.gameObject.SetActive(isAttack);
            talk.gameObject.SetActive(isTalk);
            seize.gameObject.SetActive(isSeize);
            village.gameObject.SetActive(isViliage);
            treasure.gameObject.SetActive(isTreasure);
            leave.gameObject.SetActive(isLeave);
            opendoor.gameObject.SetActive(isOpenDoor);
            wand.gameObject.SetActive(isWand);
            remove.gameObject.SetActive(isReMove);
            steal.gameObject.SetActive(isSteal);
            dance.gameObject.SetActive(isDance);
            defence.gameObject.SetActive(isDenfence);
            hide.gameObject.SetActive(isHide);
            dishide.gameObject.SetActive(isDisHide);
        }
        
        #region 主菜单按钮功能
        public void Button_Attack()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SHOWACTIONMENU, (int)ACTION_MENU.SELECTWEAPON));
            UISet.Panel_ActionState.Show(0, "攻击");
            UISet.Panel_AttackMenu.Show(SLGLevel.SLG.currentSelectGameChar);
            UISet.Panel_ActionMenu.Hide();
        }
        public void Button_Leave()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            Debug.Log("leave");
        }
        public void Button_OpenDoor()
        {
            Debug.Log("opendoor");
        }
        public void Button_Dance()
        {

        }
        public void Button_Village()//村庄宝箱压制离开Location事件全部
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            EventManage.OnLocationEvent(SLGLevel.SLG.currentSelectGameChar.TileCoords.x, SLGLevel.SLG.currentSelectGameChar.TileCoords.y, SLGLevel.SLG.currentSelectGameChar.CharID, delegate
            {
                SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SHOWACTIONMENU, (int)ACTION_MENU.WAIT));
            });
            Debug.Log("village");
        }
        public void Button_Hide()
        {

        }
        public void Button_DisHide()
        {

        }
        public void Button_Defence()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            Debug.Log("defence");//数值处理，播放防御的动画，然后待机
            SLGLevel.SLG.FinishAction();
        }
        public void Button_Treasure()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            EventManage.OnLocationEvent(SLGLevel.SLG.currentSelectGameChar.TileCoords.x, SLGLevel.SLG.currentSelectGameChar.TileCoords.y, SLGLevel.SLG.currentSelectGameChar.CharID, delegate
            {
                SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SHOWACTIONMENU, (int)ACTION_MENU.WAIT));
            });
            Debug.Log("treasure");
        }
        public void Button_Seize()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            Debug.Log("seize");
        }
        public void Button_Remove()
        {
            SLGLevel.SLG._sound.Play2DEffect(12);
            SLGLevel.SLG.findEscapeGameCharActionScope(SLGLevel.SLG.iRestFoot);
            SLGLevel.SLG.bSecondMove = true;//再移动pan
            SLGLevel.SLG.HideActionMenu();
            SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SELECTCHAR));//将状态设为选择角色了
        }
        public void Button_Talk()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            Hide();
            foreach (Point2D p in SLGLevel.SLG.companionCoordsList)
            {
                SLGLevel.SLG._Map.ShowCompanionSprite(p);
            }
            SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SELECTCOMPANION));
        }
        public void Button_Wand()
        {

        }
        public void Button_Steal()
        {

        }
        public void Button_Skill()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            SManage.Transition(new ASMInput(ACTION_STATE.ACTION_SHOWACTIONMENU, (int)ACTION_MENU.SKILL));
            UISet.Panel_Skill.Show(SLGLevel.SLG.getCurrentSelectGameChar());
            UISet.Panel_ActionState.Show(0, "技能");
            UISet.Panel_ActionMenu.Hide();
        }
        public void Button_Item()
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            Debug.Log("item");
            //setActionState(ActionState.ACTION_ITEM);
        }
        public void Button_Wait() //行动结束判断当前有没有人处于可以行动状态
        {
            SLGLevel.SLG._sound.Play2DEffect(8);
            SLGLevel.SLG.FinishAction();
        }

        #endregion

        public void PopActionMenu(ACTION_STATE actionState)//控制行动菜单出现的选项
        {
            bool isTalk = false, isAttack = false, isViliage = false, isSeize = false,
                isTreasure = false, isLeave = false, isSkill = false, isItem = false,
                isOpenDoor = false, isWand = false, isReMove = false, isSteal = false, isDance = false,
                isDenfence = false, isHide = false, isDisHide = false, isGoodHeart = false;

            int x = SLGLevel.SLG.currentSelectGameChar.TileCoords.x;
            int y = SLGLevel.SLG.currentSelectGameChar.TileCoords.y;
            List<Player> cl = SLGLevel.SLG.getSideGameChar(x, y, SLGLevel.SLG.currentSelectGameChar);//得到周围的人物
            SLGLevel.SLG._Map.HideTileInfo();
            if (!SLGLevel.SLG.panel_Action.activeSelf)
            {//在这里判断Location和Character事件，来决定某些按钮的显示
             //isShowAttackButton()是否有可以被攻击的敌人
                if (actionState == ACTION_STATE.ACTION_MOVEFINISH)//移动后执行的动作
                {
                    #region 是否出现攻击菜单
                    bool isHaveUseWeapon = true;
                    if (SLGLevel.SLG.currentSelectGameChar.ItemGroup.getAttackWeapon().Count == 0)//没有可以攻击的武器，不显示攻击选项
                        isHaveUseWeapon = false;
                    List<Point2D> attackRangeData = SLGLevel.SLG.findGameCharAttackRangeWithoutShow(SLGLevel.SLG.currentSelectGameChar);
                    bool isEnemyInRange = false;
                    foreach (Point2D p in attackRangeData)//遍历所有的敌人与可攻击的范围，只要有一个可以攻击就显示攻击按钮
                    {
                        Player c = SLGLevel.SLG._GameCharList.getEnemyByCoords(p.x, p.y);
                        if (c != null)
                            isEnemyInRange = true;
                    }
                    if (isEnemyInRange && isHaveUseWeapon)//如果敌人不在范围内,或者没有可使用的武器
                        isAttack = true;
                    #endregion
                    #region 是否出现技能
                    isSkill = true;
                    #endregion

                    #region 是否出现物品
                    isItem = true;
                    #endregion

                    #region 是否出现对话
                    // 得到周围的四个我方或NPC人 if(EventManage.isHasTalk(有对话，触发者))
                    SLGLevel.SLG.companionCoordsList.Clear();
                    foreach (Player c in cl)
                    {
                        if (EventManage.isHaveTalk(SLGLevel.SLG.currentSelectGameChar.CharID, c.CharID))
                        {
                            isTalk = true;
                            SLGLevel.SLG.companionCoordsList.Add(new Point2D(c.TileCoords.x, c.TileCoords.y));
                        }
                    }
                    #endregion

                    #region  是否出现 村庄，宝箱，压制，离开
                    LocationEvent.LocationType locaType = EventManage.getLocationEventType(x, y);
                    if (locaType == LocationEvent.LocationType.village)
                        isViliage = true;
                    if (locaType == LocationEvent.LocationType.chestItem || locaType == LocationEvent.LocationType.chestMoney)
                        isTreasure = true;
                    if (locaType == LocationEvent.LocationType.seize)
                        isSeize = true;
                    if (locaType == LocationEvent.LocationType.leave)
                        isLeave = true;
                    #endregion

                    #region 是否出现盗窃
                    if (SLGLevel.SLG.currentSelectGameChar.SkillGroup.isHaveStaticSkill(1))
                    {
                        foreach (Player c in cl)
                        {
                            if (c.attribute.Control == 1)
                            {
                                isSteal = true;
                            }
                        }
                    }
                    #endregion

                    #region 是否出现防守
                    if (SLGLevel.SLG.currentSelectGameChar.SkillGroup.isHaveStaticSkill(2))//是否有防守技能
                        isDenfence = true;
                    #endregion

                    #region 是否出现舞蹈
                    if (SLGLevel.SLG.currentSelectGameChar.SkillGroup.isHaveStaticSkill(3))//是否有防守技能 
                        foreach (Player c in cl)
                        {
                            if (c.attribute.Control == 0)
                            {
                                isDance = true;
                            }
                        }
                    #endregion

                    #region 是否出现隐蔽
                    if (SLGLevel.SLG.currentSelectGameChar.SkillGroup.isHaveStaticSkill(4))//是否有隐蔽技能
                        isHide = true;
                    #endregion
                }
                if (actionState == ACTION_STATE.ACTION_ATTACKFINISH)//攻击完毕
                {
                    #region 是否出现再移动
                    if (SLGLevel.SLG.currentSelectGameChar.SkillGroup.isHaveStaticSkill(5) && SLGLevel.SLG.iRestFoot > 0)//是否有再移动技能，并且处于行动完毕状态
                        isReMove = true;
                    if (SLGLevel.SLG.currentSelectGameChar.SkillGroup.isHaveStaticSkill(20) && SLGLevel.SLG._slgBattle.isDefencerDead())//打倒敌人后可以再攻击一次
                    {
                        isReMove = true;//再移动
                        SLGLevel.SLG.iRestFoot = SLGLevel.SLG.currentSelectGameChar.getMovement() - 2;//剩余移动步数=当前移动力-2；
                    }
                    #endregion
                }
                UISet.Panel_ActionMenu.ShowActionMenu(isItem, isSkill, isAttack, isTalk, isSeize, isViliage,
                     isTreasure, isLeave, isOpenDoor, isWand, isReMove, isSteal, isDance, isDenfence,
                     isHide, isDisHide, isGoodHeart);

            }
        }
        /*public void HideActionMenu()
        {
            if (UISet.Panel_ActionMenu.gameObject.activeSelf)
                UISet.Panel_ActionMenu.Hide();
        }*/
    }
}