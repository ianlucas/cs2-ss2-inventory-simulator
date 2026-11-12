/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Runtime.CompilerServices;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerController_InventoryServicesExtensions
{
    extension(CCSPlayerController_InventoryServices self)
    {
        public unsafe CCSPlayerInventory Inventory =>
            new(
                (nint)Unsafe.AsPointer(ref self.PersonaDataXpTrailLevel)
                    + Natives.CCSPlayerController_InventoryServices_m_pInventory
            );
    }
}
