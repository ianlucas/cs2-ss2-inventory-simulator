/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Commands;

namespace InventorySimulator;

public partial class InventorySimulator
{
    [Command(
        "ws",
        helpText: "Refreshes player inventory from the Inventory Simulator service and displays the configured URL."
    )]
    public void OnWSCommand(ICommandContext context)
    {
        var player = context.Sender;
        var prefix = InventorySimulatorCtx.GetChatPrefix();
        var url = UrlHelper.FormatUrl(ConVars.WsUrlPrintFormat.Value, ConVars.Url.Value);
        player?.SendChat(Core.Localizer["invsim.announce", prefix, url]);
        if (!ConVars.IsWsEnabled.Value || player == null)
            return;
        var controllerState = player.Controller.GetState();
        var cooldown = ConVars.WsCooldown.Value;
        var diff = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - controllerState.WsUpdatedAt;
        if (diff < cooldown)
        {
            player.SendChat(Core.Localizer["invsim.ws_cooldown", prefix, cooldown - diff]);
            return;
        }
        if (controllerState.IsFetching)
        {
            player.SendChat(Core.Localizer["invsim.ws_in_progress", prefix]);
            return;
        }
        player.RefreshInventory(force: true);
        player.SendChat(Core.Localizer["invsim.ws_new", prefix]);
    }

    [Command(
        "spray",
        helpText: "Applies the player's equipped graffiti spray at their current location."
    )]
    public void OnSprayCommand(ICommandContext context)
    {
        var player = context.Sender;
        if (player != null)
            player.TrySprayGraffiti();
    }

    [Command(
        "wslogin",
        helpText: "Authenticates the player with Inventory Simulator and displays their login URL."
    )]
    public void OnWsloginCommand(ICommandContext context)
    {
        var player = context.Sender;
        if (ConVars.IsWsLogin.Value && Api.HasApiKey() && player != null)
        {
            var controllerState = player.Controller.GetState();
            player.SendChat(
                Core.Localizer["invsim.login_in_progress", InventorySimulatorCtx.GetChatPrefix()]
            );
            if (controllerState.IsAuthenticating)
                return;
            player.SignIn();
        }
    }
}
