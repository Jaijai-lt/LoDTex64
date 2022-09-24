using System.Reflection;
using HarmonyLib;

namespace LoDTex64 {

    public static class Patcher {
        private const string HarmonyId = "jaijai.lodtexture64";

        private static bool patched = false;

        public static void PatchAll() {
            if (patched) return;

            patched = true;

            var harmony = new Harmony(HarmonyId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void UnpatchAll() {
            if (!patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);

            patched = false;
        }
    }

    [HarmonyPatch(typeof(MeshSimplification), "BakeTextures")]
    public static class BakeTexturesPatch {
        public static void Prefix(ref int minSize) {
            UnityEngine.Debug.Log("Change minSize " + minSize + " to " + Mod.Config.texSize);
            minSize = Mod.Config.texSize;
        }
    }
}
