/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Plugins;

namespace InventorySimulator;

[PluginMetadata(
    Id = "InventorySimulator",
    Version = "1.0.0",
    Name = "InventorySimulator",
    Author = "Ian Lucas",
    Description = "Inventory Simulator (inventory.cstrike.app) plugin."
)]
public partial class InventorySimulator(ISwiftlyCore core) : BasePlugin(core)
{
    public Guid? OnActivatePlayerHookGuid;

    public override void Load(bool hotReload)
    {
        Swiftly.Initialize();
        ConVars.Initialize();
        Core.Event.OnEntityCreated += OnEntityCreated;
        Core.Event.OnEntityDeleted += OnEntityDeleted;
        Core.Event.OnConVarValueChanged += OnConVarValueChanged;
        Core.Event.OnClientProcessUsercmds += OnClientProcessUsercmds;
        Core.GameEvent.HookPost<EventPlayerConnect>(OnPlayerConnect);
        Core.GameEvent.HookPost<EventPlayerConnectFull>(OnPlayerConnectFull);
        Core.GameEvent.HookPre<EventPlayerDeath>(OnPlayerDeathPre);
        Core.GameEvent.HookPre<EventRoundMvp>(OnRoundMvpPre);
        Core.GameEvent.HookPost<EventPlayerDisconnect>(OnPlayerDisconnect);
        Natives.CCSPlayer_ItemServices_GiveNamedItem.AddHook(OnGiveNamedItem);
        Natives.CCSPlayerInventory_GetItemInLoadout.AddHook(OnGetItemInLoadout);
        OnFileChanged();
        OnIsRequireInventoryChanged();
    }

    public void OnFileChanged()
    {
        if (Inventories.Load())
            foreach (var player in Core.PlayerManager.GetAllPlayers())
                if (Inventories.TryGet(player.SteamID, out var inventory))
                    player.Controller.GetState().Inventory = inventory;
    }

    public void OnIsRequireInventoryChanged()
    {
        if (ConVars.IsRequireInventory.Value)
            OnActivatePlayerHookGuid = Natives.CServerSideClientBase_ActivatePlayer.AddHook(
                OnActivatePlayer
            );
        else if (OnActivatePlayerHookGuid != null)
            Natives.CServerSideClientBase_ActivatePlayer.RemoveHook(OnActivatePlayerHookGuid.Value);
    }

    public override void Unload()
    {
        CCSPlayerControllerState.ClearAllEconItemView();
    }
}
