using HarmonyLib;
using System;
using System.Threading;

namespace OpenFun_Claim_Loot_Protect_Mod
{
    public class API : IModApi
    {
        public static string mod_name = "[OpenFun_Claim_Loot_Protect_Mod]";

        private Harmony harmony = new Harmony("OCLPM_GameManager_OpenTileEntityAllowed");

        public void InitMod(Mod _modInstance)
        {
            ModEvents.GameStartDone.RegisterHandler(new Action(this.GameStartDone));
        }

        public void GameStartDone()
        {
            harmony.PatchAll();
            new Thread(delegate () { Settings.Init(); }).Start();
        }
    }
}
