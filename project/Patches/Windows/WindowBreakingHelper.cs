using EFT.Interactive;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace Arys.USPTP.Patches.Windows
{
    internal static class WindowBreakingHelper
    {
        internal static Vector2 StaticMethod2(Vector2 pIn, Vector2 pOut, bool xAxis, float value)
        {
            return (Vector2)_staticMethod2.Invoke(null, [ pIn, pOut, xAxis, value ]);
        }

        private static readonly MethodInfo _staticMethod2 = AccessTools.FirstMethod(typeof(WindowBreakingConfig), mi =>
        {
            ParameterInfo[] parameters = mi.GetParameters();

            return parameters.Length == 4
                && parameters[0].ParameterType == typeof(Vector2)
                && parameters[0].Name == "pIn"
                && parameters[1].ParameterType == typeof(Vector2)
                && parameters[1].Name == "pOut"
                && parameters[2].ParameterType == typeof(bool)
                && parameters[2].Name == "xAxis"
                && parameters[3].ParameterType == typeof(float)
                && parameters[3].Name == "value";
        });
    }
}
