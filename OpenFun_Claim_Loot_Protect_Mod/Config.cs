using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenFun_Claim_Loot_Protect_Mod
{
    public class Settings
    {
        private static string settingsFolder = string.Format(@"{0}/{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Settings");
        private static string settingsFile = "Settings.ini";
        private static string settingsFilePath = settingsFolder + @"/" + settingsFile;

        private static FileSystemWatcher fileWatcher;

        public static List<string> DeniedTileEntity = new List<string>();

        public static string DeniedMessage = "[FFA500]Server: Access denied[-]";

        public static void Init()
        {
            try
            {
                LoadSettings();
                fileWatcher = new FileSystemWatcher(settingsFolder, settingsFile);
                InitFileWatcher();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0} {1}", API.mod_name, ex));
            }
        }

        private static void InitFileWatcher()
        {
            try
            {
                fileWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
                fileWatcher.Created += new FileSystemEventHandler(OnFileChanged);
                fileWatcher.Deleted += new FileSystemEventHandler(OnFileChanged);
                fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0} {1}", API.mod_name, ex));
            }
        }

        private static void OnFileChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                fileWatcher.EnableRaisingEvents = false;
                LoadSettings();
                fileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0} {1}", API.mod_name, ex));
            }
        }

        private static void LoadSettings()
        {
            try
            {
                if (!Directory.Exists(settingsFolder))
                    Directory.CreateDirectory(settingsFolder);
                if (!File.Exists(settingsFilePath))
                {
                    string[] createText =
                    {
                        "### OpenFun_Claim_Loot_Protect_Mod Settings ###",
                        " ",
                        "DeniedMessage=[FFA500]Server: Access denied[-]",
                        " ",
                        "DeniedTileEntity=campfire",
                        "DeniedTileEntity=forge",
                        "DeniedTileEntity=cntDewCollector",
                        "DeniedTileEntity=workbench",
                        "DeniedTileEntity=chemistryStation",
                        "DeniedTileEntity=cementMixer",
                        "DeniedTileEntity=cntWoodWritableCrate",
                        "DeniedTileEntity=cntIronWritableCrate",
                        "DeniedTileEntity=cntSteelWritableCrate",
                        "DeniedTileEntity=cntWallSafe_Player",
                        "DeniedTileEntity=cntDeskSafe_Player",
                        "DeniedTileEntity=cntGunSafe_PlayerWhite",
                        "DeniedTileEntity=cntGunSafe_PlayerBrown",
                        "DeniedTileEntity=cntGunSafe_PlayerRed",
                        "DeniedTileEntity=cntGunSafe_PlayerOrange",
                        "DeniedTileEntity=cntGunSafe_PlayerYellow",
                        "DeniedTileEntity=cntGunSafe_PlayerGreen",
                        "DeniedTileEntity=cntGunSafe_PlayerBlue",
                        "DeniedTileEntity=cntGunSafe_PlayerPurple",
                        "DeniedTileEntity=cntGunSafe_PlayerGrey",
                        "DeniedTileEntity=cntGunSafe_Player",
                        "DeniedTileEntity=cntGunSafe_PlayerPink",
                        "DeniedTileEntity=cntGunSafe_PlayerArmyGreen",
                        " ",
                        "### The configuration is updated in real time if you change anything and save ###"
                    };
                    File.WriteAllLines(settingsFilePath, createText);
                }
                DeniedTileEntity.Clear();
                List<string> ReadFile = File.ReadAllLines(settingsFilePath).ToList();
                foreach (string line in ReadFile)
                {
                    if (line.StartsWith("DeniedMessage="))
                    {
                        string value = line.Replace("DeniedMessage=", "");
                        if (!string.IsNullOrEmpty(value))
                        {
                            DeniedMessage = value;
                        }
                    }
                    else if (line.StartsWith("DeniedTileEntity="))
                    {
                        string value = line.Replace("DeniedTileEntity=", "");
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (!DeniedTileEntity.Contains(value))
                                DeniedTileEntity.Add(value);
                        }
                    }
                }
                Log.Out(string.Format("{0} {1}", API.mod_name, "Reload settings..."));
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0} {1}", API.mod_name, ex));
            }
        }
    }
}
