using UnityEngine;
using System.Collections;
namespace Utils
{
    public class GameUtil
    {
        public static GameMode gameMode { get { return GameMode.Instance; } }
        private static IEnumerator IEnumDelayTimeFunc(UnityEngine.Events.UnityAction Action, float Time)
        {
            yield return new WaitForSeconds(Time);
            Action();
        }
        private static IEnumerator IEnumDelayFrameFunc(UnityEngine.Events.UnityAction Action, int frame)
        {
            for (int i = 0; i < frame; i++)
                yield return null;
            Action();
        }
        public static void DelayFunc(UnityEngine.Events.UnityAction Action, float Time)
        {
            UnityEngine.Assertions.Assert.IsNotNull(gameMode, "GameMode 是Null，请确保存在于场景中且Active");
            gameMode.StartCoroutine(IEnumDelayTimeFunc(Action, Time));
        }
        public static void DelayFunc(UnityEngine.Events.UnityAction Action, int frame)
        {
            UnityEngine.Assertions.Assert.IsNotNull(gameMode, "GameMode 是Null，请确保存在于场景中且Active");
            gameMode.StartCoroutine(IEnumDelayFrameFunc(Action, frame));
        }
        public static void DelayFuncEndOfFrame(UnityEngine.Events.UnityAction Action)
        {
            UnityEngine.Assertions.Assert.IsNotNull(gameMode, "GameMode 是Null，请确保存在于场景中且Active");
            gameMode.StartCoroutine(IEnumDelayFrameFunc(Action, 1));
        }
        public static void DelayFunc(MonoBehaviour Mono, UnityEngine.Events.UnityAction Action, float Time)
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
        public static void UpdateSwitch(float interval, params System.Action[] func1)
        {
            int choose = Mathf.RoundToInt(Time.time / ConstTable.CONST_INDIATOR_FLASH_INTERVAL) % func1.Length;
            func1[choose].Invoke();
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