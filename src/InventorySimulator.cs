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
    public override void Load(bool hotReload)
    {
        Runtime.Initialize();
        ConVars.Initialize();
        Core.Event.OnEntityCreated += OnEntityCreated;
        Core.Event.OnEntityDeleted += OnEntityDeleted;
        Core.Event.OnConVarValueChanged += OnConVarValueChanged;
        Core.GameEvent.HookPost<EventPlayerConnect>(OnPlayerConnect);
        Core.GameEvent.HookPost<EventPlayerConnectFull>(OnPlayerConnectFull);
        Core.GameEvent.HookPost<EventPlayerSpawn>(OnPlayerSpawn);
        Core.GameEvent.HookPre<EventPlayerDeath>(OnPlayerDeathPre);
        Core.GameEvent.HookPre<EventRoundMvp>(OnRoundMvpPre);
        Core.GameEvent.HookPost<EventPlayerDisconnect>(OnPlayerDisconnect);
        _giveNamedItemHookGuid = Natives.CCSPlayer_ItemServices_GiveNamedItem.AddHook(
            OnGiveNamedItem
        );
        _getItemInLoadoutHookGuid = Natives.CCSPlayerInventory_GetItemInLoadout.AddHook(
            OnGetItemInLoadout
        );
        OnFileChanged();
        OnIsRequireInventoryChanged(ConVars.IsRequireInventory.Value);
        OnIsSprayOnUseChanged(ConVars.IsSprayOnUse.Value);
    }

    private Guid _giveNamedItemHookGuid;
    private Guid _getItemInLoadoutHookGuid;
    private Guid? _activatePlayerHookGuid;
    private bool _isProcessUsercmdsHooked = false;

    public void OnFileChanged()
    {
        if (Inventories.Load())
            foreach (var player in Core.PlayerManager.GetAllPlayers())
                if (Inventories.TryGet(player.SteamID, out var inventory))
                    player.Controller.GetState().Inventory = inventory;
    }

    public void OnUrlChanged(string oldValue, string newValue)
    {
        Api.ResetSuspension();
        if (oldValue == newValue)
            return;
        var isOfficialHost =
            Uri.TryCreate(newValue, UriKind.Absolute, out var uri)
            && uri.Host.Equals("inventory.cstrike.app", StringComparison.OrdinalIgnoreCase);
        if (!isOfficialHost)
        {
            ConVars.IsPublicApiStatTrakIncrement.SetInternal(false);
            ConVars.IsPublicApiSprayConsume.SetInternal(false);
        }
    }

    public void OnIsRequireInventoryChanged(bool value)
    {
        if (value == (_activatePlayerHookGuid != null))
            return;
        if (value)
            _activatePlayerHookGuid = Natives.CServerSideClientBase_ActivatePlayer.AddHook(
                OnActivatePlayer
            );
        else
        {
            Natives.CServerSideClientBase_ActivatePlayer.RemoveHook(_activatePlayerHookGuid!.Value);
            _activatePlayerHookGuid = null;
        }
    }

    public void OnIsSprayOnUseChanged(bool value)
    {
        if (value == _isProcessUsercmdsHooked)
            return;
        if (value)
            Core.GameHooks.Controller.ProcessUsercmds.Pre += OnProcessUsercmdsPre;
        else
            Core.GameHooks.Controller.ProcessUsercmds.Pre -= OnProcessUsercmdsPre;
        _isProcessUsercmdsHooked = value;
    }

    public override void Unload()
    {
        Natives.CCSPlayer_ItemServices_GiveNamedItem.RemoveHook(_giveNamedItemHookGuid);
        Natives.CCSPlayerInventory_GetItemInLoadout.RemoveHook(_getItemInLoadoutHookGuid);
        OnIsRequireInventoryChanged(false);
        OnIsSprayOnUseChanged(false);
        CCSPlayerControllerState.ClearAllEconItemView();
    }
}
