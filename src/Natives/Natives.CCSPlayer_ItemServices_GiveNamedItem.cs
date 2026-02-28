/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayer_ItemServices_GiveNamedItemDelegate(
        nint thisPtr,
        nint pchName,
        int subType,
        nint itemDef,
        byte forceGive,
        nint position
    );

    public static readonly IUnmanagedFunction<CCSPlayer_ItemServices_GiveNamedItemDelegate> CCSPlayer_ItemServices_GiveNamedItem =
        GetFunctionBySignature<CCSPlayer_ItemServices_GiveNamedItemDelegate>(
            "CCSPlayer_ItemServices::GiveNamedItem"
        );
}
