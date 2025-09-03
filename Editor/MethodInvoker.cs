using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using static UnityEditor.DebugTools.CodeLinkHelper;

namespace UnityEditor.DebugTools
{
    public class MethodInvoker
    {
        private Dictionary<string, object[]> methodParameters = new();
        private Dictionary<string, object> lastResults = new();
        private Dictionary<string, double> lastCopyTimes = new();
        private Dictionary<string, bool> continuousStates = new();
        private List<MethodInfo> activeMethods = new();
        private int lastFrameCount = -1;

        public void PrepareParameters(MethodInfo method)
        {
            string methodKey = MethodDiscovery.GetMethodKey(method);
            if (!methodParameters.ContainsKey(methodKey))
                methodParameters[methodKey] = new object[method.GetParameters().Length];
        }

        public void InvokeMethod(MethodInfo method, object[] targets, bool log = true)
        {
            string methodKey = MethodDiscovery.GetMethodKey(method);
            foreach (var t in targets)
            {
                try
                {
                    var result = method.Invoke(t, methodParameters[methodKey]);
                    if (log)
                    {
                        if (method.ReturnType == typeof(void))
                        {
                            Debug.Log($"Invoked {method.Name} on {t} (void)");
                        }
                        else
                        {
                            Debug.Log($"Invoked {method.Name} on {t}, returned: {result}");
                            lastResults[methodKey] = result;
                        }
                    }
                    else
                    {
                        if (method.ReturnType != typeof(void))
                        {
                            lastResults[methodKey] = result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (log)
                    {
                        Debug.LogError($"Error invoking {method.Name} on {t}: {ex}");
                    }
                }
            }
        }

        public void DrawResult(MethodInfo method)
        {
            string methodKey = MethodDiscovery.GetMethodKey(method);
            if (lastResults.ContainsKey(methodKey) && method.ReturnType != typeof(void))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Result: {lastResults[methodKey]}");
                string buttonText;
                GUIStyle buttonStyle;
                if (lastCopyTimes.ContainsKey(methodKey) && EditorApplication.timeSinceStartup - lastCopyTimes[methodKey] < 1)
                {
                    buttonText = DebugMethodGUIStyles.CopiedSymbol;
                    buttonStyle = DebugMethodGUIStyles.CopiedButtonStyle;
                }
                else
                {
                    buttonText = DebugMethodGUIStyles.CopySymbol;
                    buttonStyle = GUI.skin.button;
                }
                if (GUILayout.Button(buttonText, buttonStyle, GUILayout.Width(50)))
                {
                    if (buttonText == DebugMethodGUIStyles.CopySymbol)
                    {
                        GUIUtility.systemCopyBuffer = lastResults[methodKey]?.ToString() ?? "null";
                        lastCopyTimes[methodKey] = EditorApplication.timeSinceStartup;
                    }
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
        }

        public object DrawParameterField(ParameterInfo param, object currentValue)
        {
            var type = param.ParameterType;

            if (type == typeof(int))
                return EditorGUILayout.IntField(param.Name, currentValue is int v ? v : 0);

            if (type == typeof(float))
                return EditorGUILayout.FloatField(param.Name, currentValue is float v ? v : 0f);

            if (type == typeof(bool))
                return EditorGUILayout.Toggle(param.Name, currentValue is bool v && v);

            if (type == typeof(string))
                return EditorGUILayout.TextField(param.Name, currentValue as string ?? "");

            if (type.IsEnum)
                return EditorGUILayout.EnumPopup(param.Name, currentValue as Enum ?? (Enum)Enum.GetValues(type).GetValue(0));

            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                return EditorGUILayout.ObjectField(param.Name, currentValue as UnityEngine.Object, type, true);

            EditorGUILayout.LabelField(new GUIContent($"{param.Name} (Unsupported: {type.Name})", $"Parameter type '{type.Name}' is not supported for editing."));
            return currentValue;
        }

        public object GetParameter(string methodKey, int index)
        {
            return methodParameters[methodKey][index];
        }

        public void SetParameter(string methodKey, int index, object value)
        {
            methodParameters[methodKey][index] = value;
        }

        public void OpenMethod(MethodInfo method)
        {
            OpenMethodInEditor(method);
        }

        public bool HasResult(MethodInfo method)
        {
            string methodKey = MethodDiscovery.GetMethodKey(method);
            return lastResults.ContainsKey(methodKey) && method.ReturnType != typeof(void);
        }

        public void ToggleContinuous(MethodInfo method)
        {
            string methodKey = MethodDiscovery.GetMethodKey(method);
            bool isActive = continuousStates.ContainsKey(methodKey) && continuousStates[methodKey];

            if (isActive)
            {
                // Deactivate continuous invocation
                continuousStates[methodKey] = false;
                activeMethods.Remove(method);
            }
            else
            {
                // Activate continuous invocation
                continuousStates[methodKey] = true;
                if (!activeMethods.Contains(method))
                {
                    activeMethods.Add(method);
                }
            }
        }

        public bool IsContinuousActive(MethodInfo method)
        {
            string methodKey = MethodDiscovery.GetMethodKey(method);
            return continuousStates.ContainsKey(methodKey) && continuousStates[methodKey];
        }

        public void InvokeContinuousMethods(object[] targets)
        {
            if (!Application.isPlaying) return;

            // Sync with game frame rate
            if (Time.frameCount != lastFrameCount)
            {
                lastFrameCount = Time.frameCount;

                foreach (var method in activeMethods.ToArray()) // Use ToArray to avoid modification during iteration
                {
                    InvokeMethod(method, targets, false);
                }
            }
        }

        public void ClearContinuousStates()
        {
            continuousStates.Clear();
            activeMethods.Clear();
        }
    }
}