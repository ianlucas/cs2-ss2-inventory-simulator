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

    public static readonly IUnmanagedFunction<CCSPlayerPawn_IsAbleToApplySprayDelegate> CCSPlayerPawn_IsAbleToApplySpray =
        GetFunctionBySignature<CCSPlayerPawn_IsAbleToApplySprayDelegate>(
            "CCSPlayerPawn::IsAbleToApplySpray"
        );

    public delegate void CCSPlayerPawn_SetModelFromClassDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CCSPlayerPawn_SetModelFromClassDelegate> CCSPlayerPawn_SetModelFromClass =
        GetFunctionBySignature<CCSPlayerPawn_SetModelFromClassDelegate>(
            "CCSPlayerPawn::SetModelFromClass"
        );

    public delegate nint CCSPlayerPawn_SetModelFromLoadoutDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CCSPlayerPawn_SetModelFromLoadoutDelegate> CCSPlayerPawn_SetModelFromLoadout =
        GetFunctionBySignature<CCSPlayerPawn_SetModelFromLoadoutDelegate>(
            "CCSPlayerPawn::SetModelFromLoadout"
        );
}
