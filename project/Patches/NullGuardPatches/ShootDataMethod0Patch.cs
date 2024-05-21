using Aki.Reflection.Patching;
using EFT;
using HarmonyLib;
using System.Reflection;

namespace Arys.USPTP.Patches.NullGuardPatches;

internal class ShootDataMethod0Patch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(ShootData), nameof(ShootData.method_0));
    }

    [PatchPrefix]
    private static bool Prefix(BotOwner ____owner)
    {
        // Check for null references in necessary fields
        if (____owner == null)
        {
            //Debug.LogError("ShootData.method_0(): _owner is null.");
            return false;
        }

        if (____owner.WeaponRoot == null)
        {
            //Debug.LogError("ShootData.method_0(): _owner.WeaponRoot is null.");
            return false;
        }

        if (____owner.Position == null)
        {
            //Debug.LogError("ShootData.method_0(): _owner.Position is null.");
            return false;
        }

        return true;
    }
}
