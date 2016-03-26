﻿using System.Collections.Generic;
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
        /// <summary>
        /// 仅获取第一级子组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static List<T> GetChildComponents<T>(Transform transform)where T :Component
        {
            List<T> tList = new List<T>();
            foreach (Transform t in transform)
            {
                T ui = t.GetComponent<T>();
                if (ui != null)
                    tList.Add(ui);
            }
            return tList;
        }
        /// <summary>
        ///  仅获取第一级子组件的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static T GetChildComponent<T>(Transform transform) where T : Component
        {
            foreach (Transform t in transform)
            {
                T ui = t.GetComponent<T>();
                if (ui != null)
                    return ui;
            }
            return null;
        }
    }
}
