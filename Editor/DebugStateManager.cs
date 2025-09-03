using UnityEngine;
using UnityEditor;

namespace UnityEditor.DebugTools
{
    public class DebugStateManager
    {
        private bool showUnsupported = false;

        public bool ShowUnsupported
        {
            get => showUnsupported;
            set => showUnsupported = value;
        }

        public bool GetFoldoutState(string key, bool defaultValue = false)
        {
            return EditorPrefs.GetBool(key, defaultValue);
        }

        public void SetFoldoutState(string key, bool value)
        {
            EditorPrefs.SetBool(key, value);
        }
    }
}