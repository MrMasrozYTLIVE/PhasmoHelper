using MelonLoader;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Console = System.Console;
using Object = UnityEngine.Object;
using String = System.String;

namespace PhasmoHelper
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            BasicInject.Main();
            Debug.Msg("Set console title to: Phasmophobia", 1);
            Console.Title = string.Format("Phasmophobia");

            HandleConfig();
        }

        public override void OnLevelWasLoaded(int index)
        {
            if (initializedScene == 1)
                gameStarted = true;
        }

        public override void OnLevelWasInitialized(int index)
        {
            initializedScene = index;
            if (initializedScene > 0 && canRun)
            {
                canRun = false;
                coRoutine = null;
                isRunning = false;
                new Thread(() =>
                {
                    while (true)
                    {
                        if (!isRunning)
                        {
                            coRoutine = MelonCoroutines.Start(CollectGameObjects());
                        }
                        Thread.Sleep(5000);
                    }
                }).Start();
            }
            Debug.Msg("Initialized Scene: " + mapNames[initializedScene], 1);
            if (gameStarted && initializedScene == 1)
                DisableAll();

            if (initializedScene == 1 && !canRun)
            {
                MelonCoroutines.Stop(coRoutine);
                canRun = true;
            }
        }
        public override void OnUpdate()
        {
            getPing = PhotonNetwork.GetPing();

            Keyboard keyboard = Keyboard.current;

            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                ModToggles.enableBI = !ModToggles.enableBI;
                if (ModToggles.enableBI)
                {
                    ModToggles.enableBIGhost = true;
                    ModToggles.enableBIMissions = true;
                    ModToggles.enableBIPlayer = true;
                }
                else
                {
                    ModToggles.enableBIGhost = false;
                    ModToggles.enableBIMissions = false;
                    ModToggles.enableBIPlayer = false;
                }
                Debug.Msg("Basic informations: Toggled " + (ModToggles.enableBI ? "On" : "Off"), 1);
            }

            if (keyboard.insertKey.wasPressedThisFrame || keyboard.deleteKey.wasPressedThisFrame || keyboard.rightCtrlKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
            {
                ModToggles.guiEnabled = !ModToggles.guiEnabled;
                Debug.Msg("GUI: Toggled " + (ModToggles.guiEnabled ? "On" : "Off"), 1);

                if (ModToggles.guiEnabled)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    if (myPlayer != null)
                        myPlayer.field_Public_FirstPersonController_0.enabled = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    if (myPlayer != null)
                        myPlayer.field_Public_FirstPersonController_0.enabled = true;
                }
            }
        }
        public override void OnGUI()
        {
            if (ModToggles.guiEnabled)
            {
                if (GUI.Toggle(new Rect(500f, 2f, 150f, 20f), ModToggles.guiHelper, "Helper GUI") != ModToggles.guiHelper)
                {
                    ModToggles.guiHelper = !ModToggles.guiHelper;
                    ModToggles.guiHelperInfo = false;
                    ModToggles.guiDebug = false;
                }
                if (ModToggles.guiHelper == true)
                {
                    if (GUI.Toggle(new Rect(650f, 2f, 150f, 20f), ModToggles.guiHelperInfo, "Basic Info") != ModToggles.guiHelperInfo)
                    {
                        ModToggles.guiHelperInfo = !ModToggles.guiHelperInfo;
                    }
                    if (ModToggles.guiHelperInfo == true)
                    {
                        if (GUI.Toggle(new Rect(800f, 2f, 150f, 20f), ModToggles.enableBIGhost, "Ghost Info") != ModToggles.enableBIGhost)
                        {
                            ModToggles.enableBIGhost = !ModToggles.enableBIGhost;
                            Debug.Msg("Ghost Info: Toggled " + (ModToggles.enableBIGhost ? "On" : "Off"), 1);
                        }
                        if (GUI.Toggle(new Rect(800f, 22f, 150f, 20f), ModToggles.enableBIMissions, "Missions Info") != ModToggles.enableBIMissions)
                        {
                            ModToggles.enableBIMissions = !ModToggles.enableBIMissions;
                            Debug.Msg("Missions Info: Toggled " + (ModToggles.enableBIMissions ? "On" : "Off"), 1);
                        }
                        if (GUI.Toggle(new Rect(800f, 42f, 150f, 20f), ModToggles.enableBIPlayer, "Player Info") != ModToggles.enableBIPlayer)
                        {
                            ModToggles.enableBIPlayer = !ModToggles.enableBIPlayer;
                            Debug.Msg("Player Info: Toggled " + (ModToggles.enableBIPlayer ? "On" : "Off"), 1);
                        }
                    }
                }
                if (GUI.Toggle(new Rect(500f, 22f, 150f, 20f), ModToggles.guiDebug, "Debug GUI") != ModToggles.guiDebug)
                {
                    ModToggles.guiDebug = !ModToggles.guiDebug;
                    ModToggles.guiHelper = false;
                    ModToggles.guiHelperInfo = false;
                }
                if (ModToggles.guiDebug == true)
                {
                    if (GUI.Toggle(new Rect(650f, 2f, 150f, 20f), ModToggles.enableDebug, "Enable Debug") != ModToggles.enableDebug)
                    {
                        ModToggles.enableDebug = !ModToggles.enableDebug;
                        Debug.Msg("Debug: Toggled " + (ModToggles.enableDebug ? "On" : "Off"), 1);
                    }
                    if (GUI.Toggle(new Rect(650f, 22f, 150f, 20f), Debug.debugMode1, "Debug Mode 1") != Debug.debugMode1)
                    {
                        Debug.debugMode1 = !Debug.debugMode1;
                        Debug.Msg("Debug Mode 1: Toggled " + (Debug.debugMode1 ? "On" : "Off"), 1);
                    }
                    if (GUI.Toggle(new Rect(650f, 42f, 150f, 20f), Debug.debugMode2, "Debug Mode 2") != Debug.debugMode2)
                    {
                        Debug.debugMode2 = !Debug.debugMode2;
                        Debug.Msg("Debug Mode 2: Toggled " + (Debug.debugMode2 ? "On" : "Off"), 1);
                    }
                    if (GUI.Toggle(new Rect(650f, 62f, 150f, 20f), Debug.debugMode3, "Debug Mode 3") != Debug.debugMode3)
                    {
                        Debug.debugMode3 = !Debug.debugMode3;
                        Debug.Msg("Debug Mode 3: Toggled " + (Debug.debugMode3 ? "On" : "Off"), 1);
                    }
                }
            }

            if (ModToggles.enableBIGhost || ModToggles.enableBIMissions || ModToggles.enableBIPlayer)
            {
                GUI.Label(new Rect(0f, 0f, 465f, 120f), "", "box");
            }
            if (ModToggles.enableBIGhost)
            {
                BasicInformations.EnableGhost();
                GUI.Label(new Rect(10f, 2f, 300f, 50f), "<color=#00FF00><b>Ghost Name:</b> " + (ghostNameAge ?? "") + "</color>");
            }
            else
            {
                if (initializedScene > 1)
                {
                    Debug.Msg("BasicInformations.DisableGhost", 3);
                    BasicInformations.DisableGhost();
                }
            }
            if (ModToggles.enableBIMissions)
            {
                BasicInformations.EnableMissions();
            }
            if (ModToggles.enableBIPlayer)
            {
                BasicInformations.EnablePlayer();
                GUI.Label(new Rect(10f, 17f, 300f, 50f), "<color=#00FF00><b>My Sanity:</b> " + (myPlayerSanity ?? "N/A") + "      Your ping: " + getPing + "</color>");
            }
        }

        private void DisableAll()
        {
            Debug.Msg("DisableAll", 3);
            ModToggles.enableBI = false;
            ModToggles.enableBIGhost = false;
            ModToggles.enableBIMissions = false;
            ModToggles.enableBIPlayer = false;

            BasicInformations.DisableGhost();
        }

        private Player GetLocalPlayer()
        {
            Debug.Msg("GetLocalPlayer", 3);
            if (players == null || players.Count == 0)
            {
                return null;
            }
            if (players.Count == 1)
            {
                return players[0];
            }
            foreach (Player player in players)
            {
                if (player != null && player.field_Public_PhotonView_0 != null && player.field_Public_PhotonView_0.AmOwner)
                {
                    return player;
                }
            }
            return null;
        }

        private void HandleConfig()
        {
            MelonPrefs.RegisterCategory("Settings", "Settings");
            Debug.Msg("Create Category: Settings", 2);

            MelonPrefs.RegisterBool("Settings", "HotkeysEnabled", true, "Hotkeys Enabled");
            Debug.Msg("Create Entry: HotkeysEnabled", 2);

            MelonPrefs.RegisterBool("Settings", "DebugEnabled", true, "Debug Enabled");
            Debug.Msg("Create Entry: DebugEnabled", 2);

            MelonPrefs.RegisterBool("Settings", "DebugM1Enabled", true, "Debug M1 Enabled");
            Debug.Msg("Create Entry: Debug1Enabled", 2);

            MelonPrefs.RegisterBool("Settings", "DebugM2Enabled", true, "Debug M2 Enabled");
            Debug.Msg("Create Entry: Debug2Enabled", 2);

            MelonPrefs.RegisterBool("Settings", "DebugM3Enabled", false, "Debug M3 Enabled");
            Debug.Msg("Create Entry: Debug3Enabled", 2);

            ModToggles.enableHotkeys = MelonPrefs.GetBool("Settings", "HotkeysEnabled");
            ModToggles.enableDebug = MelonPrefs.GetBool("Settings", "DebugEnabled");

            Debug.debugMode1 = MelonPrefs.GetBool("Settings", "DebugM1Enabled");
            Debug.debugMode2 = MelonPrefs.GetBool("Settings", "DebugM2Enabled");
            Debug.debugMode3 = MelonPrefs.GetBool("Settings", "DebugM3Enabled");

            MelonPrefs.SaveConfig();
        }

        IEnumerator CollectGameObjects()
        {
            try {
                Debug.Msg("isRunningTrue", 3);
                isRunning = true;
                Debug.Msg("cameraMain", 3);
                cameraMain = Camera.main ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("dnaEvidences", 3);
                dnaEvidences = Object.FindObjectsOfType<DNAEvidence>().ToList<DNAEvidence>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("doors", 3);
                doors = Object.FindObjectsOfType<Door>().ToList<Door>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("fuseBox", 3);
                fuseBox = Object.FindObjectOfType<FuseBox>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("gameController", 3);
                gameController = Object.FindObjectOfType<GameController>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("ghostAI", 3);
                ghostAI = Object.FindObjectOfType<GhostAI>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("ghostAIs", 3);
                ghostAIs = Object.FindObjectsOfType<GhostAI>().ToList<GhostAI>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("ghostActivity", 3);
                ghostActivity = Object.FindObjectOfType<GhostActivity>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("ghostInfo", 3);
                ghostInfo = Object.FindObjectOfType<GhostInfo>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("levelController", 3);
                levelController = Object.FindObjectOfType<LevelController>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("lightSwitch", 3);
                lightSwitch = Object.FindObjectOfType<LightSwitch>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("lightSwitches", 3);
                lightSwitches = Object.FindObjectsOfType<LightSwitch>().ToList<LightSwitch>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("soundController", 3);
                soundController = Object.FindObjectOfType<SoundController>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("ouijaBoards", 3);
                ouijaBoards = Object.FindObjectsOfType<OuijaBoard>().ToList<OuijaBoard>() ?? null;
                yield return new WaitForSeconds(0.15f);

                Debug.Msg("ouijaBoards", 3);
                windows = Object.FindObjectsOfType<Window>().ToList<Window>() ?? null;
                yield return new WaitForSeconds(0.15f);

                if (Object.FindObjectOfType<Player>() != null)
                {
                    Debug.Msg("player", 3);
                    player = Object.FindObjectOfType<Player>() ?? null;
                    yield return new WaitForSeconds(0.15f);

                    Debug.Msg("players", 3);
                    players = Object.FindObjectsOfType<Player>().ToList<Player>() ?? null;
                    yield return new WaitForSeconds(0.15f);

                    Debug.Msg("playerStatsManager", 3);
                    playerStatsManager = Object.FindObjectOfType<PlayerStatsManager>() ?? null;
                    yield return new WaitForSeconds(0.15f);

                    Debug.Msg("myPlayer", 3);
                    myPlayer = GetLocalPlayer() ?? player;
                    yield return new WaitForSeconds(0.15f);

                    Debug.Msg("playerAnim", 3);
                    playerAnim = myPlayer.field_Public_Animator_0 ?? null;
                    yield return new WaitForSeconds(0.15f);

                    if (playerAnim != null)
                    {
                        Debug.Msg("boneTransform", 3);
                        boneTransform = playerAnim.GetBoneTransform(HumanBodyBones.Head) ?? null;
                        yield return new WaitForSeconds(0.15f);
                    }
                }

                if (levelController != null)
                {
                    Debug.Msg("emf", 3);
                    emf = Object.FindObjectsOfType<EMF>().ToList<EMF>() ?? null;
                    yield return new WaitForSeconds(0.15f);
                }

                isRunning = false;
                yield return new WaitForSeconds(0.15f);
                Debug.Msg("-----------------------------", 3);

                yield return null;
            } finally {
                if(isRunning) {
                    Debug.Msg("Unexpected Error while collecting game objects.");
                    isRunning = false;
                }
            }
        }

        public static Transform boneTransform;
        public static Camera cameraMain;
        public static List<DNAEvidence> dnaEvidences;
        public static List<Door> doors;
        public static GameController gameController;
        public static GhostAI ghostAI;
        public static List<GhostAI> ghostAIs;
        public static List<EMF> emf;
        public static EMFData emfData;
        public static FuseBox fuseBox;
        public static GhostActivity ghostActivity;
        public static GhostInteraction ghostInteraction;
        public static GhostController ghostController;
        public static GhostEventPlayer ghostEventPlayer;
        public static GhostInfo ghostInfo;
        public static List<InventoryItem> items;
        public static LevelController levelController;
        public static Light light;
        public static LightSwitch lightSwitch;
        public static List<LightSwitch> lightSwitches;
        public static Player myPlayer;
        public static List<OuijaBoard> ouijaBoards;
        public static PhotonView photonView;
        public static Player player;
        public static List<Player> players;
        public static LobbyManager lobbyManager;
        public static Animator playerAnim;
        public static PlayerStatsManager playerStatsManager;
        public static ServerManager serverManager;
        public static SoundController soundController;
        public static List<Window> windows;
        public static String ghostNameAge;
        public static int getPing;
        public static String myPlayerSanity;
        public static String[] mapNames = { "Opening Scene", "Lobby", "Tanglewood Street", "Ridgeview Road House", "Edgefield Street House", "Asylum", "Brownstone High School", "Bleasdale Farmhouse", "Grafton Farmhouse", "Prison" };
        public static String inSight = "";
        public static bool settingsExist = false;
        public static int initializedScene;
        private static bool gameStarted = false;
        private static object coRoutine;
        private static bool canRun = true;
        private static bool isRunning = false;

        public static ChallengesManager challengesManager;
        public static int test = 0;
    }
}
