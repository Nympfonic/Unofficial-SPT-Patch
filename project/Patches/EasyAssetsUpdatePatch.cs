using Aki.Reflection.Patching;
using Cysharp.Threading.Tasks;
using Diz.Resources;
using HarmonyLib;
using System.Reflection;

namespace Arys.USPTP.Patches
{
    internal class EasyAssetsUpdatePatch : ModulePatch
    {
        private static bool _updateInitialised = false;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.DeclaredMethod(typeof(EasyAssets), nameof(EasyAssets.Update));
        }

        [PatchPrefix]
        private static bool PatchPrefix(EasyAssets __instance)
        {
            if (!_updateInitialised)
            {
                UniTask.RunOnThreadPool(async () =>
                {
                    while (true)
                    {
                        __instance.System.Update();
                        await UniTask.Yield();
                    }
                });

                _updateInitialised = true;
            }

            return false;
        }
    }
}
