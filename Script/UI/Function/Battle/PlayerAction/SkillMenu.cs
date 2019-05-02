using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SkillMenu : IMenu
    {
        public Transform Content;
        public GameObject prefabButton;
        public Text Description;
        public int count;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }
        public void Show(RPGCharacter ch)
        {
            /*List<int> keyList = ch.SkillGroup.getAllActionSkill();//获取任务所有主动技能
            for (int i = 0; i < keyList.Count; i++)
            {
                AddButton(keyList[i]);//添加按钮
            }
            this.gameObject.SetActive(true);*/
        }
        public override void Hide(bool InvokeDelegate = true,bool onlyOnce=false)
        {
            base.Hide(InvokeDelegate);

            Clear();
        }
        public void AddButton(int Key)
        {
            GameObject newButton = Instantiate(prefabButton) as GameObject;
            newButton.transform.SetParent(Content);
            newButton.name = count.ToString();
            newButton.GetComponent<ActionSkillElement>().init(count, Key);
            count++;
        }
        private void Clear()
        {
            count = 0;
            foreach (Transform child in Content.transform)
            {
                if (child.name.Equals("Text"))
                    continue;
                Destroy(child.gameObject);
            }
        }
        public void SetDescription(string description)
        {
            Description.text = description;
        }
    }
}