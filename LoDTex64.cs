using CitiesHarmony.API;
using ColossalFramework.UI;
using ICities;
using System;
using System.IO;
using System.Xml.Serialization;

namespace LoDTex64 {
    public class Mod : IUserMod {

        public string Name => "LoDTex64";
        public string Description => "Change the size of the auto-generated LoD textures instead 32x32 in the asset editor.";

        public static LodTex64Config Config;

        public void OnEnabled() {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            LoadSettings();
        }

        public void OnDisabled() {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        internal static UIDropDown sizeDropdown;

        public void OnSettingsUI(UIHelperBase helper) {
            sizeDropdown = (UIDropDown)helper.AddDropdown("LoD Texture Size", sizeList, Array.IndexOf(sizeList, Config.texSize.ToString()), delegate (int selectedIndex) {
                Config.texSize = Int32.Parse(sizeList[selectedIndex]);
                SaveSettings();
            });
        }

        internal static string[] sizeList = new string[] {
            "32",
            "64",
            "128",
            "256"
        };
                
        public string ConfigFilePath => Path.Combine(ColossalFramework.IO.DataLocation.localApplicationData, Name + "settings.xml");

        public void LoadSettings() {
            XmlSerializer serializer = new XmlSerializer(typeof(LodTex64Config));
            try {
                StreamReader reader = new StreamReader(ConfigFilePath, false);
                Config = (LodTex64Config)serializer.Deserialize(reader);
                reader.Close();
            }
            catch {
                if (Config == null) Config = new LodTex64Config();
            }
        }

        public void SaveSettings() {
            XmlSerializer serializer = new XmlSerializer(typeof(LodTex64Config));
            StreamWriter writer = new StreamWriter(ConfigFilePath, false);
            serializer.Serialize(writer, Config);
            writer.Close();
        }
    }

    public class LodTex64Config {
        public int texSize = 64;
    }

}

