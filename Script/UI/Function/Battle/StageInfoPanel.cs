using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace RPG.UI
{
    public class StageInfoPanel : IPanel
    {
        public Text tStaticClear;
        public GameObject ClearInfoPanel;
        public GameObject ClearInfoElement;
        public Text tTurn;
        // Use this for initialization

        public void Init(List<string> clearInfo, int turn)
        {
            UnityEngine.Assertions.Assert.IsFalse(clearInfo == null || clearInfo.Count == 0, "无效的过关信息");
            int ChildCount = ClearInfoPanel.transform.childCount;
            for(int i = 0; i < ChildCount; i++)
            {
                Destroy(ClearInfoPanel.transform.GetChild(i));
            }
           
            for (int i = 1; i < clearInfo.Count; i++)
            {
                Text t = Instantiate(ClearInfoElement).GetComponent<Text>();
                t.transform.SetParent(transform, false);
                t.text = clearInfo[i];
            }

            Init(turn);
        }
        public void Init(int turn)
        {
            tTurn.text = turn.ToString() + " Turn";
        }
    }
}