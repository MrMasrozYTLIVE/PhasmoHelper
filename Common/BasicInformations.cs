using System;
using UnityEngine;

namespace PhasmoHelper
{
    class BasicInformations
    {
        public static void EnableGhost()
        {
            if (Main.initializedScene > 1)
            {
                if (Main.levelController != null && Main.ghostInfo != null && firstRun && CheatToggles.enableBIGhost)
                {
                    Debug.Msg("ghostNameAge", 3);
                    if (Main.ghostInfo.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0.field_Public_String_0 != "" && Main.ghostInfo.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0.field_Public_Int32_0 > 0)
                        Main.ghostNameAge = Main.ghostInfo.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0.field_Public_String_0 ;
                }
            }
        }

        public static void DisableGhost()
        {
            Main.ghostNameAge = "";
            firstRun = true;
        }

        public static void EnablePlayer()
        {
            if (Main.initializedScene > 1 && (CheatToggles.enableBIPlayer || CheatToggles.enableBI) && Main.myPlayer.field_Public_PlayerSanity_0.field_Public_Single_0 > -1)
            {
                Debug.Msg("myPlayerSanity", 3);
                Main.myPlayerSanity = Math.Round(100 - Main.myPlayer.field_Public_PlayerSanity_0.field_Public_Single_0, 0).ToString();
            }
        }

        public static void EnableMissions()
        {
            if (Main.initializedScene > 1 && Main.levelController != null && MissionManager.field_Public_Static_MissionManager_0.field_Public_List_1_Mission_0 != null && CheatToggles.enableBIMissions)
            {
                int missionNum = 1;
                Debug.Msg("missions", 3);
                foreach (Mission mission in MissionManager.field_Public_Static_MissionManager_0.field_Public_List_1_Mission_0)
                {
                    GUI.Label(new Rect(10f, 32f + (float)missionNum * 15f, 600f, 50f), string.Concat(new object[]
                    {
                        ((mission.field_Public_Boolean_0) ? "<color=#CCCCCC>" : "<color=#00FF00>"),
                        "<b>" + missionNum + "</b>",
                        ") ",
                        mission.field_Public_String_0,
                        "</color>"
                    }));
                    missionNum++;
                }
            }
        }

        public static bool firstRun = true;
    }
}