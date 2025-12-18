/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

public static partial class Natives
{
    public static int CCSPlayerInventory_m_pSOCache =>
        new Lazy<int>(() => FromOffset("CCSPlayerInventory::m_pSOCache")).Value;
}
