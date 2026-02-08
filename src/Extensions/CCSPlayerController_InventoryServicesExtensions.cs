/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerController_InventoryServicesExtensions
{
    public static CCSPlayerInventory GetInventory(this CCSPlayerController_InventoryServices self)
    {
        return new CCSPlayerInventory(
            self.Address + Natives.CCSPlayerController_InventoryServices_m_pInventory
        );
    }
}
