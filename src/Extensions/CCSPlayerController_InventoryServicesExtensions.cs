/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerController_InventoryServicesExtensions
{
    public static CCSPlayerController? GetController(this CCSPlayer_ItemServices itemServices)
    {
        var pawn = itemServices.Pawn;
        return
            pawn != null && pawn.IsValid && pawn.Controller.IsValid && pawn.Controller.Value != null
            ? pawn.Controller.Value.As<CCSPlayerController>()
            : null;
    }

    public static CCSPlayerInventory GetInventory(
        this CCSPlayerController_InventoryServices inventoryServices
    )
    {
        return new CCSPlayerInventory(
            inventoryServices.Address + Natives.CCSPlayerController_InventoryServices_m_pInventory
        );
    }
}
