/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public partial class InventorySimulator
{
    public void OnConVarValueChanged(IOnConVarValueChanged @event)
    {
        switch (@event.ConVarName)
        {
            case "invsim_file":
                OnFileChanged();
                return;
            case "invsim_require_inventory":
                OnIsRequireInventoryChanged();
                return;
        }
    }

    public void OnEntityCreated(IOnEntityCreatedEvent @event)
    {
        var entity = @event.Entity;
        var designerName = entity.DesignerName;
        if (designerName.Contains("weapon"))
        {
            Core.Scheduler.NextTick(() =>
            {
                var weapon = entity.As<CBasePlayerWeapon>();
                if (!weapon.IsValid || weapon.OriginalOwnerXuidLow == 0)
                    return;
                var player = Core.PlayerManager.GetPlayerFromSteamID(weapon.OriginalOwnerXuidLow);
                if (player == null || player.IsFakeClient || !player.IsValid)
                    return;
                var isMelee = ItemHelper.IsMeleeDesignerName(designerName);
                GivePlayerWeaponSkin(player.Controller, weapon, isMelee);
            });
        }
    }

    public void OnEntityDeleted(IOnEntityDeletedEvent @event)
    {
        var entity = @event.Entity;
        var designerName = entity.DesignerName;
        if (designerName == "cs_player_controller")
        {
            var controller = entity.As<CCSPlayerController>();
            if (controller.SteamID != 0)
                ClearPlayerControllerSteamID(controller);
        }
    }
}
