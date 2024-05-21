using Aki.Reflection.Patching;
using EFT;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace Arys.USPTP.Patches.NullGuardPatches;

internal class CoverPointMasterGetClosePointsPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {

        return AccessTools.Method(typeof(CoverPointMaster), nameof(CoverPointMaster.GetClosePoints));
    }

    [PatchPrefix]
    public static bool Prefix(BotOwner bot, ref List<CustomNavigationPoint> __result)
    {
        if (bot == null || bot.Covers == null)
        {
            __result = new List<CustomNavigationPoint>(); // Return an empty list or handle as needed
            return false;
        }
        return true;
    }
}
