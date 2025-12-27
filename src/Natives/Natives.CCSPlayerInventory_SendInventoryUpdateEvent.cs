/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerInventory_SendInventoryUpdateEventDelegate(nint thisPtr);

    private static readonly Lazy<
        IUnmanagedFunction<CCSPlayerInventory_SendInventoryUpdateEventDelegate>
    > _lazyPlayerInventorySendInventoryUpdateEvent = new(() =>
        FromSignature<CCSPlayerInventory_SendInventoryUpdateEventDelegate>(
            "CCSPlayerInventory::SendInventoryUpdateEvent"
        )
    );

    public static IUnmanagedFunction<CCSPlayerInventory_SendInventoryUpdateEventDelegate> CCSPlayerInventory_SendInventoryUpdateEvent =>
        _lazyPlayerInventorySendInventoryUpdateEvent.Value;
}
