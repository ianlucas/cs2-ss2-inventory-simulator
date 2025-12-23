/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class IPlayerExtensions
{
    public static bool IsUseCmdBusy(this IPlayer player)
    {
        if (player.PlayerPawn?.IsBuyMenuOpen == true)
            return true;
        if (player.PlayerPawn?.IsDefusing == true)
            return true;
        var weapon = player.PlayerPawn?.WeaponServices?.ActiveWeapon.Value;
        if (weapon?.DesignerName != "weapon_c4")
            return false;
        var c4 = weapon.As<CC4>();
        return c4.IsPlantingViaUse;
    }
}
