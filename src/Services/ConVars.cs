/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Convars;

namespace InventorySimulator;

public static class ConVars
{
    public static readonly IConVar<string> Url = Runtime.Core.ConVar.CreateOrFind(
        "invsim_url",
        "API URL for the Inventory Simulator service.",
        "https://inventory.cstrike.app"
    );

    public static readonly IConVar<string> ApiKey = Runtime.Core.ConVar.CreateOrFind(
        "invsim_apikey",
        "API key for the Inventory Simulator service.",
        ""
    );

    public static readonly IConVar<string> File = Runtime.Core.ConVar.CreateOrFind(
        "invsim_file",
        "Inventory data file to load when the plugin starts.",
        "inventories.json"
    );

    public static readonly IConVar<bool> IsWsEnabled = Runtime.Core.ConVar.CreateOrFind(
        "invsim_ws_enabled",
        "Allow players to refresh their inventory using the !ws command.",
        false
    );

    public static readonly IConVar<bool> IsWsImmediately = Runtime.Core.ConVar.CreateOrFind(
        "invsim_ws_immediately",
        "Apply skin changes immediately without requiring a respawn.",
        false
    );

    public static readonly IConVar<int> WsCooldown = Runtime.Core.ConVar.CreateOrFind(
        "invsim_ws_cooldown",
        "Cooldown duration in seconds between inventory refreshes per player.",
        30
    );

    public static readonly IConVar<string> ChatPrefix = Runtime.Core.ConVar.CreateOrFind(
        "invsim_chat_prefix",
        "Prefix displayed before chat messages.",
        ""
    );

    public static readonly IConVar<string> WsUrlPrintFormat = Runtime.Core.ConVar.CreateOrFind(
        "invsim_ws_url_print_format",
        "URL format string displayed when using the !ws command.",
        "{Host}"
    );

    public static readonly IConVar<bool> IsWsLogin = Runtime.Core.ConVar.CreateOrFind(
        "invsim_wslogin",
        "Allow players to authenticate with Inventory Simulator and display their login URL (not recommended).",
        false
    );

    public static readonly IConVar<bool> IsPersistInventory = Runtime.Core.ConVar.CreateOrFind(
        "invsim_persist_inventory",
        "Keep a player's cached inventory after they disconnect.",
        false
    );

    public static readonly IConVar<bool> IsRequireInventory = Runtime.Core.ConVar.CreateOrFind(
        "invsim_require_inventory",
        "Require the player's inventory to be fetched before allowing them to join the game.",
        false
    );

    public static readonly IConVar<bool> IsSprayEnabled = Runtime.Core.ConVar.CreateOrFind(
        "invsim_spray_enabled",
        "Enable spraying via the !spray command and/or use key.",
        true
    );

    public static readonly IConVar<bool> IsSprayOnUse = Runtime.Core.ConVar.CreateOrFind(
        "invsim_spray_on_use",
        "Apply spray when the player presses the use key.",
        false
    );

    public static readonly IConVar<int> SprayCooldown = Runtime.Core.ConVar.CreateOrFind(
        "invsim_spray_cooldown",
        "Cooldown duration in seconds between sprays per player.",
        30
    );

    public static readonly IConVar<bool> IsSprayChangerEnabled = Runtime.Core.ConVar.CreateOrFind(
        "invsim_spraychanger_enabled",
        "Replace the player's vanilla spray with their equipped graffiti.",
        false
    );

    public static readonly IConVar<bool> IsPublicApiStatTrakIncrement =
        Runtime.Core.ConVar.CreateOrFind(
            "invsim_public_api_stattrak_increment",
            "Send keyless StatTrak increment requests to the public API when invsim_apikey is not set.",
            true
        );

    public static readonly IConVar<bool> IsPublicApiSprayConsume = Runtime.Core.ConVar.CreateOrFind(
        "invsim_public_api_spray_consume",
        "Send keyless graffiti consume requests to the public API when invsim_apikey is not set.",
        true
    );

    public static readonly IConVar<bool> IsStatTrakIgnoreBots = Runtime.Core.ConVar.CreateOrFind(
        "invsim_stattrak_ignore_bots",
        "Ignore StatTrak kill count increments for bot kills.",
        true
    );

    public static readonly IConVar<bool> IsFallbackTeam = Runtime.Core.ConVar.CreateOrFind(
        "invsim_fallback_team",
        "Allow using skins from any team (prioritizes current team first).",
        false
    );

    public static readonly IConVar<int> MinModels = Runtime.Core.ConVar.CreateOrFind(
        "invsim_minmodels",
        "Enable player agents (0 = enabled, 1 = use map models per team, 2 = SAS & Phoenix).",
        0
    );

    public static void Initialize()
    {
        _ = Url;
        _ = ApiKey;
        _ = File;
        _ = IsWsEnabled;
        _ = IsWsImmediately;
        _ = WsCooldown;
        _ = WsUrlPrintFormat;
        _ = IsWsLogin;
        _ = IsPersistInventory;
        _ = IsRequireInventory;
        _ = IsSprayEnabled;
        _ = IsSprayOnUse;
        _ = SprayCooldown;
        _ = IsSprayChangerEnabled;
        _ = IsPublicApiStatTrakIncrement;
        _ = IsPublicApiSprayConsume;
        _ = IsStatTrakIgnoreBots;
        _ = IsFallbackTeam;
        _ = MinModels;
    }
}
