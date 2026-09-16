/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerInventory_GetItemInLoadoutDelegate(
        nint thisPtr,
        int iTeam,
        int iSlot
    );

    public static readonly IUnmanagedFunction<CCSPlayerInventory_GetItemInLoadoutDelegate> CCSPlayerInventory_GetItemInLoadout =
        GetFunctionBySignature<CCSPlayerInventory_GetItemInLoadoutDelegate>(
            "CCSPlayerInventory::GetItemInLoadout"
        );

    public delegate nint CCSPlayerInventory_SendInventoryUpdateEventDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CCSPlayerInventory_SendInventoryUpdateEventDelegate> CCSPlayerInventory_SendInventoryUpdateEvent =
        GetFunctionBySignature<CCSPlayerInventory_SendInventoryUpdateEventDelegate>(
            "CCSPlayerInventory::SendInventoryUpdateEvent"
        );

    public static readonly int CCSPlayerInventory_m_pSOCache = GetOffset(
        "CCSPlayerInventory::m_pSOCache"
    );
}
