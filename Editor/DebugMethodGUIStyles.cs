using UnityEngine;
using UnityEditor;

namespace UnityEditor.DebugTools
{
    public static class DebugMethodGUIStyles
    {
        public static readonly GUIStyle InvokeButtonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.green } };
        public static readonly GUIStyle ContinuousActiveStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.red } };
        public static readonly GUIStyle ContinuousInactiveStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.green } };
        public static readonly GUIStyle GotoStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = new Color(0.3f, 0.6f, 1.0f) } };
        public static readonly GUIStyle UnsupportedGotoStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = new Color(0, 0, 0.5f) } };
        public static readonly GUIStyle CopiedButtonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = Color.green } };

        public const string InvokeSymbol = "▶¹";
        public const string ContinuousActiveSymbol = "■";
        public const string ContinuousInactiveSymbol = "▶";
        public const string GotoSymbol = "→";
        public const string CopySymbol = "Copy";
        public const string CopiedSymbol = "√";
    }
}