using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEditor.DebugTools
{
    public class DebugMethodGUI
    {
        private MethodInvoker invoker;

        private object[] currentTargets;
        private DebugStateManager stateManager;

        public DebugMethodGUI(MethodInvoker invoker)
        {
            this.invoker = invoker;
            stateManager = new DebugStateManager();
            EditorApplication.update += OnEditorUpdate;
        }

        ~DebugMethodGUI()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            // Invoke continuous methods if we have targets
            if (currentTargets != null && currentTargets.Length > 0)
            {
                invoker.InvokeContinuousMethods(currentTargets);
            }
        }

        public void ClearContinuous()
        {
            invoker.ClearContinuousStates();
        }

        public void DrawInspector(object[] targets)
        {
            currentTargets = targets;
            string mainKey = $"DebugMethods.Main.{targets[0].GetType().FullName}";
            bool showDebugMethods = stateManager.GetFoldoutState(mainKey, false);
            showDebugMethods = EditorGUILayout.Foldout(showDebugMethods, "Method Debugger", true);
            stateManager.SetFoldoutState(mainKey, showDebugMethods);

            if (showDebugMethods)
            {
                EditorGUI.indentLevel++;

                var categorized = MethodFilter.GetCategorizedMethods(targets);
                var targetType = targets[0].GetType();

                // Public declared non-Unity
                if (categorized.PublicSupported.Any() || (stateManager.ShowUnsupported && categorized.PublicUnsupported.Any()))
                {
                    string key = $"DebugMethods.Public.{targetType.FullName}";
                    bool expanded = stateManager.GetFoldoutState(key, false);
                    expanded = EditorGUILayout.Foldout(expanded, "Public", true);
                    stateManager.SetFoldoutState(key, expanded);
                    if (expanded)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var method in categorized.PublicSupported)
                        {
                            MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                        }
                        if (stateManager.ShowUnsupported)
                        {
                            foreach (var method in categorized.PublicUnsupported)
                            {
                                MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                // Private declared non-Unity
                if (categorized.PrivateSupported.Any() || (stateManager.ShowUnsupported && categorized.PrivateUnsupported.Any()))
                {
                    string key = $"DebugMethods.Private.{targetType.FullName}";
                    bool expanded = stateManager.GetFoldoutState(key, false);
                    expanded = EditorGUILayout.Foldout(expanded, "Private", true);
                    stateManager.SetFoldoutState(key, expanded);
                    if (expanded)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var method in categorized.PrivateSupported)
                        {
                            MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                        }
                        if (stateManager.ShowUnsupported)
                        {
                            foreach (var method in categorized.PrivateUnsupported)
                            {
                                MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                // Inherited non-Unity
                if (categorized.InheritedSupported.Any() || (stateManager.ShowUnsupported && categorized.InheritedUnsupported.Any()))
                {
                    string key = $"DebugMethods.Base.{targetType.FullName}";
                    bool expanded = stateManager.GetFoldoutState(key, false);
                    string baseName = targetType.BaseType?.Name ?? "Object";
                    expanded = EditorGUILayout.Foldout(expanded, $"{baseName} (Inherited)", true);
                    stateManager.SetFoldoutState(key, expanded);
                    if (expanded)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var method in categorized.InheritedSupported)
                        {
                            MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                        }
                        if (stateManager.ShowUnsupported)
                        {
                            foreach (var method in categorized.InheritedUnsupported)
                            {
                                MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                // All Unity methods
                if (categorized.UnityMethods.Any())
                {
                    string key = $"DebugMethods.Unity.{targetType.FullName}";
                    bool expanded = stateManager.GetFoldoutState(key, false);
                    expanded = EditorGUILayout.Foldout(expanded, "MonoBehaviour", true);
                    stateManager.SetFoldoutState(key, expanded);
                    if (expanded)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var method in categorized.UnityMethods)
                        {
                            MethodDrawer.DrawMethodFoldout(method, targets, invoker);
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                if (categorized.HasUnsupported)
                {
                    stateManager.ShowUnsupported = EditorGUILayout.Toggle("Show Unsupported", stateManager.ShowUnsupported);
                    EditorGUILayout.HelpBox("Some methods have unsupported parameters and are hidden by default.", MessageType.Warning);
                }

                EditorGUI.indentLevel--;
            }
        }


    }
}