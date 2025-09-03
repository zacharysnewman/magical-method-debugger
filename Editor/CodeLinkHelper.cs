using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Reflection;
using System.Linq;
// using Mono.Cecil;

namespace UnityEditor.DebugTools
{
    public static class CodeLinkHelper
    {
        public static void OpenMethodInEditor(MethodInfo method)
        {
            // if (method == null || method.DeclaringType == null)
            //     return;

            // string assemblyPath = method.DeclaringType.Assembly.Location;
            // string pdbPath = Path.ChangeExtension(assemblyPath, "pdb");

            // if (!File.Exists(pdbPath))
            // {
            //     Debug.LogWarning("No PDB symbols found. Falling back to line 1.");
            //     OpenAtLine(method.DeclaringType, 1);
            //     return;
            // }

            // using var resolver = new DefaultAssemblyResolver();
            // resolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));

            // var readerParams = new ReaderParameters
            // {
            //     ReadSymbols = true,
            //     AssemblyResolver = resolver
            // };

            // var assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath, readerParams);
            // var typeDef = assemblyDef.MainModule.GetType(method.DeclaringType.FullName);

            // var methodDef = typeDef?.Methods.FirstOrDefault(m => m.MetadataToken.ToInt32() == method.MetadataToken);
            // if (methodDef?.DebugInformation.HasSequencePoints == true)
            // {
            //     var sp = methodDef.DebugInformation.SequencePoints.FirstOrDefault();
            //     if (sp != null)
            //     {
            //         int line = sp.StartLine > 1 ? sp.StartLine - 1 : 1;
            //         OpenAtLine(method.DeclaringType, line, sp.Document.Url);
            //         return;
            //     }
            // }

            // // fallback
            // OpenAtLine(method.DeclaringType, 1);
        }

        private static void OpenAtLine(System.Type declaringType, int line, string filePathOverride = null)
        {
            // MonoScript script = null;

            // var scripts = AssetDatabase.FindAssets($"t:MonoScript {declaringType.Name}");
            // foreach (var guid in scripts)
            // {
            //     var candidate = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(guid));
            //     if (candidate != null && candidate.GetClass() == declaringType)
            //     {
            //         script = candidate;
            //         break;
            //     }
            // }

            // if (script == null)
            // {
            //     Debug.LogWarning($"Could not locate script for {declaringType}");
            //     return;
            // }

            // var path = filePathOverride ?? AssetDatabase.GetAssetPath(script);
            // InternalEditorUtility.OpenFileAtLineExternal(path, line);
        }
    }
}
