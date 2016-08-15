using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPG.UI
{
    public class TidyUpPanel : IPanel
    {
        public static TidyUpPanel tidyUp;
        public GameObject characterElementPrefab;

        public GameObject ContentContainer;//包含子游戏角色对象的容器
        public GameObject firstSelectChar;
        public bool bWaitSelectSecond;
        public GameObject secondSelectChar;
        public Text textCaption;
        public GameObject CharStateBiggerPanel;
        public GameObject ItemsPanelTop;
        public GameObject ItemsPanelBottom;

        public GameObject ItemCommand;
        private CharStateBiggerPanel csbp;
        private ItemPanel ipTop;
        private ItemPanel ipBottom;

        public int selectedCharIndex;
        public int exchangeCharIndex;
        public int selectedItemIndex;
        public int exchangeItemIndex;

        private List<RPGCharacter> _gameCharList;
        protected override void Awake()
        {
            base.Awake();

            tidyUp = this;
            csbp = CharStateBiggerPanel.GetComponent<CharStateBiggerPanel>();
            ipTop = ItemsPanelTop.GetComponent<ItemPanel>();
            ipBottom = ItemsPanelBottom.GetComponent<ItemPanel>();
            this.gameObject.SetActive(false);
            ItemsPanelTop.SetActive(false);
            ItemCommand.SetActive(false);
            bWaitSelectSecond = false;
        }

        void Update()
        {
            if (bWaitSelectSecond)//选择第二个状态，如果按下右键（取消键）则使文字颜色变回来，并且取消状态，且将上面的ItemPanel隐藏
                if (Input.GetMouseButtonUp(1))
                {
                    ItemsPanelTop.SetActive(false);
                    bWaitSelectSecond = false;
                    firstSelectChar.GetComponentInChildren<Text>().color = Color.black;
                    ShowCommandWindow();
                }
        }
        public void init(List<RPGCharacter> gameCharList)
        {
            foreach (Transform child in ContentContainer.transform)
            {
                Destroy(child.gameObject);
            }
            if (this.gameObject.activeSelf)
                return;
            _gameCharList = gameCharList;
            //将角色表中的所有角色放到Content的子物件，设置第一个为默认选择的对象，并且在右边显示人物的状态和装备的物品
            for (int i = 0; i < gameCharList.Count; i++)
            {
                GameObject obj = Instantiate(characterElementPrefab) as GameObject;
                obj.transform.SetParent(ContentContainer.transform, false);//设为false否则会有问题
                obj.GetComponent<CharacterElement>().init(i, gameCharList[i]);
            }
            refreshCharInfo();
            firstSelectChar = ContentContainer.transform.GetChild(0).gameObject;
            selectedCharIndex = 0;
            this.gameObject.SetActive(true);
        }
        public void setSelectChar(GameObject obj, int index)
        {
            firstSelectChar = obj;
            selectedCharIndex = index;
            refreshCharInfo();
        }
        public void setSelectExchangeChar(GameObject obj, int index)
        {
            secondSelectChar = obj;
            exchangeCharIndex = index;
            ItemsPanelTop.SetActive(true);
            ipTop.Init(_gameCharList[index]);
        }
        public void ShowCommandWindow()
        {
            ItemCommand.SetActive(true);
        }
        public void HideCommandWindow()
        {
            ItemCommand.SetActive(false);
        }
        public void refreshCharInfo()
        {
            csbp.Init(_gameCharList[selectedCharIndex]);
            ipBottom.Init(_gameCharList[selectedCharIndex]);
        }

        public void exchangeItem(int char_Index0, int item_Index0, int char_Index1, int item_Index1)
        {
            RPGCharacter ch0 = _gameCharList[char_Index0];
            RPGCharacter ch1 = _gameCharList[char_Index1];
            WeaponItem item0 = ch0.Item.GetWeaponByIndex(item_Index0);
            WeaponItem item1 = ch1.Item.GetWeaponByIndex(item_Index1);
            if (item0 == null)
                return;
            if (item1 == null)
            {
                ch0.Item.RemoveWeaponByIndex(item_Index0);
                ch1.Item.AddWeapon(item0,null);
            }
            else
            {
                ch0.Item.RemoveWeaponByIndex(item_Index0);
                ch1.Item.RemoveWeaponByIndex(item_Index1);
                ch0.Item.AddWeapon(item1, item_Index0);
                ch1.Item.AddWeapon(item0, item_Index1);
            }
            ipBottom.Init(_gameCharList[selectedCharIndex]);
            ipTop.Init(_gameCharList[exchangeCharIndex]);
        }
        #region 按钮功能实现区
        public void button_Exchange()
        {
            HideCommandWindow();
            firstSelectChar.GetComponentInChildren<Text>().color = Color.grey;
            bWaitSelectSecond = true;//等待第二个人物的选择
        }

        public void button_Warehouse()
        {

        }
        public void button_UseItem()
        {

        }
        public void button_Toss()
        {

        }
        #endregion
    }
}