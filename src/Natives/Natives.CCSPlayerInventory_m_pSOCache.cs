/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

public static partial class Natives
{
    public static readonly int CCSPlayerInventory_m_pSOCache =
        GetOffset("CCSPlayerInventory::m_pSOCache");
}
