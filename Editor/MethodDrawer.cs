using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEditor.DebugTools
{
    public class MethodDrawer
    {
        public static void DrawMethodFoldout(MethodInfo method, object[] targets, MethodInvoker invoker)
        {
            var parameters = method.GetParameters();
            string methodKey = MethodDiscovery.GetMethodKey(method);
            var targetType = targets[0].GetType();
            string prefKey = $"DebugMethods.Method.{targetType.FullName}.{methodKey}";

            // Prepare parameter storage early
            invoker.PrepareParameters(method);

            var unsupportedParams = parameters.Where(p => !MethodDiscovery.IsTypeSupported(p.ParameterType)).ToList();

            if (unsupportedParams.Any())
            {
                var unsupportedTypes = string.Join(", ", unsupportedParams.Select(p => p.ParameterType.Name));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent(method.Name + " (Unsupported)", $"Unsupported parameter types: {unsupportedTypes}"));
                if (!MethodDiscovery.IsUnityMethod(method))
                {
                    if (GUILayout.Button(DebugMethodGUIStyles.GotoSymbol, DebugMethodGUIStyles.UnsupportedGotoStyle, GUILayout.Width(30)))
                    {
                        invoker.OpenMethod(method);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                bool hasContent = parameters.Length > 0 || invoker.HasResult(method);

                if (hasContent)
                {
                    bool isExpanded = EditorPrefs.GetBool(prefKey, false);
                    EditorGUILayout.BeginHorizontal();
                     isExpanded = EditorGUILayout.Foldout(isExpanded, method.Name, true);
                     // Invoke Once Button (▶¹)
                     using (new EditorGUI.DisabledScope(!EditorApplication.isPlaying))
                     {
                         if (GUILayout.Button(DebugMethodGUIStyles.InvokeSymbol, DebugMethodGUIStyles.InvokeButtonStyle, GUILayout.Width(30)))
                         {
                             invoker.InvokeMethod(method, targets);
                             // Auto-expand foldout if method returns a value
                             if (method.ReturnType != typeof(void))
                             {
                                 EditorPrefs.SetBool(prefKey, true);
                             }
                         }

                         // Continuous Invoke Button (▶/■)
                         bool isContinuousActive = invoker.IsContinuousActive(method);
                         GUIStyle continuousStyle = isContinuousActive ? DebugMethodGUIStyles.ContinuousActiveStyle : DebugMethodGUIStyles.ContinuousInactiveStyle;
                         string continuousButtonText = isContinuousActive ? DebugMethodGUIStyles.ContinuousActiveSymbol : DebugMethodGUIStyles.ContinuousInactiveSymbol;
                         if (GUILayout.Button(continuousButtonText, continuousStyle, GUILayout.Width(30)))
                         {
                             invoker.ToggleContinuous(method);
                         }
                     }

                     // Code Link Button (→)
                     if (!MethodDiscovery.IsUnityMethod(method))
                     {
                         if (GUILayout.Button(DebugMethodGUIStyles.GotoSymbol, DebugMethodGUIStyles.GotoStyle, GUILayout.Width(30)))
                         {
                             invoker.OpenMethod(method);
                         }
                     }
                    EditorGUILayout.EndHorizontal();
                    EditorPrefs.SetBool(prefKey, isExpanded);

                    if (isExpanded)
                    {
                        EditorGUI.indentLevel++;

                        // Prepare parameter storage
                        invoker.PrepareParameters(method);

                        // Draw parameter fields
                        using (new EditorGUI.DisabledScope(!EditorApplication.isPlaying))
                        {
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                var param = parameters[i];
                                var currentValue = invoker.GetParameter(methodKey, i);
                                var newValue = invoker.DrawParameterField(param, currentValue);
                                invoker.SetParameter(methodKey, i, newValue);
                            }
                        }

                        // Display last result if available
                        invoker.DrawResult(method);

                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    // Simple layout for methods without parameters or results
                    EditorGUILayout.BeginHorizontal();
                     EditorGUILayout.LabelField(method.Name);
                     // Invoke Once Button (▶¹)
                     using (new EditorGUI.DisabledScope(!EditorApplication.isPlaying))
                     {
                         if (GUILayout.Button(DebugMethodGUIStyles.InvokeSymbol, DebugMethodGUIStyles.InvokeButtonStyle, GUILayout.Width(30)))
                         {
                             invoker.InvokeMethod(method, targets);
                             // Auto-expand foldout if method returns a value
                             if (method.ReturnType != typeof(void))
                             {
                                 EditorPrefs.SetBool(prefKey, true);
                             }
                         }

                         // Continuous Invoke Button (▶/■)
                         bool isContinuousActive = invoker.IsContinuousActive(method);
                         GUIStyle continuousStyle = isContinuousActive ? DebugMethodGUIStyles.ContinuousActiveStyle : DebugMethodGUIStyles.ContinuousInactiveStyle;
                         string continuousButtonText = isContinuousActive ? DebugMethodGUIStyles.ContinuousActiveSymbol : DebugMethodGUIStyles.ContinuousInactiveSymbol;
                         if (GUILayout.Button(continuousButtonText, continuousStyle, GUILayout.Width(30)))
                         {
                             invoker.ToggleContinuous(method);
                         }
                     }

                     // Code Link Button (→)
                     if (!MethodDiscovery.IsUnityMethod(method))
                     {
                         if (GUILayout.Button(DebugMethodGUIStyles.GotoSymbol, DebugMethodGUIStyles.GotoStyle, GUILayout.Width(30)))
                         {
                             invoker.OpenMethod(method);
                         }
                     }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
        }
    }
}