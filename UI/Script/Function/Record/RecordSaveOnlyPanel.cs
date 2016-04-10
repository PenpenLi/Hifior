using UnityEngine;
using System.Collections;
using System;

namespace RPG.UI
{
    /// <summary>
    /// 如果该处有存档提示是否需要进行覆盖操作
    /// </summary>
    public class RecordSaveOnlyPanel : IPanel
    {
        public ModalPanel Modal;
        public Sprite ModalIcon;
        public Sprite ModalBG;

        /// <summary>
        /// 按钮按下的时候
        /// </summary>
        /// <param name="Index"></param>
        public void OnButtonClick(int Index)
        {
            ModalPanelDetail details = new ModalPanelDetail("是否需要覆盖", ModalIcon, ModalBG, new EventButtonDetail("确定", ConfirmCover), new EventButtonDetail("取消", CancelCover));
            Modal.Show(details);
            GameRecord.SaveTo(Index);
        }
        /// <summary>
        /// 确认覆盖存档
        /// </summary>
        private void ConfirmCover()
        {

        }
        /// <summary>
        /// 取消覆盖存档
        /// </summary>
        private void CancelCover()
        {

        }
    }
}
