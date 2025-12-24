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
    Description = "Inventory Simulator Lite (inventory.cstrike.app) plugin."
)]
public partial class InventorySimulator(ISwiftlyCore core) : BasePlugin(core)
{
    public override void Load(bool hotReload)
    {
        Natives.Initialize(Core);
        Api.Initialize(Core, Url, ApiKey);
        Core.Event.OnEntityCreated += OnEntityCreated;
        Core.Event.OnEntityDeleted += OnEntityDeleted;
        Core.Event.OnConVarValueChanged += OnConVarValueChanged;
        Core.GameEvent.HookPost<EventPlayerConnect>(OnPlayerConnect);
        Core.GameEvent.HookPost<EventPlayerConnectFull>(OnPlayerConnectFull);
        Core.GameEvent.HookPost<EventPlayerSpawn>(OnPlayerSpawn);
        Core.GameEvent.HookPre<EventPlayerDeath>(OnPlayerDeathPre);
        Core.GameEvent.HookPost<EventPlayerDisconnect>(OnPlayerDisconnect);
        Natives.CCSPlayer_ItemServices_GiveNamedItem.AddHook(OnGiveNamedItem);
        OnFileChanged();
        OnIsRequireInventoryChanged();
    }

    public override void Unload() { }
}
