using Aki.Reflection.Patching;
using EFT.Interactive;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Arys.USPTP.Patches.Windows
{
    internal class WindowBreakingConfigPatches
    {
        private static readonly Type _targetType = typeof(WindowBreakingConfig);

        internal class StaticMethod3 : ModulePatch
        {
            private struct MainJob : IJobFor
            {
                [ReadOnly] internal NativeArray<Vector2> points;
                [ReadOnly] internal int num;
                [ReadOnly] internal bool positive;
                [ReadOnly] internal bool xAxis;
                [ReadOnly] internal float value;

                internal bool flag;
                internal Vector2 vector;
                internal NativeList<Vector2> result;

                public void Execute(int index)
                {
                    Vector2 vector2 = points[index];
                    bool flag2 = positive == vector2[num] < value;
                    if (flag == flag2)
                    {
                        if (!flag2)
                        {
                            result.AddNoResize(vector2);
                        }
                    }
                    else if (flag2)
                    {
                        result.AddNoResize(WindowBreakingHelper.StaticMethod2(vector, vector2, xAxis, value));
                    }
                    else
                    {
                        result.AddNoResize(WindowBreakingHelper.StaticMethod2(vector2, vector, xAxis, value));
                        result.AddNoResize(vector2);
                    }
                    vector = vector2;
                    flag = flag2;
                }
            }

            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.FirstMethod(_targetType, IsTargetMethod);
            }

            private bool IsTargetMethod(MethodInfo mi)
            {
                ParameterInfo[] parameters = mi.GetParameters();

                return parameters.Length == 5
                    && parameters[0].ParameterType == typeof(List<Vector2>)
                    && parameters[0].Name == "newList"
                    && parameters[1].ParameterType == typeof(List<Vector2>)
                    && parameters[1].Name == "points"
                    && parameters[2].ParameterType == typeof(bool)
                    && parameters[2].Name == "xAxis"
                    && parameters[3].ParameterType == typeof(bool)
                    && parameters[3].Name == "positive"
                    && parameters[4].ParameterType == typeof(float)
                    && parameters[4].Name == "value";
            }

            [PatchPrefix]
            private static bool PatchPrefix(
                List<Vector2> newList,
                List<Vector2> points,
                bool xAxis,
                bool positive,
                float value
            )
            {
                int count = points.Count;
                Vector2 vector = points[count - 1];
                int num = xAxis ? 0 : 1;
                bool flag = positive == vector[num] < value;

                var pointsArray = new NativeArray<Vector2>(points.ToArray(), Allocator.TempJob);
                var resultList = new NativeList<Vector2>(count * 2, Allocator.TempJob);

                var job = new MainJob()
                {
                    num = num,
                    flag = flag,
                    positive = positive,
                    value = value,
                    vector = vector,
                    xAxis = xAxis,
                    points = pointsArray,
                    result = resultList
                };

                JobHandle jobHandle = job.Schedule(count, new JobHandle());

                jobHandle.Complete();

                resultList.CopyTo(newList);

                pointsArray.Dispose();
                resultList.Dispose();

                return false;
            }
        }

        internal class StaticMethod4 : ModulePatch
        {
            private struct MainJob : IJobParallelFor
            {
                [ReadOnly] internal NativeArray<Vector2> points;
                [ReadOnly] internal Vector2 xAxis;

                internal NativeArray<Vector2> result;

                public void Execute(int index)
                {
                    float x = xAxis.x;
                    float y = xAxis.y;
                    float num = -y;

                    float x2 = points[index].x;
                    float y2 = points[index].y;

                    var vector = new Vector2
                    {
                        x = x2 * x + y2 * num,
                        y = x2 * y + y2 * x
                    };

                    result[index] = vector;
                }
            }

            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.FirstMethod(_targetType, IsTargetMethod);
            }

            private bool IsTargetMethod(MethodInfo mi)
            {
                ParameterInfo[] parameters = mi.GetParameters();

                return parameters.Length == 2
                    && parameters[0].ParameterType == typeof(Vector2[])
                    && parameters[0].Name == "points"
                    && parameters[1].ParameterType == typeof(Vector2)
                    && parameters[1].Name == "xAxis";
            }

            [PatchPrefix]
            private static bool PatchPrefix(ref Vector2[] __result, Vector2[] points, Vector2 xAxis)
            {
                var pointsArray = new NativeArray<Vector2>(points, Allocator.TempJob);
                var resultArray = new NativeArray<Vector2>(points.Length, Allocator.TempJob);

                var job = new MainJob
                {
                    points = pointsArray,
                    xAxis = xAxis,
                    result = resultArray
                };

                JobHandle jobHandle = job.Schedule(resultArray.Length, 64);

                jobHandle.Complete();

                __result = [.. resultArray];

                pointsArray.Dispose();
                resultArray.Dispose();

                return false;
            }
        }

        internal class StaticMethod7 : ModulePatch
        {
            private struct MainJob : IJobFor
            {
                [ReadOnly] internal NativeArray<Vector2> points;
                [ReadOnly] internal float value;
                [ReadOnly] internal bool xAxis;

                internal bool flag;
                internal bool flag2;

                public void Execute(int index)
                {
                    if (xAxis)
                    {
                        if (points[index].x < value)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag2 = true;
                        }
                    }
                    else
                    {
                        if (points[index].y < value)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag2 = true;
                        }
                    }
                }
            }

            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.FirstMethod(_targetType, IsTargetMethod);
            }

            private bool IsTargetMethod(MethodInfo mi)
            {
                ParameterInfo[] parameters = mi.GetParameters();

                return parameters.Length == 4
                    && parameters[0].ParameterType == typeof(List<Vector2>)
                    && parameters[0].Name == "points"
                    && parameters[1].ParameterType == typeof(bool)
                    && parameters[1].Name == "xAxis"
                    && parameters[2].ParameterType == typeof(bool)
                    && parameters[2].Name == "positive"
                    && parameters[3].ParameterType == typeof(float)
                    && parameters[3].Name == "value";
            }

            [PatchPrefix]
            private static bool PatchPrefix(ref int __result, List<Vector2> points, bool xAxis, bool positive, float value)
            {
                bool flag = false;
                bool flag2 = false;
                
                var pointsArray = new NativeArray<Vector2>(points.ToArray(), Allocator.TempJob);
                
                var job = new MainJob()
                {
                    points = pointsArray,
                    value = value,
                    xAxis = xAxis,
                    flag = flag,
                    flag2 = flag2
                };

                JobHandle jobHandle = job.Schedule(points.Count, new JobHandle());

                jobHandle.Complete();

                if (positive)
                {
                    bool flag3 = job.flag2;
                    flag2 = job.flag;
                    flag = flag3;
                }

                pointsArray.Dispose();

                if (!flag2)
                {
                    __result = 1;
                    return false;
                }

                if (!flag)
                {
                    __result = -1;
                    return false;
                }

                __result = 0;

                return false;
            }
        }

        internal class GClassGetMesh : ModulePatch
        {
            private struct MainJob : IJobParallelFor
            {
                [ReadOnly] internal NativeArray<Vector3> vertices;
                [ReadOnly] internal Vector2 center;

                internal NativeArray<Vector3> result;

                public void Execute(int index)
                {
                    Vector3 vector = vertices[index];
                    result[index] = new Vector3(vector.x - center.x, vector.y - center.y, vector.z);
                }
            }

            protected override MethodBase GetTargetMethod()
            {
                Type innerType = AccessTools.FirstInner(_targetType, type =>
                {
                    return AccessTools.DeclaredMethod(type, "GetMesh") != null;
                });

                return AccessTools.DeclaredMethod(innerType, "GetMesh");
            }

            [PatchPrefix]
            private static bool PatchPrefix(
                ref Mesh __result,
                Vector2 center,
                int[] ___Triangles,
                Vector3[] ___Vertices,
                Vector3[] ___Normals,
                Vector2[] ___Uv
            )
            {
                var verticesArray = new NativeArray<Vector3>(___Vertices, Allocator.TempJob);
                var resultArray = new NativeArray<Vector3>(___Vertices.Length, Allocator.TempJob);

                var job = new MainJob()
                {
                    vertices = verticesArray,
                    center = center,
                    result = resultArray
                };

                JobHandle jobHandle = job.Schedule(___Vertices.Length, 64);

                jobHandle.Complete();                

                __result = new Mesh()
                {
                    vertices = [.. resultArray],
                    triangles = ___Triangles,
                    normals = ___Normals,
                    uv = ___Uv,
                    name = "WindowBreakingConfig GetMesh"
                };

                verticesArray.Dispose();
                resultArray.Dispose();

                return false;
            }
        }
    }

    internal class WindowBreakerPatches
    {
        private static readonly Type _targetType = typeof(WindowBreaker);

        //internal class CreateShardPieces : ModulePatch
        //{
        //    protected override MethodBase GetTargetMethod()
        //    {
        //        return AccessTools.FirstMethod(_windowBreakerType, IsTargetMethod);
        //    }

        //    private bool IsTargetMethod(MethodInfo mi)
        //    {
        //        ParameterInfo[] parameters = mi.GetParameters();

        //        return parameters.Length == 5
        //            && parameters[0].ParameterType == typeof(WindowBreakingConfig.Crack)
        //            && parameters[1].ParameterType == typeof(Vector2)
        //            && parameters[2].ParameterType == typeof(Vector2)
        //            && parameters[3].ParameterType == typeof(float)
        //            && parameters[4].ParameterType == typeof(float);
        //    }

        //    [PatchPrefix]
        //    private static bool PatchPrefix(
        //        WindowBreaker __instance,
        //        ref int ___int_1,
        //        List<object> ___list_0,
        //        bool ___NeedToSwap,
        //        Vector2 ___UvMult,
        //        Vector2 ___UvAdd,
        //        Vector2 ___ZSurfs,
        //        Vector4 ___Box,
        //        ref object[] __result,
        //        WindowBreakingConfig.Crack crack,
        //        Vector2 add,
        //        Vector2 shift,
        //        float angle,
        //        float scale
        //    )
        //    {
        //        object[] shards = [0];
        //        var windowBreakerFieldData = new WindowBreakerFieldData(
        //            ___list_0, ___NeedToSwap, ___UvMult, ___UvAdd, ___ZSurfs, ___Box);

        //        UniTask.RunOnThreadPool(async () =>
        //        {
        //            shards = await CreateWindowShards(__instance, ref ___int_1, windowBreakerFieldData, crack, add, shift, angle, scale);
        //        });

        //        __result = shards;
        //        return false;
        //    }

        //    private static async UniTask<object[]> CreateWindowShards(
        //        WindowBreaker windowBreaker,
        //        ref int int_1,
        //        WindowBreakerFieldData fieldData,
        //        WindowBreakingConfig.Crack crack,
        //        Vector2 add,
        //        Vector2 shift,
        //        float angle,
        //        float scale
        //    )
        //    {
        //        Vector4 box = fieldData.Box;
        //        box.x -= add.x;
        //        box.z -= add.x;
        //        box.y -= add.y;
        //        box.w -= add.y;

        //        float num = UnityEngine.Random.Range(windowBreaker.FirstShotRadius * 0.9f, windowBreaker.FirstShotRadius * 1.1f);
        //        Vector2 vector = WindowBreakingConfig.GetXAxis(angle) * scale;

        //        for (int i = 0; i < crack.Polygons.Length; i++)
        //        {
        //            // CutPolygon seems to be expensive
        //            // Try using Unity Jobs?
        //            WindowBreakingConfig.Polygon polygon = crack.Polygons[i].CutPolygon(box, vector);
        //            if (polygon.Points != null && polygon.Points.Length >= 3)
        //            {
        //                Vector2 vector2 = Vector3.zero;
        //                foreach (Vector2 vector3 in polygon.Points)
        //                {
        //                    vector2 += vector3;
        //                }
        //                vector2 /= polygon.Points.Length;

        //                var gclass = WindowBreakingConfig.GenerateMeshPice(
        //                    polygon.Points,
        //                    fieldData.NeedToSwap,
        //                    fieldData.UvMult,
        //                    fieldData.UvAdd + Vector2.Scale(add, fieldData.UvMult),
        //                    fieldData.ZSurfs,
        //                    windowBreaker.EdgesWidth);

        //                var shardClass = new WindowBreaker.Class2249
        //                {
        //                    Id = windowBreaker.Id + "_piece" + int_1.ToString(),
        //                    MeshPiece = gclass,
        //                    Center = vector2,
        //                    Stuck = (shift - vector2).magnitude > num,
        //                    Edge = polygon.Intersects,
        //                    Mass = polygon.Mass * windowBreaker.MassMultyplier
        //                };

        //                int_1++;
        //                fieldData.List_0.Add(shardClass);
        //            }
        //        }
        //    }
        //}
    }
}
