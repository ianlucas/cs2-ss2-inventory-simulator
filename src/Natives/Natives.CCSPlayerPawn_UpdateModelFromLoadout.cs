/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerPawn_UpdateModelFromLoadoutDelegate(nint thisPtr);

    private static readonly Lazy<
        IUnmanagedFunction<CCSPlayerPawn_UpdateModelFromLoadoutDelegate>
    > _lazyPlayerPawnUpdateModelFromLoadout = new(() =>
        FromSignature<CCSPlayerPawn_UpdateModelFromLoadoutDelegate>(
            "CCSPlayerPawn::UpdateModelFromLoadout"
        )
    );

    public static IUnmanagedFunction<CCSPlayerPawn_UpdateModelFromLoadoutDelegate> CCSPlayerPawn_UpdateModelFromLoadout =>
        _lazyPlayerPawnUpdateModelFromLoadout.Value;
}
