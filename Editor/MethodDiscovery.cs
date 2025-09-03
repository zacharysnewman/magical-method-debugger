using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEditor.DebugTools
{
    public static class MethodDiscovery
    {
        private static readonly HashSet<string> unityLifecycleMethods = new()
        {
            "Awake", "Start", "Update", "FixedUpdate", "LateUpdate",
            "OnEnable", "OnDisable", "OnDestroy", "OnApplicationQuit",
            "OnGUI", "OnDrawGizmos", "OnDrawGizmosSelected", "OnValidate", "Reset"
        };

        public static bool IsUnityMethod(MethodInfo method)
        {
            if (unityLifecycleMethods.Contains(method.Name))
                return true;

            return method.DeclaringType.Namespace?.StartsWith("UnityEngine") == true;
        }

        public static bool IsTypeSupported(Type type)
        {
            if (type == typeof(int) || type == typeof(float) || type == typeof(bool) || type == typeof(string))
                return true;

            if (type.IsEnum)
                return true;

            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                return true;

            return false;
        }

        public static string GetMethodKey(MethodInfo method)
        {
            var paramTypes = method.GetParameters().Select(p => p.ParameterType.Name);
            return $"{method.DeclaringType.FullName}.{method.Name}({string.Join(",", paramTypes)})";
        }

        public static List<MethodInfo> GetCommonMethods(object[] targets)
        {
            if (targets.Length == 0) return new List<MethodInfo>();

            var firstType = targets[0].GetType();
            var common = firstType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object) && !m.IsGenericMethod) // ignore property getters/setters, System.Object methods, and generic methods
                .ToList();

            for (int i = 1; i < targets.Length; i++)
            {
                var tMethods = targets[i].GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object) && !m.IsGenericMethod);

                common = common
                    .Where(m => tMethods.Any(tm => MethodsMatch(m, tm)))
                    .ToList();
            }

            return common;
        }

        public static bool MethodsMatch(MethodInfo a, MethodInfo b)
        {
            if (a.Name != b.Name) return false;

            var aParams = a.GetParameters();
            var bParams = b.GetParameters();

            if (aParams.Length != bParams.Length) return false;

            for (int i = 0; i < aParams.Length; i++)
            {
                if (aParams[i].ParameterType != bParams[i].ParameterType)
                    return false;
            }

            return true;
        }
    }
}