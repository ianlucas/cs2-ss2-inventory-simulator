/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerController_InventoryServicesExtensions
{
    extension(CCSPlayerController_InventoryServices self)
    {
        public CCSPlayerInventory GetInventory()
        {
            return new CCSPlayerInventory(
                self.Address + Natives.CCSPlayerController_InventoryServices_m_pInventory
            );
        }
    }
}
