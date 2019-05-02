using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
namespace RPG.UI
{
    /// <summary>
    /// 如果该处有存档提示是否需要进行覆盖操作
    /// </summary>
    public class UI_RecordChapterPanel : IPanel
    {
        public UI_WidgetYesNo ConfirmPanel;
        public WaitingPanel WaitSaveFinishPanel;
        [Header("按钮选定")]
        public Button[] Buttons;
        [Tooltip("该位置是否有存档")]
        public bool[] AlreadyHadSave;
        private ChapterRecordCollection[] ChapterRecordDatas;
        [Tooltip("是否是保存模式")]
        public bool bSaveMode;
        public bool bNewGameSave;
        private int m_saveIndex;
        private bool bToNextChapter;
        protected override void Awake()
        {
            base.Awake();
            bNewGameSave = false;
        }
        /// <summary>
        /// 按钮按下的时候
        /// </summary>
        /// <param name="slot"></param>
        public void OnButtonClick(int slot)
        {
            ConfirmPanel = gameMode.UIManager.WidgetYesNo;
            if (bSaveMode)
            {
                m_saveIndex = slot;
                if (AlreadyHadSave[slot])
                {
                    ConfirmPanel.InitCallBack(() => SaveTo(slot), ConfirmPanel.Hide);
                    ConfirmPanel.Show("是否需要覆盖该存档?");
                }
                else
                {
                    ConfirmPanel.InitCallBack(() => SaveTo(slot), ConfirmPanel.Hide);
                    ConfirmPanel.Show("确认在这里存档吗?");
                }
            }
            else
            {
                if (AlreadyHadSave[slot])
                {
                    ConfirmPanel.InitCallBack(() => LoadFrom(slot), ConfirmPanel.Hide);
                    ConfirmPanel.Show("是否读取该存档?");
                }
                else
                {
                    ConfirmPanel.InitCallBack(() => { bNewGameSave = true; SaveTo(slot); }, ConfirmPanel.Hide);
                    ConfirmPanel.Show("无存档，是否开始新游戏?");
                }
            }
        }
        public void SetSaveMode()
        {
            bSaveMode = true;
        }
        public void SetLoadMode()
        {
            bSaveMode = false;
        }
        public void ToggleMode()
        {
            bSaveMode = !bSaveMode;
            Debug.Log(bSaveMode ? "保存模式" : "载入模式");
        }
        public void SaveTo(int slot)
        {
            if (bNewGameSave)
                gameMode.NewGame(slot);
            else
                gameMode.SaveGame(slot);
            ConfirmShowSaveFinish(slot);
        }
        public void LoadFrom(int slot)
        {
            gameMode.LoadGame(slot);
            ConfirmShowLoadFinish();
        }

        private void ConfirmShowLoadFinish()
        {
            ConfirmPanel.InitCallBack(OnFinishYesClick, true);
            ConfirmPanel.Show("载入完毕");
        }

        private void ConfirmShowSaveFinish(int slot)
        {
            ConfirmPanel.InitCallBack(OnFinishYesClick, true);
            ConfirmPanel.Show("存储完毕");

            var nextChapterID = gameMode.ChapterManager.ChapterId;
            Buttons[slot].GetComponentInChildren<Text>().text = ResourceManager.GetChapterName(nextChapterID) + "\n" + Utils.TextUtil.GetStandardDataTime();
            AlreadyHadSave[slot] = true;
        }
        private void OnFinishYesClick()
        {
            ConfirmPanel.Hide();
            Hide(true, true);
                gameMode.PlayStartSequence();
        }
        public void Show_NewGame()
        {
            SetSaveMode();
            bNewGameSave = true;
            InitData();
            base.Show();
        }
        public void Show_Continue()
        {
            SetLoadMode();
            InitData();
            base.Show();
        }
        /// <summary>
        /// 游戏过程中需要进行的存档
        /// </summary>
        public void Show_Save(bool nextChapter)
        {
            bToNextChapter = nextChapter;
            if (nextChapter)
                gameMode.LoadNextChapter();
            SetSaveMode();
            InitData();
            base.Show();
        }
        private void InitData()
        {
            AlreadyHadSave = new bool[Buttons.Length];
            //设置按钮显示的文字
            ChapterRecordDatas = new ChapterRecordCollection[Buttons.Length];
            for (int i = 0; i < Buttons.Length; i++)
            {
                ChapterRecordDatas[i] = new ChapterRecordCollection();
                ChapterRecordDatas[i].SetIndex(i);
                if (ChapterRecordDatas[i].Exists())
                {
                    ChapterRecordDatas[i] = gameMode.ChapterManager.Record.LoadChapterFromDisk(i);
                    Buttons[i].GetComponentInChildren<Text>().text = ResourceManager.GetChapterName(ChapterRecordDatas[i].CurrentTeam.Chapter) + "\n" + ChapterRecordDatas[i].Now;
                    AlreadyHadSave[i] = true;
                }
                else
                {
                    Buttons[i].GetComponentInChildren<Text>().text = "无存档";
                    AlreadyHadSave[i] = false;
                }
            }
        }
        private void Update()
        {
            if (ConfirmPanel.isActiveAndEnabled == false && gameMode.InputManager.GetNoInput())
            {
                if (ConfirmPanel.isActiveAndEnabled) return;
                if (bToNextChapter)
                {
                    ConfirmPanel.InitCallBack(OnFinishYesClick, ConfirmPanel.Hide);
                    ConfirmPanel.Show("是否不存档进入下一章?");
                }
                else
                {
                    Hide(true, true);
                }
            }
        }
    }
}
