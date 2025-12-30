/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Convars;

namespace InventorySimulator;

public static class ConVars
{
    public static IConVar<bool> IsStatTrakIgnoreBots { get; private set; } = null!;
    public static IConVar<bool> IsSprayChangerEnabled { get; private set; } = null!;
    public static IConVar<bool> IsSprayEnabled { get; private set; } = null!;
    public static IConVar<bool> IsSprayOnUse { get; private set; } = null!;
    public static IConVar<bool> IsWsEnabled { get; private set; } = null!;
    public static IConVar<string> WsUrlPrintFormat { get; private set; } = null!;
    public static IConVar<bool> IsWsImmediately { get; private set; } = null!;
    public static IConVar<bool> IsFallbackTeam { get; private set; } = null!;
    public static IConVar<bool> IsRequireInventory { get; private set; } = null!;
    public static IConVar<int> MinModels { get; private set; } = null!;
    public static IConVar<int> WsCooldown { get; private set; } = null!;
    public static IConVar<int> SprayCooldown { get; private set; } = null!;
    public static IConVar<string> ApiKey { get; private set; } = null!;
    public static IConVar<string> Url { get; private set; } = null!;
    public static IConVar<bool> IsWsLogin { get; private set; } = null!;
    public static IConVar<string> File { get; private set; } = null!;

    public static void Initialize(ISwiftlyCore core)
    {
        IsStatTrakIgnoreBots = core.ConVar.Create(
            "invsim_stattrak_ignore_bots",
            "Ignore StatTrak kill count increments for bot kills.",
            true
        );

        IsSprayChangerEnabled = core.ConVar.Create(
            "invsim_spraychanger_enabled",
            "Replace the player's vanilla spray with their equipped graffiti.",
            false
        );

        IsSprayEnabled = core.ConVar.Create(
            "invsim_spray_enabled",
            "Enable spraying via the !spray command and/or use key.",
            true
        );

        IsSprayOnUse = core.ConVar.Create(
            "invsim_spray_on_use",
            "Apply spray when the player presses the use key.",
            false
        );

        IsWsEnabled = core.ConVar.Create(
            "invsim_ws_enabled",
            "Allow players to refresh their inventory using the !ws command.",
            false
        );

        WsUrlPrintFormat = core.ConVar.Create(
            "invsim_ws_url_print_format",
            "URL format string displayed when using the !ws command.",
            "{Host}"
        );

        IsWsImmediately = core.ConVar.Create(
            "invsim_ws_immediately",
            "Apply skin changes immediately without requiring a respawn.",
            false
        );

        IsFallbackTeam = core.ConVar.Create(
            "invsim_fallback_team",
            "Allow using skins from any team (prioritizes current team first).",
            false
        );

        IsRequireInventory = core.ConVar.Create(
            "invsim_require_inventory",
            "Require the player's inventory to be fetched before allowing them to join the game.",
            false
        );

        MinModels = core.ConVar.Create(
            "invsim_minmodels",
            "Enable player agents (0 = enabled, 1 = use map models per team, 2 = SAS & Phoenix).",
            0
        );

        WsCooldown = core.ConVar.Create(
            "invsim_ws_cooldown",
            "Cooldown duration in seconds between inventory refreshes per player.",
            30
        );

        SprayCooldown = core.ConVar.Create(
            "invsim_spray_cooldown",
            "Cooldown duration in seconds between sprays per player.",
            30
        );

        ApiKey = core.ConVar.Create(
            "invsim_apikey",
            "API key for the Inventory Simulator service.",
            ""
        );

        Url = core.ConVar.Create(
            "invsim_url",
            "API URL for the Inventory Simulator service.",
            "https://inventory.cstrike.app"
        );

        IsWsLogin = core.ConVar.Create(
            "invsim_wslogin",
            "Allow players to authenticate with Inventory Simulator and display their login URL (not recommended).",
            false
        );

        File = core.ConVar.Create(
            "invsim_file",
            "Inventory data file to load when the plugin starts.",
            "inventories.json"
        );
    }
}
