/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CInventoryManager_GetReferenceEconItemDelegate(
        nint thisPtr,
        ushort unDefIndex,
        int nPaintIndex,
        byte ub1
    );

    public static readonly IUnmanagedFunction<CInventoryManager_GetReferenceEconItemDelegate> CInventoryManager_GetReferenceEconItem =
        GetFunctionBySignature<CInventoryManager_GetReferenceEconItemDelegate>(
            "CInventoryManager::GetReferenceEconItem"
        );
}
