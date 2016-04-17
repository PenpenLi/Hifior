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
        public List<CharacterInfo> _PlayersList;
        private List<int> _forceCome ;//必须出场的人物
        private List<int> _dontCome;//此章节强制不可出动的人物
        public int _limit;
        public int currentSelectCount = 0;
        private List<int> _toBattleIndexList = new List<int>();
        protected override void Awake()
        {
            base.Awake();

            selectToBattle = this;
            gameObject.SetActive(false);
        }
        public override void Show()
        {
            base.Show();
            ChapterSettingDef def = GetGameMode<GM_Battle>().GetSLGChapter().ChapterSetting;
            Init(GetGameInstance().GetAvailablePlayersInfo(), def.MaxPlayerCount, def.ForceInvolve, new List<int>());
        }
        public void Init(List<CharacterInfo> PlayerList, int PlayersLimit, List<int> ForceInvolve, List<int> ForceDontInvolve)
        {
            ///////////////清理工作
            foreach (Transform child in ContentContainer.transform)
            {
                Destroy(child.gameObject);
            }
            //_PlayersList.Clear();
            //_forceCome.Clear();//必须出场的人物
           // _dontCome.Clear();//此章节强制不可出动的人物
           // _limit = 0;
            currentSelectCount = 0;
            _toBattleIndexList.Clear();
            //
            _limit = PlayersLimit;
            _forceCome = ForceInvolve;
            _dontCome = ForceDontInvolve;
            _PlayersList = PlayerList;
            //
            ResortCharList();//重新整理排序

            //将角色表中的所有角色放到Content的子物件，设置第一个为默认选择的对象，并且在右边显示人物的状态和装备的物品
            for (int i = 0; i < PlayerList.Count; i++)
            {
                GameObject obj = Instantiate(Char_BattleInfo_Element) as GameObject;
                obj.transform.SetParent(ContentContainer.transform, false);//设为false否则会有问题
                if (i < ForceInvolve.Count)
                {
                    obj.GetComponent<CharacterBattleInfoElement>().Init(i, PlayerList[i], 1);
                }
                else
                {
                    if (i >= _PlayersList.Count - ForceDontInvolve.Count)//大于等于所有数量-不能出动的人物数量 ，则显示为灰的
                        obj.GetComponent<CharacterBattleInfoElement>().Init(i, PlayerList[i], -1);
                    else
                        obj.GetComponent<CharacterBattleInfoElement>().Init(i, PlayerList[i], 0);
                }
            }
            RefreshSelectCount();
        }
        public override void OnCancelKeyDown()
        {
            base.OnCancelKeyDown();
            base.Hide();
        }
        public void ResortCharList()
        {
            currentSelectCount = 0;
            List<CharacterInfo> forceComeChar = new List<CharacterInfo>();
            List<CharacterInfo> forceDontComeChar = new List<CharacterInfo>();
            for (int i = 0; i < _PlayersList.Count; i++)
            {
                if (_forceCome.Contains(_PlayersList[i].ID))
                {
                    forceComeChar.Add(_PlayersList[i]);
                    continue;
                }

                if (_dontCome.Contains(_PlayersList[i].ID))//将当前的角色与后面的调换
                {
                    forceDontComeChar.Add(_PlayersList[i]);
                    continue;
                }
            }
            foreach (CharacterInfo ch in forceComeChar)
            {
                _PlayersList.Remove(ch);
            }
            foreach (CharacterInfo ch in forceDontComeChar)
            {
                _PlayersList.Remove(ch);
            }
            _PlayersList.InsertRange(0, forceComeChar);
            _PlayersList.AddRange(forceDontComeChar);
        }
        public void RefreshSelectCount()
        {
            Text_Number.text = currentSelectCount + "/" + _limit;
            if (currentSelectCount == _limit)
                Text_SelectInfo.text = "无法继续选择";
            else
            {
                Text_SelectInfo.text = "还可以选择" + (_limit - currentSelectCount).ToString() + "人";
            }
        }

        public void GetBattleCharIndexList()//Button_OK点击触发的事件
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
