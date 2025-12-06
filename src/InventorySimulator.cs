/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Convars;
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
    public readonly IConVar<bool> IsStatTrakIgnoreBots = core.ConVar.Create(
        "invsim_stattrak_ignore_bots",
        "Whether to ignore StatTrak increments for bot kills.",
        true
    );
    public readonly IConVar<bool> IsSpraychangerEnabled = core.ConVar.Create(
        "invsim_spraychanger_enabled",
        "Whether to change player vanilla spray if they have a graffiti equipped.",
        false
    );
    public readonly IConVar<bool> IsSprayEnabled = core.ConVar.Create(
        "invsim_spray_enabled",
        "Whether to enable spraying using !spray and/or use key.",
        true
    );
    public readonly IConVar<bool> IsSprayOnUse = core.ConVar.Create(
        "invsim_spray_on_use",
        "Whether to try to apply spray when player presses use.",
        false
    );
    public readonly IConVar<bool> IsWsEnabled = core.ConVar.Create(
        "invsim_ws_enabled",
        "Whether players can refresh their inventory using !ws.",
        false
    );
    public readonly IConVar<string> WsUrlPrintFormat = core.ConVar.Create(
        "invsim_ws_url_print_format",
        "URL print format using !ws.",
        "{host}"
    );
    public readonly IConVar<bool> IsWsGlovesFix = core.ConVar.Create(
        "invsim_ws_gloves_fix",
        "Whether to apply the glove change fix.",
        false
    );
    public readonly IConVar<bool> IsWsImmediately = core.ConVar.Create(
        "invsim_ws_immediately",
        "Whether to apply skin changes immediately.",
        false
    );
    public readonly IConVar<bool> IsFallbackTeam = core.ConVar.Create(
        "invsim_fallback_team",
        "Whether get skin from any team (first current team).",
        false
    );
    public readonly IConVar<bool> IsRequireInventory = core.ConVar.Create(
        "invsim_require_inventory",
        "Require the player's inventory to be fetched before allowing them to connect to the game.",
        false
    );
    public readonly IConVar<int> MinModels = core.ConVar.Create(
        "invsim_minmodels",
        "Allows agents or use specific models for each team.",
        0
    );
    public readonly IConVar<int> WsCooldown = core.ConVar.Create(
        "invsim_ws_cooldown",
        "Cooldown in seconds between player inventory refreshes.",
        30
    );
    public readonly IConVar<int> SprayCooldown = core.ConVar.Create(
        "invsim_spray_cooldown",
        "Cooldown in seconds between player sprays.",
        30
    );
    public readonly IConVar<string> ApiKey = core.ConVar.Create(
        "invsim_apikey",
        "Inventory Simulator API's key.",
        ""
    );
    public readonly IConVar<string> Url = core.ConVar.Create(
        "invsim_hostname",
        "Inventory Simulator API's url.",
        "inventory.cstrike.app"
    );
    public readonly IConVar<bool> IsWsLogin = core.ConVar.Create(
        "invsim_wslogin",
        "Not recommended, but allows authenticating into Inventory Simulator and printing login URL to the player.",
        false
    );
    public readonly IConVar<string> File = core.ConVar.Create(
        "invsim_file",
        "File to load when plugin is loaded.",
        "inventories.json"
    );

    public override void Load(bool hotReload) { }

    public override void Unload() { }
}
