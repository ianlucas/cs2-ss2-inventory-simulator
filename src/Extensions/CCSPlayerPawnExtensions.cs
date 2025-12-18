/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerPawnExtensions
{
    public static bool IsAbleToApplySpray(this CCSPlayerPawn pawn, IntPtr ptr = 0)
    {
        return Natives.CCSPlayerPawn_IsAbleToApplySpray.Call(pawn.Address, ptr, 0, 0) == nint.Zero;
    }

    public static void UpdateModelFromLoadout(this CCSPlayerPawn pawn)
    {
        Natives.CCSPlayerPawn_UpdateModelFromLoadout.Call(pawn.Address);
    }

    public static void SetModelFromClass(this CCSPlayerPawn pawn)
    {
        Natives.CCSPlayerPawn_SetModelFromClass.Call(pawn.Address);
    }
}
