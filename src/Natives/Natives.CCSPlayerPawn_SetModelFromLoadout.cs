/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerPawn_SetModelFromLoadoutDelegate(nint thisPtr);

    private static readonly Lazy<
        IUnmanagedFunction<CCSPlayerPawn_SetModelFromLoadoutDelegate>
    > _lazySetModelFromLoadout = new(() =>
        FromSignature<CCSPlayerPawn_SetModelFromLoadoutDelegate>(
            "CCSPlayerPawn::SetModelFromLoadout"
        )
    );

    public static IUnmanagedFunction<CCSPlayerPawn_SetModelFromLoadoutDelegate> CCSPlayerPawn_SetModelFromLoadout =>
        _lazySetModelFromLoadout.Value;
}
