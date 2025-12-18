/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CPlayerInventory_SendInventoryUpdateEventDelegate(nint thisPtr);

    private static readonly Lazy<
        IUnmanagedFunction<CPlayerInventory_SendInventoryUpdateEventDelegate>
    > _lazyPlayerInventorySendInventoryUpdateEvent = new(() =>
        FromSignature<CPlayerInventory_SendInventoryUpdateEventDelegate>(
            "CPlayerInventory::SendInventoryUpdateEvent"
        )
    );

    public static IUnmanagedFunction<CPlayerInventory_SendInventoryUpdateEventDelegate> CPlayerInventory_SendInventoryUpdateEvent =>
        _lazyPlayerInventorySendInventoryUpdateEvent.Value;
}
