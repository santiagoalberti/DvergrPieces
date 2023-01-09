using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using PieceManager;
using ServerSync;

namespace DvergrPieces
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class DvergrPiecesPlugin : BaseUnityPlugin
    {
        internal const string ModName = "DvergrPieces";
        internal const string ModVersion = "1.0.2";
        internal const string Author = "Tequila";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource DvergrPiecesLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
                "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            // Globally turn off configuration options for your pieces, omit if you don't want to do this.
            BuildPiece.ConfigurationEnabled = false;


            BuildPiece teq_DvergrChest = new("dvergr_pieces_bundle_tq", "piece_chest_dvergr_tq");
            //teq_DvergrChest.Name.English("Dvergr Chest"); // Localize the name and description for the building piece for a language.
            //teq_DvergrChest.Description.English("Dvergr Silver Chest");
            teq_DvergrChest.RequiredItems.Add("ElderBark", 10, true); // Set the required items to build. Format: ("PrefabName", Amount, Recoverable)
            teq_DvergrChest.RequiredItems.Add("Silver", 3, true);
            teq_DvergrChest.Category.Add(PieceManager.BuildPieceCategory.Furniture);
            teq_DvergrChest.Crafting.Set(PieceManager.CraftingTable.Workbench);

            BuildPiece teq_dvergrBlackCoreStandTorch = new("dvergr_pieces_bundle_tq", "blackCoreTorch_tq");
            //teq_dvergrBlackCoreStandTorch.Name.English("Black Core Stand Torch"); // Localize the name and description for the building piece for a language.
            //teq_dvergrBlackCoreStandTorch.Description.English("Black Core Stand Torch");
            teq_dvergrBlackCoreStandTorch.RequiredItems.Add("FineWood", 10, true); // Set the required items to build. Format: ("PrefabName", Amount, Recoverable)
            teq_dvergrBlackCoreStandTorch.RequiredItems.Add("SurtlingCore", 1, true);
            teq_dvergrBlackCoreStandTorch.RequiredItems.Add("Iron", 1, true);
            teq_dvergrBlackCoreStandTorch.Category.Add(PieceManager.BuildPieceCategory.Furniture);
            teq_dvergrBlackCoreStandTorch.Crafting.Set(PieceManager.CraftingTable.Workbench); // Set a crafting station requirement for the piece.

            //BuildPiece teq_dvergrFermenter = new("dvergr_pieces_bundle_tq", "dvergr_fermenter_tq");
            //teq_dvergrFermenter.Name.English("Dvergr Fermenter"); // Localize the name and description for the building piece for a language.
            //teq_dvergrFermenter.Description.English("Dvergr Fermenter");
            //teq_dvergrFermenter.RequiredItems.Add("ElderBark", 20, true); // Set the required items to build. Format: ("PrefabName", Amount, Recoverable)
            //teq_dvergrFermenter.RequiredItems.Add("Iron", 5, true);
            //teq_dvergrFermenter.RequiredItems.Add("Resin", 10, true);
            //teq_dvergrFermenter.Category.Add(PieceManager.BuildPieceCategory.Crafting);
            //teq_dvergrFermenter.Crafting.Set(PieceManager.CraftingTable.Forge); // Set a crafting station requirement for the piece.

            // Format: new("AssetBundleName", "PrefabName", "FolderName");
            //BuildPiece examplePiece1 = new("funward", "funward", "FunWard");
            //examplePiece1.Name.English("Fun Ward"); // Localize the name and description for the building piece for a language.
            //examplePiece1.Description.English("Ward For testing the Piece Manager");
            //examplePiece1.RequiredItems.Add("FineWood", 20, false); // Set the required items to build. Format: ("PrefabName", Amount, Recoverable)
            //examplePiece1.RequiredItems.Add("SurtlingCore", 20, false);
            //examplePiece1.Category.Add(PieceManager.BuildPieceCategory.Misc);
            //examplePiece1.Crafting.Set(PieceManager.CraftingTable.ArtisanTable); // Set a crafting station requirement for the piece.
            //examplePiece1.Extension.Set(CraftingTable.Forge, 2); // Makes this piece a station extension, can change the max station distance by changing the second value. Use strings for custom tables.

            // Or you can do it for a custom table (### Default maxStationDistance is 5. I used 2 as an example here.)
            //examplePiece1.Extension.Set("MYCUSTOMTABLE", 2); // Makes this piece a station extension, can change the max station distance by changing the second value. Use strings for custom tables.

            //examplePiece1.Crafting.Set("CUSTOMTABLE"); // If you have a custom table you're adding to the game. Just set it like this.

            //examplePiece1.SpecialProperties.NoConfig = true;  // Do not generate a config for this piece, omit this line of code if you want to generate a config.
            //examplePiece1.SpecialProperties = new SpecialProperties() { AdminOnly = true, NoConfig = true}; // You can declare multiple properties in one line           


            //BuildPiece examplePiece2 = new("bamboo", "Bamboo_Wall"); // Note: If you wish to use the default "assets" folder for your assets, you can omit it!
            //examplePiece2.Name.English("Bamboo Wall");
            //examplePiece2.Description.English("A wall made of bamboo!");
            //examplePiece2.RequiredItems.Add("BambooLog", 20, false);
            //examplePiece2.Category.Add(PieceManager.BuildPieceCategory.Building);
            //examplePiece2.Crafting.Set("CUSTOMTABLE"); // If you have a custom table you're adding to the game. Just set it like this.
            //examplePiece2.SpecialProperties.AdminOnly = true;  // You can declare these one at a time as well!.


            // If you want to add your item to the cultivator or another hammer with vanilla categories
            // Format: (AssetBundle, "PrefabName", addToCustom, "Item that has a piecetable")
            //BuildPiece examplePiece3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "Bamboo_Sapling", true, "Cultivator");
            //examplePiece3.Name.English("Bamboo Sapling");
            //examplePiece3.Description.English("A young bamboo tree, called a sapling");
            //examplePiece3.RequiredItems.Add("BambooSeed", 20, false);
            //examplePiece3.SpecialProperties.NoConfig = true;

            // Need to add something to ZNetScene but not the hammer, cultivator or other? 
            //PiecePrefabManager.RegisterPrefab("bamboo", "Bamboo_Beam_Light");

            // Does your model need to swap materials with a vanilla material? Format: (GameObject, isJotunnMock)
            //MaterialReplacer.RegisterGameObjectForMatSwap(examplePiece3.Prefab, false);

            // Does your model use a shader from the game like Custom/Creature or Custom/Piece in unity? Need it to "just work"?
            //MaterialReplacer.RegisterGameObjectForShaderSwap(examplePiece3.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            // What if you want to use a custom shader from the game (like Custom/Piece that allows snow!!!) but your unity shader isn't set to Custom/Piece? Format: (GameObject, MaterialReplacer.ShaderType.)
            //MaterialReplacer.RegisterGameObjectForShaderSwap(examplePiece3.Prefab, MaterialReplacer.ShaderType.PieceShader);
            MaterialReplacer.RegisterGameObjectForShaderSwap(teq_DvergrChest.Prefab, MaterialReplacer.ShaderType.UseUnityShader);
            //MaterialReplacer.RegisterGameObjectForShaderSwap(teq_dvergrFermenter.Prefab, MaterialReplacer.ShaderType.UseUnityShader);
            MaterialReplacer.RegisterGameObjectForShaderSwap(teq_dvergrBlackCoreStandTorch.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            // Detailed instructions on how to use the MaterialReplacer can be found on the current PieceManager Wiki. https://github.com/AzumattDev/PieceManager/wiki

            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_fermenter_add_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_fermenter_tap_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_Place_wood_pole_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_HitSparks_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_Place_chest_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_SawDust_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_RockDestroyed_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_Place_smelter_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_MeadSplash_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_lootspawn_tq", false);
            PiecePrefabManager.RegisterPrefab(PiecePrefabManager.RegisterAssetBundle("dvergr_pieces_bundle_tq"), "vfx_Place_forge_tq", false);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                DvergrPiecesLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                DvergrPiecesLogger.LogError($"There was an issue loading your {ConfigFileName}");
                DvergrPiecesLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }

        #endregion
    }
}