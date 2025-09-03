using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEditor.DebugTools
{
    public class CategorizedMethods
    {
        public List<MethodInfo> PublicSupported { get; set; }
        public List<MethodInfo> PublicUnsupported { get; set; }
        public List<MethodInfo> PrivateSupported { get; set; }
        public List<MethodInfo> PrivateUnsupported { get; set; }
        public List<MethodInfo> InheritedSupported { get; set; }
        public List<MethodInfo> InheritedUnsupported { get; set; }
        public List<MethodInfo> UnityMethods { get; set; }
        public bool HasUnsupported { get; set; }
    }

    public class MethodFilter
    {
        public static CategorizedMethods GetCategorizedMethods(object[] targets)
        {
            var commonMethods = MethodDiscovery.GetCommonMethods(targets);
            var targetType = targets[0].GetType();

            var declaredMethods = commonMethods.Where(m => m.DeclaringType == targetType).ToList();
            var inheritedMethods = commonMethods.Where(m => m.DeclaringType != targetType).ToList();

            var allUnityMethods = commonMethods.Where(m => MethodDiscovery.IsUnityMethod(m) && !m.GetParameters().Any(p => !MethodDiscovery.IsTypeSupported(p.ParameterType))).ToList();
            var allNonUnityMethods = commonMethods.Where(m => !MethodDiscovery.IsUnityMethod(m)).ToList();

            var declaredNonUnity = declaredMethods.Where(m => !MethodDiscovery.IsUnityMethod(m)).ToList();
            var inheritedNonUnity = inheritedMethods.Where(m => !MethodDiscovery.IsUnityMethod(m)).ToList();

            var publicDeclaredNonUnity = declaredNonUnity.Where(m => m.IsPublic).ToList();
            var privateDeclaredNonUnity = declaredNonUnity.Where(m => !m.IsPublic).ToList();

            // Split into supported and unsupported
            bool HasUnsupported(MethodInfo m) => m.GetParameters().Any(p => !MethodDiscovery.IsTypeSupported(p.ParameterType));
            var publicSupported = publicDeclaredNonUnity.Where(m => !HasUnsupported(m)).ToList();
            var publicUnsupported = publicDeclaredNonUnity.Where(HasUnsupported).ToList();
            var privateSupported = privateDeclaredNonUnity.Where(m => !HasUnsupported(m)).ToList();
            var privateUnsupported = privateDeclaredNonUnity.Where(HasUnsupported).ToList();
            var inheritedSupported = inheritedNonUnity.Where(m => !HasUnsupported(m)).ToList();
            var inheritedUnsupported = inheritedNonUnity.Where(HasUnsupported).ToList();

            bool hasUnsupportedInSections = publicUnsupported.Any() || privateUnsupported.Any() || inheritedUnsupported.Any();

            return new CategorizedMethods
            {
                PublicSupported = publicSupported,
                PublicUnsupported = publicUnsupported,
                PrivateSupported = privateSupported,
                PrivateUnsupported = privateUnsupported,
                InheritedSupported = inheritedSupported,
                InheritedUnsupported = inheritedUnsupported,
                UnityMethods = allUnityMethods,
                HasUnsupported = hasUnsupportedInSections
            };
        }
    }
}