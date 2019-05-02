using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI
{
    public class UI_ChapterStartPreface : IPanel
    {
        public Text ChapterName;
        public void Show(string text,Sprite bg=null)
        {
            Background.gameObject.SetActive(true);
            if (bg != null) Background.sprite = bg;
            ChapterName.text = text;
            DelayHide(2.0f);
            base.Show();
        }
    }
}