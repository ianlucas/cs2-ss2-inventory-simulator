/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate void CCSPlayerPawn_SetModelFromClassDelegate(nint thisPtr);

    private static readonly Lazy<
        IUnmanagedFunction<CCSPlayerPawn_SetModelFromClassDelegate>
    > _lazyPlayerPawnSetModelFromClass = new(() =>
        FromSignature<CCSPlayerPawn_SetModelFromClassDelegate>("CCSPlayerPawn::SetModelFromClass")
    );

    public static IUnmanagedFunction<CCSPlayerPawn_SetModelFromClassDelegate> CCSPlayerPawn_SetModelFromClass =>
        _lazyPlayerPawnSetModelFromClass.Value;
}
