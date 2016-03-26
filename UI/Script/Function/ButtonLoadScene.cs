using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace RPG.UI
{
    public class ButtonLoadScene : IButton
    {
        public int SceneIndex;
        public override void OnClick()
        {
            if(SceneIndex<0 || SceneIndex>= SceneManager.sceneCountInBuildSettings)
            {
                Utils.Log.Write("Scene only has " + SceneManager.sceneCountInBuildSettings + " scenes");
            }
            LoadingScreenManager.LoadScene(SceneIndex);
        }
    }
}