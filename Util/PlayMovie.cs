using UnityEngine;
namespace Utils
{
    public class PlayMovie : MonoBehaviour
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        public MovieTexture movTexture;

        void Start()
        {
            //设置电影纹理播放模式为循环
            movTexture.loop = false;
            movTexture.Play();
        }

        void OnGUI()
        {
            //绘制电影纹理
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movTexture, ScaleMode.StretchToFill);
        }
        void Update()
        {
            if (!movTexture.isPlaying)
            {
                Log.Write("movie play finished");
            }
        }
    }
#endif
}
