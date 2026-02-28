/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate void CCSPlayerPawn_SetModelFromClassDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CCSPlayerPawn_SetModelFromClassDelegate> CCSPlayerPawn_SetModelFromClass =
        GetFunctionBySignature<CCSPlayerPawn_SetModelFromClassDelegate>(
            "CCSPlayerPawn::SetModelFromClass"
        );
}
