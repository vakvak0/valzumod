// JotunnModStub
// a Valheim mod skeleton using Jötunn
// 
// File:    JotunnModStub.cs
// Project: JotunnModStub

using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;

namespace JotunnModStub
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class JotunnModStub : BaseUnityPlugin
    {
        public const string PluginGUID = "com.vakvak0.valzumod";
        public const string PluginName = "valzumod";
        public const string PluginVersion = "0.0.1";
        
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            On.FejdStartup.Awake += FejdStartup_Awake;
            Jotunn.Logger.LogInfo("ModStub has landed");
        }

        private void FejdStartup_Awake(On.FejdStartup.orig_Awake orig, FejdStartup self)
        {
            Jotunn.Logger.LogInfo("FejdStartup is going to awake");
            orig(self);
        }

        private void DisableStamina()
        {
            if (Player.m_localPlayer.m_runStaminaDrain == 0 && 
                Player.m_localPlayer.m_dodgeStaminaUsage == 0 && 
                Player.m_localPlayer.m_sneakStaminaDrain == 0 && 
                Player.m_localPlayer.m_jumpStaminaUsage == 0) { return; }

            Player.m_localPlayer.m_runStaminaDrain = 0;
            Player.m_localPlayer.m_dodgeStaminaUsage = 0;
            Player.m_localPlayer.m_sneakStaminaDrain = 0;
            Player.m_localPlayer.m_jumpStaminaUsage = 0;
            Player.m_localPlayer.m_maxCarryWeight = 600; 
        }

        private void TeleportOres()
        {
            foreach (ItemDrop.ItemData itemData in Player.m_localPlayer.m_inventory.GetAllItems())
            {
                itemData.m_shared.m_teleportable = true;
            }
        }

        private void MaxStackSize()
        {
            foreach (ItemDrop.ItemData itemData in Player.m_localPlayer.m_inventory.m_inventory)
            {
                if (itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Material ||
                    itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable ||
                    itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo ||
                    itemData.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility)
                {
                    itemData.m_shared.m_maxStackSize = 999;
                }

            }
        }
        private void CheckInventoryChanged()
        {
            int m_oldItemsInInv = 0;
            int m_itemsInInv = Player.m_localPlayer.m_inventory.NrOfItemsIncludingStacks();
            if (m_itemsInInv == m_oldItemsInInv) { return; }
            MaxStackSize();
            
        }

        private void Update()
        {
            if (Player.m_localPlayer)
            {
                DisableStamina();
                if (Player.m_localPlayer.m_inventory.IsTeleportable() == false) { TeleportOres(); }
                CheckInventoryChanged();
            }
        }
    }
}

