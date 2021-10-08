#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Editor
{
    public static class EditorExtensions
    {
        public enum EditorMessageType : byte
        {
            None,
            Editor,
            Game
        }
        private static Dictionary<EditorMessageType, string> Prefix = new Dictionary<EditorMessageType, string>()
    {
        { EditorMessageType.None, string.Empty },
        { EditorMessageType.Editor, "<color=green>[EDITOR]</color>" },
        { EditorMessageType.Game, "<color=red>[GAME]</color>" }
    };
        public static void LogError(object message, EditorMessageType type = EditorMessageType.None)
        {
            Debug.LogError($"{Prefix[type]} {message}");
        }
        public static void Log(object message, EditorMessageType type = EditorMessageType.None)
        {
            Debug.Log($"{Prefix[type]} {message}");
        }
    }
}
#endif