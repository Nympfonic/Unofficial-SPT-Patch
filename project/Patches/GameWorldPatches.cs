using Aki.Reflection.Patching;
using EFT;
using HarmonyLib;
using System;
using System.Reflection;

namespace Arys.USPTP.Patches
{
    internal class GameWorldPatches
    {
        private static readonly Type _gameWorldType = typeof(GameWorld);

        internal class OnGameStarted : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.DeclaredMethod(_gameWorldType, nameof(GameWorld.OnGameStarted));
            }

            [PatchPostfix]
            private static void PatchPostfix(GameWorld __instance)
            {

            }
        }
    }
}
