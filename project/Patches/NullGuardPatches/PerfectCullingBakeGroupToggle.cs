﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aki.Reflection.Patching;
using Koenigz.PerfectCulling;

namespace Arys.USPTP.Patches.NullGuardPatches;

//Stops issue where game freezes if a gameobject is missing (such as deleting a door)
internal class PerfectCullingBakeGroupToggle : ModulePatch
{
    protected override MethodBase GetTargetMethod() => typeof(PerfectCullingBakeGroup).GetMethod(nameof(PerfectCullingBakeGroup.Toggle));

    [PatchPrefix]

    static bool Prefix(PerfectCullingBakeGroup __instance, bool rendererEnabled, int ___int_0, PerfectCullingBakeGroup.RuntimeGroupContent[] ___runtimeGroupContent_0)
    {
        // Safely handle Renderer[] array
        if (__instance.runtimeProxies != null)
        {
            foreach (var renderer in __instance.runtimeProxies)
            {
                if (renderer != null)
                {
                    renderer.enabled = !rendererEnabled;
                }
            }
        }

        // Safely handle CullingObject array
        if (__instance.cullingLightObjects != null)
        {
            foreach (var cullingObject in __instance.cullingLightObjects)
            {
                if (cullingObject != null)
                {
                    cullingObject.SetAutocullVisibility(rendererEnabled);
                }
            }
        }

        // Safely handle AnalyticSource array
        if (__instance.analyticSources != null)
        {
            foreach (var analyticSource in __instance.analyticSources)
            {
                if (analyticSource != null)
                {
                    analyticSource.IsAutocullVisible = rendererEnabled;
                }
            }
        }

        // Safely handle ScreenDistanceSwitcher
        if (__instance.screenDistanceSwitcher != null)
        {
            __instance.screenDistanceSwitcher.IsBakedAutocullVisible = rendererEnabled;
        }

        // Safely handle RuntimeGroupContent
        for (int j = 0; j < ___int_0; j++)
        {
            if (___runtimeGroupContent_0[j].Renderer != null)
            {
                ___runtimeGroupContent_0[j].Renderer.enabled = rendererEnabled;
            }
        }

        return false;
    }
}
