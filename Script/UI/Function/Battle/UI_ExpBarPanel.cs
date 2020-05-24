using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
namespace RPG.UI
{
    public class UI_ExpBarPanel : IPanel
    {
        public Image iExpBar;
        public Text tLV;
        public float BarWidth = 196.0f;
        private RectTransform rt;
        private int curExp;
        private int curLV;
        private LevelUPInfo info;

        protected override void Awake()
        {
            base.Awake();

            rt = iExpBar.GetComponent<RectTransform>();
            Hide();
        }
        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    var v = new LevelUPInfo();
        //    v.startLevel = 9;
        //    v.endLevel = 10;
        //    v.startExp = 90;
        //    v.endExp = 50;
        //    v.abilityData = new List<LevelUPInfo.AbilityData>();
        //    LevelUPInfo.AbilityData ad = new LevelUPInfo.AbilityData(new int[] {1000,1000,102,120,102,102,120,903,120,432 },new int[] { 10,1,0,1,1,0,1,1,1});
        //    v.abilityData.Add(ad);
        //    Show(v);
        //}
        public void Show(LevelUPInfo _info)
        {
            base.Show();
            info = _info;
            tLV.text = info.startExp.ToString();
            StartCoroutine(BarFadeAdd());
        }

        IEnumerator BarFadeAdd()
        {
            curExp = info.startExp;
            curLV = info.startLevel;
            rt.sizeDelta = new Vector2(((float)curExp / 100.0f) * BarWidth, rt.sizeDelta.y);
            yield return new WaitForSeconds(1.0f);
            while (curLV <= info.endLevel)
            {
                int boost = 100 - curExp;
                if (curLV == info.endLevel)//不到100
                {
                    boost = info.endExp - curExp;
                }
                for (int i = 0; i < boost; i++)
                {
                    curExp++;
                    tLV.text = curExp.ToString();
                    rt.sizeDelta = new Vector2(((float)curExp / 100.0f) * BarWidth, rt.sizeDelta.y);
                    yield return null;
                }
                yield return new WaitForSeconds(0.5f);
                if (curLV < info.endLevel)//升级界面弹出来
                {
                    FakeHide();
                    curExp = 0;
                    curLV++;
                    int dataIndex = info.endLevel - curLV;
                    gameMode.UIManager.LevelUp.Show("Mikaya", info.abilityData[dataIndex].original, info.abilityData[dataIndex].add);
                    yield return new WaitUntil(() => gameMode.UIManager.LevelUp.isActiveAndEnabled == false);
                    yield return new WaitForSeconds(0.5f);
                    FakeShow();
                    yield return new WaitForSeconds(0.5f);
                }
                else { break; }
            }
            Hide();
        }

    }
}