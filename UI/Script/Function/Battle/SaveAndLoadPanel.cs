using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveAndLoadPanel : IPanel
    {

        //public const int STATE_MAINMENU = 0;
        //public const int STATE_PREPARE = 1;
       /* public int LastState = 0;
        private const string saveConfirm_text = "是否保存当前进度到这个存档位置？";
        private const string overrideConfirm_text = "是否覆盖以前的存档以继续保存？";
        private const string loadConfirm_text = "是否读取这个存档以继续游戏？";
        public bool bContinueFromSave = false;//是否在保存一个章节后开始这一章，true则说明是在刚打开游戏选择新游戏或者游戏某章节结束记录，false则表示是在游戏准备画面时进行存档
        public bool bSave = false;//是存储还是读取
        private SaveHeader[] sh;//存储文件的头部
        public UnityEngine.UI.Button[] buttons;

        public GameObject blackFadeMask;
        private int currentSelectIndex = -1;
        private bool bSaveFileExist = false;
        void Awake()
        {
            WidgetYesNo.Instance.InitCallBack(OnYesClick);
            WidgetYesNo.Instance.Hide();
            Hide();
        }

        public void init(bool isSave, bool isContinueFromSave)//是存储则传入true，是读取传入false
        {
            gameObject.SetActive(true);
            bSave = isSave;
            bContinueFromSave = isContinueFromSave;
            sh = GameRecord._data.getRecordHeader();
            for (int i = 0; i < buttons.Length; i++)
            {
                //b.GetComponentInChildren<Image>().sprite = null;
                if (sh[i].chapter >= 0)
                {
                    bSaveFileExist = true;
                    string s = GameRecord.getRecordInfo(sh[i].chapter, sh[i].time);
                    buttons[i].GetComponentInChildren<Text>().text = s;
                }
                else
                {
                    string s = "无记录";
                    buttons[i].GetComponentInChildren<Text>().text = s;
                }
            }
        }

        public bool isHaveSave()
        {
            return bSaveFileExist;
        }

        public void ShowConfirmWindow()
        {
            if (bSave)//保存的时候
            {
                if (sh[currentSelectIndex].chapter >= 0)
                    WidgetYesNo.Instance.Show(overrideConfirm_text);
                else

                    WidgetYesNo.Instance.Show(saveConfirm_text);
            }
            else//读取的时候
            {
                WidgetYesNo.Instance.Show(loadConfirm_text);
            }
        }
        public void OnElementClick(int index)//按钮点击事件
        {
            if (WidgetYesNo.Instance.gameObject.activeSelf)//确认面板已经存在则，不接受事件
                return;
            currentSelectIndex = index;
            ShowConfirmWindow();
        }
        public void UpdateElement(string sSaveInfo)
        {
            buttons[currentSelectIndex].GetComponentInChildren<Text>().text = sSaveInfo;
        }

        public void OnYesClick()//是否确认保存或读取的窗口
        {
            if (bSave)//保存的时候
            {
                string s = GameRecord._data.SaveRecord(currentSelectIndex);//保存并得到目标文本
                UpdateElement(s);
                if (bContinueFromSave)//如果是开始新游戏或者某个章节结束时记录的，
                {//那么载入该章节
                    this.gameObject.SetActive(false);
                    SManage.Transition(new SMInput(BATTLE_STATE.Loading));
                    //SLGLevel.SLG.chapterInit(SLGLevel.ChapterIndex);这一句放到OnLoadingScene的OnDisable中
                }
                else//如果在准备画面或者是在临时战场存档
                {
                    //medifyneed画面淡出2秒后返回上一个UI窗口
                }
            }
            else//读取的时候
            {
                GameRecord._data.LoadRecord(currentSelectIndex);
                SManage.Transition(new SMInput(BATTLE_STATE.Loading));
            }
            WidgetYesNo.Instance.Hide();
            Hide();
            //然后更新当前档位 的内容显示
        }

        public void LoadingChapter()
        {
            DontDestroyOnLoad(GameObject.Find("SLG"));
            Application.LoadLevel("Loading");
        }

        private IEnumerator CrossFadeTips()
        {
            yield return new WaitForSeconds(1f);
            GetComponent<Image>().CrossFadeAlpha(0f, 1.5f, true);
        }

        private IEnumerator ReturnFadeTips()
        {
            yield return null;
            GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
        }*/
    }
}