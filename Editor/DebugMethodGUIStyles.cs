using UnityEngine;
using UnityEditor;

namespace UnityEditor.DebugTools
{
    public static class DebugMethodGUIStyles
    {
        private static GUIStyle _invokeButtonStyle;
        public static GUIStyle InvokeButtonStyle => _invokeButtonStyle ??= new GUIStyle(GUI.skin.button) { normal = { textColor = Color.green } };

        private static GUIStyle _continuousActiveStyle;
        public static GUIStyle ContinuousActiveStyle => _continuousActiveStyle ??= new GUIStyle(GUI.skin.button) { normal = { textColor = Color.red } };

        private static GUIStyle _continuousInactiveStyle;
        public static GUIStyle ContinuousInactiveStyle => _continuousInactiveStyle ??= new GUIStyle(GUI.skin.button) { normal = { textColor = Color.green } };

        private static GUIStyle _gotoStyle;
        public static GUIStyle GotoStyle => _gotoStyle ??= new GUIStyle(GUI.skin.button) { normal = { textColor = new Color(0.3f, 0.6f, 1.0f) } };

        private static GUIStyle _unsupportedGotoStyle;
        public static GUIStyle UnsupportedGotoStyle => _unsupportedGotoStyle ??= new GUIStyle(GUI.skin.button) { normal = { textColor = new Color(0, 0, 0.5f) } };

        private static GUIStyle _copiedButtonStyle;
        public static GUIStyle CopiedButtonStyle => _copiedButtonStyle ??= new GUIStyle(GUI.skin.button) { normal = { textColor = Color.green } };

        public const string InvokeSymbol = "▶¹";
        public const string ContinuousActiveSymbol = "■";
        public const string ContinuousInactiveSymbol = "▶";
        public const string GotoSymbol = "→";
        public const string CopySymbol = "Copy";
        public const string CopiedSymbol = "√";
    }
}
