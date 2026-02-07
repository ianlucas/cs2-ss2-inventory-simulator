/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerPawn_IsAbleToApplySprayDelegate(
        nint thisPtr,
        nint traceResultOut,
        nint sprayPosOut,
        nint eyePosOut
    );

    private static readonly Lazy<
        IUnmanagedFunction<CCSPlayerPawn_IsAbleToApplySprayDelegate>
    > _lazyIsAbleToApplySpray = new(() =>
        GetFunctionBySignature<CCSPlayerPawn_IsAbleToApplySprayDelegate>(
            "CCSPlayerPawn::IsAbleToApplySpray"
        )
    );

    public static IUnmanagedFunction<CCSPlayerPawn_IsAbleToApplySprayDelegate> CCSPlayerPawn_IsAbleToApplySpray =>
        _lazyIsAbleToApplySpray.Value;
}
