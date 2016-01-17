using UnityEngine;
using System.Collections;
namespace Utils
{
    public class GameUtil
    {
        private static IEnumerator IEnumDelayFunc(UnityEngine.Events.UnityAction Action, float Time)
        {
            yield return new WaitForSeconds(Time);
            Action();
        }
        public static void DelayFunc(MonoBehaviour MonoInstance, UnityEngine.Events.UnityAction Action, float Time)
        {
            MonoInstance.StartCoroutine(IEnumDelayFunc(Action, Time));
        }
        public static GameObject Instantiate(GameObject ob, Transform parent)
        {
            if (ob == null)
            {
                return null;
            }
            Vector3 vector = ob.transform.localPosition;
            Quaternion quaternion = ob.transform.localRotation;
            Vector3 vector2 = ob.transform.localScale;
            GameObject obj2 = Object.Instantiate(ob) as GameObject;
            obj2.transform.parent = parent;
            obj2.transform.localPosition = vector;
            obj2.transform.localRotation = quaternion;
            obj2.transform.localScale = vector2;
            return obj2;
        }

        #region 游戏控制
        public static void Pause()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
        public static void Quit()
        {
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        #endregion

        #region 截屏功能
        public static void ScreenShot(string name)
        {
            Application.CaptureScreenshot(name + ".png");
        }
        /// <param name="rect">Rect.截图的区域，左下角为o点</param>
        /// 截取全屏：CaptureScreenshot2( new Rect( Screen.width*0f, Screen.height*0f, Screen.width*1f, Screen.height*1f));
        /// 截取四分之一：CaptureScreenshot2( new Rect( Screen.width*0.25f, Screen.height*0.25f, Screen.width*0.5f, Screen.height*0.5f));
        public static Texture2D ScreenShot(Rect rect)
        {
            // 先创建一个的空纹理，大小可根据实现需要来设置
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

            // 读取屏幕像素信息并存储为纹理数据，
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            // 然后将这些纹理数据，成一个png图片文件
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Application.dataPath + "/Screenshot.png";
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("截屏了一张图片: {0}", filename));

            // 最后，我返回这个Texture2d对象，这样我们直接，所这个截图图示在游戏中，当然这个根据自己的需求的。
            return screenShot;
        }

        public static Texture2D ScreenShot(Camera camera, Rect rect)
        {
            // 创建一个RenderTexture对象  
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
            camera.targetTexture = rt;
            camera.Render();
            //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
            //ps: camera2.targetTexture = rt;  
            //ps: camera2.Render();  
            //ps: -------------------------------------------------------------------  

            // 激活这个rt, 并从中中读取像素。  
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
            screenShot.Apply();

            // 重置相关参数，以使用camera继续在屏幕上显示  
            camera.targetTexture = null;
            //ps: camera2.targetTexture = null;  
            RenderTexture.active = null; // JC: added to avoid errors  
            GameObject.Destroy(rt);
            // 最后将这些纹理数据，成一个png图片文件  
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Application.dataPath + "/Screenshot.png";
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("截屏了一张照片: {0}", filename));

            return screenShot;
        }
        #endregion
    }
}