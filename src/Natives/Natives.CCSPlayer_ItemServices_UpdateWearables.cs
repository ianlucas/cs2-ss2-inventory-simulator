/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate void CCSPlayer_ItemServices_UpdateWearablesDelegate(nint thisPtr);

    private static readonly Lazy<
        IUnmanagedFunction<CCSPlayer_ItemServices_UpdateWearablesDelegate>
    > _lazyItemServicesUpdateWearables = new(() =>
        FromSignature<CCSPlayer_ItemServices_UpdateWearablesDelegate>(
            "CCSPlayer_ItemServices::UpdateWearables"
        )
    );

    public static IUnmanagedFunction<CCSPlayer_ItemServices_UpdateWearablesDelegate> CCSPlayer_ItemServices_UpdateWearables =>
        _lazyItemServicesUpdateWearables.Value;
}
