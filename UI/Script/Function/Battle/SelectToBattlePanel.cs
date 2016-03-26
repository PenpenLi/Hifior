using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPG.UI
{
    public class SelectToBattlePanel : IPanel
    {
        public static SelectToBattlePanel selectToBattle;
        public Text Text_SelectInfo;
        public Text Text_Number;//显示人物出场数和最大出场数
        public GameObject ContentContainer;//包含子游戏角色对象的容器
        public GameObject Char_BattleInfo_Element;//选择去战场的人物信息
        public List<RPGCharacter> _gameCharList;
        private List<int> _forceCome = new List<int>();//必须出场的人物
        private List<int> _dontCome = new List<int>();//此章节强制不可出动的人物
        public int _limit;
        public int currentSelectCount = 0;
        private List<int> _toBattleIndexList = new List<int>();
        protected override void Awake()
        {
            base.Awake();

            selectToBattle = this;
            gameObject.SetActive(false);
        }
        public void init(List<RPGCharacter> gameCharList, int limit, List<int> forceCome, List<int> forceDont)
        {
            ///////////////清理工作
            foreach (Transform child in ContentContainer.transform)
            {
                Destroy(child.gameObject);
            }
            _gameCharList.Clear();
            _forceCome.Clear();//必须出场的人物
            _dontCome.Clear();//此章节强制不可出动的人物
            _limit = 0;
            currentSelectCount = 0;
            _toBattleIndexList.Clear();
            //////////////

            if (this.gameObject.activeSelf)
                return;
            //
            _limit = limit;
            _forceCome = forceCome;
            _dontCome = forceDont;
            _gameCharList = gameCharList;
            //
            resortCharList();//重新整理排序

            //将角色表中的所有角色放到Content的子物件，设置第一个为默认选择的对象，并且在右边显示人物的状态和装备的物品
            for (int i = 0; i < gameCharList.Count; i++)
            {
                GameObject obj = Instantiate(Char_BattleInfo_Element) as GameObject;
                obj.transform.SetParent(ContentContainer.transform, false);//设为false否则会有问题
                if (i < forceCome.Count)
                {
                    obj.GetComponent<CharacterBattleInfoElement>().init(i, gameCharList[i], 1);
                }
                else
                {
                    if (i >= _gameCharList.Count - forceDont.Count)//大于等于所有数量-不能出动的人物数量 ，则显示为灰的
                        obj.GetComponent<CharacterBattleInfoElement>().init(i, gameCharList[i], -1);
                    else
                        obj.GetComponent<CharacterBattleInfoElement>().init(i, gameCharList[i], 0);
                }
            }
            refreshSelectCount();
            this.gameObject.SetActive(true);
        }
        public void resortCharList()
        {
            currentSelectCount = 0;
            List<RPGCharacter> forceComeChar = new List<RPGCharacter>();
            List<RPGCharacter> forceDontComeChar = new List<RPGCharacter>();
            for (int i = 0; i < _gameCharList.Count; i++)
            {
                if (_forceCome.Contains(_gameCharList[i].GetCharacterID()))
                {
                    forceComeChar.Add(_gameCharList[i]);
                    continue;
                }

                if (_dontCome.Contains(_gameCharList[i].GetCharacterID()))//将当前的角色与后面的调换
                {
                    forceDontComeChar.Add(_gameCharList[i]);
                    continue;
                }
            }
            foreach (RPGCharacter ch in forceComeChar)
            {
                _gameCharList.Remove(ch);
            }
            foreach (RPGCharacter ch in forceDontComeChar)
            {
                _gameCharList.Remove(ch);
            }
            _gameCharList.InsertRange(0, forceComeChar);
            _gameCharList.AddRange(forceDontComeChar);
        }
        public void refreshSelectCount()
        {
            Text_Number.text = currentSelectCount + "/" + _limit;
            if (currentSelectCount == _limit)
                Text_SelectInfo.text = "无法继续选择";
            else
            {
                Text_SelectInfo.text = "还可以选择" + (_limit - currentSelectCount).ToString() + "人";
            }
        }

        public void getBattleCharIndexList()//Button_OK点击触发的事件
        {
            CharacterBattleInfoElement[] cviArray = ContentContainer.GetComponentsInChildren<CharacterBattleInfoElement>();
            for (int i = 0; i < cviArray.Length; i++)
            {
                if (cviArray[i].bEnable)//出场
                {
                    _toBattleIndexList.Add(cviArray[i].index);
                    Debug.Log(cviArray[i].index);
                }
            }
            gameObject.SetActive(false);
        }
    }
}
