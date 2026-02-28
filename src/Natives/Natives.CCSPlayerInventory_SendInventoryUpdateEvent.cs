/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerInventory_SendInventoryUpdateEventDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CCSPlayerInventory_SendInventoryUpdateEventDelegate> CCSPlayerInventory_SendInventoryUpdateEvent =
        GetFunctionBySignature<CCSPlayerInventory_SendInventoryUpdateEventDelegate>(
            "CCSPlayerInventory::SendInventoryUpdateEvent"
        );
}
