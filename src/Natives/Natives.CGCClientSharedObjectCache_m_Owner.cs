/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

public static partial class Natives
{
    public static int CGCClientSharedObjectCache_m_Owner =>
        new Lazy<int>(() => FromOffset("CGCClientSharedObjectCache::m_Owner")).Value;
}
