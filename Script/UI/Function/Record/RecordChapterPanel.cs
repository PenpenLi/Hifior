using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
namespace RPG.UI
{
    /// <summary>
    /// 如果该处有存档提示是否需要进行覆盖操作
    /// </summary>
    public class RecordChapterPanel : IPanel
    {
        public ModalPanel ConfirmPanel;
        public WaitingPanel WaitSaveFinishPanel;
        [Header("ModalPanel素材选定")]
        public Sprite ModalIcon;
        public Sprite ModalBG;
        [Header("按钮选定")]
        public Button[] Buttons;
        [Tooltip("该位置是否有存档")]
        public bool[] AlreadyHadSave;
        private ChapterRecordCollection[] ChapterRecordDatas;
        [Tooltip("是否是保存模式")]
        public bool bSaveMode;
        [Tooltip("保存完毕后开始当前记录的存档位置")]
        public bool bLoadNewChapterAfterSaved;
        private int m_saveIndex;
        /// <summary>
        /// 按钮按下的时候
        /// </summary>
        /// <param name="Index"></param>
        public void OnButtonClick(int Index)
        {
            if (bSaveMode)
            {
                m_saveIndex = Index;
                if (AlreadyHadSave[Index])
                {
                    ModalPanelDetail details = new ModalPanelDetail("是否需要覆盖该存档?", ModalIcon, ModalBG, new EventButtonDetail("确定", SaveTo), new EventButtonDetail("取消", ConfirmPanel.Hide));
                    ConfirmPanel.Show(details);
                }
                else
                {
                    ModalPanelDetail details = new ModalPanelDetail("确认在这里存档吗?", ModalIcon, ModalBG, new EventButtonDetail("确定", SaveTo), new EventButtonDetail("取消", ConfirmPanel.Hide));
                    ConfirmPanel.Show(details);
                }
            }
            else
            {
                if (AlreadyHadSave[Index])
                {
                    ModalPanelDetail details = new ModalPanelDetail("是否读取该存档?", ModalIcon, ModalBG, new EventButtonDetail("确定", () => GameRecord.LoadChapterSceneWithRecordData( ChapterRecordDatas[Index])), new EventButtonDetail("取消", ConfirmPanel.Hide));
                    ConfirmPanel.Show(details);
                }
                else
                {
                    ModalPanelDetail details = new ModalPanelDetail("无存档，是否开始新游戏?", ModalIcon, ModalBG, new EventButtonDetail("确定", () => GameRecord.LoadNewGame()), new EventButtonDetail("取消", ConfirmPanel.Hide));
                    ConfirmPanel.Show(details);
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
        public void SaveTo()
        {
            GameRecord.SaveTo(m_saveIndex);
            ConfirmPanel.Hide();
            WaitSaveFinishPanel.Show(ShowSaveFinish, 1.5f);
        }
        private void ShowSaveFinish()
        {
            ModalPanelDetail details = new ModalPanelDetail("存储完毕", ModalIcon, ModalBG, new EventButtonDetail("确定", OnFinishYesClick));
            ConfirmPanel.Show(details);
        }
        private void OnFinishYesClick()
        {
            ConfirmPanel.Hide();
            if (bLoadNewChapterAfterSaved)
            {
                GameRecord.LoadChapterSceneWithRecordData(GameRecord.LoadChapterRecordFrom(m_saveIndex));
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();

            AlreadyHadSave = new bool[Buttons.Length];
            ChapterRecordDatas = new ChapterRecordCollection[Buttons.Length];
            //设置按钮显示的文字
            for (int i = 0; i < Buttons.Length; i++)
            {
                ChapterRecordDatas[i] = GameRecord.LoadChapterRecordFrom(i);
                if (ChapterRecordDatas[i] != null)
                {
                    Buttons[i].GetComponentInChildren<Text>().text = ResourceManager.GetChapterName(ChapterRecordDatas[i].CurrentTeam.Chapter);
                    AlreadyHadSave[i] = true;
                }
                else
                {
                    Buttons[i].GetComponentInChildren<Text>().text = "无存档";
                    AlreadyHadSave[i] = false;
                }
            }
        }
    }
}
