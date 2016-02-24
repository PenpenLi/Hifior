using System.Collections;
using UnityEngine;
namespace Utils
{
    public class MiscUtil
    {
        /// <summary>
        /// 根据名称查找子物体
        /// </summary>
        /// <param name="ParentObject"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static GameObject FindChild(GameObject ParentObject, string Name)
        {
            if (ParentObject == null)
            {
                return null;
            }
            Transform transform = ParentObject.transform.FindChild(Name);
            if (transform == null)
            {
                return null;
            }
            return transform.gameObject;
        }
        /// <summary>
        /// 获取一个组件，当该组件不存在时添加该组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetComponentNotNull<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}
