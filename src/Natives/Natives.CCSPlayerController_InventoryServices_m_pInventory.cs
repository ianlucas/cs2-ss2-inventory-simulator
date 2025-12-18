/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

public static partial class Natives
{
    public static int CCSPlayerController_InventoryServices_m_pInventory =>
        new Lazy<int>(() =>
            FromOffset("CCSPlayerController_InventoryServices::m_pInventory")
        ).Value;
}
