using UnityEngine;
using System.Collections;
using UnityEngine.Events;
namespace Utils
{
    public class GameUtil
    {
        public static GameMode gameMode { get { return GameMode.Instance; } }
        private static IEnumerator IEnumDelayTimeFunc(UnityAction Action, float Time)
        {
            yield return new WaitForSeconds(Time);
            Action();
        }
        private static IEnumerator IEnumDelayFrameFunc(UnityAction Action, int frame)
        {
            for (int i = 0; i < frame; i++)
                yield return null;
            Action();
        }
        public static void DelayFunc(UnityAction Action, float Time)
        {
            UnityEngine.Assertions.Assert.IsNotNull(gameMode, "GameMode 是Null，请确保存在于场景中且Active");
            gameMode.StartCoroutine(IEnumDelayTimeFunc(Action, Time));
        }
        public static void DelayFunc(UnityAction Action, int frame)
        {
            UnityEngine.Assertions.Assert.IsNotNull(gameMode, "GameMode 是Null，请确保存在于场景中且Active");
            gameMode.StartCoroutine(IEnumDelayFrameFunc(Action, frame));
        }
        public static void DelayFuncEndOfFrame(UnityAction Action)
        {
            UnityEngine.Assertions.Assert.IsNotNull(gameMode, "GameMode 是Null，请确保存在于场景中且Active");
            gameMode.StartCoroutine(IEnumDelayFrameFunc(Action, 1));
        }
        public static void DelayFunc(MonoBehaviour Mono,UnityAction Action, float Time)
        {
            Mono.StartCoroutine(IEnumDelayTimeFunc(Action, Time));
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

        public static void ScreenShot(string name)
        {
            ScreenCapture.CaptureScreenshot(name + ".png");
        }
    }
}