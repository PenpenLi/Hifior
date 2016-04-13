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
        public ModalPanel ComfirmPanel;
        public Sprite ModalIcon;
        public Sprite ModalBG;
        public Button[] Buttons;
        public bool[] AvailableSave;
        private ChapterRecordCollection[] ChapterRecordDatas;
            public bool bSaveMode;
        /// <summary>
        /// 按钮按下的时候
        /// </summary>
        /// <param name="Index"></param>
        public void OnButtonClick(int Index)
        {
            if (bSaveMode)
            {
                ModalPanelDetail details = new ModalPanelDetail("是否需要覆盖", ModalIcon, ModalBG, new EventButtonDetail("确定", () => GameRecord.SaveTo(Index)), new EventButtonDetail("取消", ComfirmPanel.Hide));
                ComfirmPanel.Show(details);
            }
            else
            {
                if (AvailableSave[Index])
                {
                ModalPanelDetail details = new ModalPanelDetail("是否需要读取", ModalIcon, ModalBG, new EventButtonDetail("确定", () => GameRecord.LoadChapterSceneWithRecordData(Index,ChapterRecordDatas[Index])), new EventButtonDetail("取消", ComfirmPanel.Hide));
                ComfirmPanel.Show(details);
                }
                else
                {
                    ModalPanelDetail details = new ModalPanelDetail("无可用存档，是否开始新游戏？", ModalIcon, ModalBG, new EventButtonDetail("确定", () => GameRecord.LoadNewGame()), new EventButtonDetail("取消", ComfirmPanel.Hide));
                    ComfirmPanel.Show(details);
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
        protected override void OnEnable()
        {
            base.OnEnable();

            AvailableSave = new bool[Buttons.Length];
            ChapterRecordDatas = new ChapterRecordCollection[Buttons.Length];
            //设置按钮显示的文字
            for (int i = 0; i < Buttons.Length; i++)
            {
                ChapterRecordDatas[i] = GameRecord.LoadChapterRecordFrom(i);
                if (ChapterRecordDatas[i]!=null)
                {
                    Buttons[i].GetComponentInChildren<Text>().text = ResourceManager.GetChapterName(ChapterRecordDatas[i].Chapter);
                    AvailableSave[i] = true;
                }
                else
                {
                    Buttons[i].GetComponentInChildren<Text>().text = "无存档";
                    AvailableSave[i] = false;
                }
            }
        }
    }
}
