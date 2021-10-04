using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Tanks.Editor;
#endif

public static class MonoBehaviourExtensions
{
    public static T FindComponent<T>(this MonoBehaviour component) where T : Component
    {
        var result = component.GetComponent<T>();
#if UNITY_EDITOR
        if (result == null) EditorExtensioins.LogError($"Component <b>{typeof(T).Name}</b> not found!");
#endif
        return result;
    }
    public static bool IsNullOrEmpty(this string str)
    {
        if (str == null || str.Length == 0) return true;

        const char zeroWidthSymbol = (char)8203;

        int index = str.IndexOf(zeroWidthSymbol);
        while (index >= 0)
        {
            str = str.Remove(index, 1);
            index = str.IndexOf(zeroWidthSymbol);
        }
        return str.Length == 0 ? true : false;
    }
}
