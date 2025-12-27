/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerPawnExtensions
{
    public static bool IsAbleToApplySpray(this CCSPlayerPawn self, IntPtr ptr = 0)
    {
        return Natives.CCSPlayerPawn_IsAbleToApplySpray.Call(self.Address, ptr, 0, 0) == nint.Zero;
    }

    public static void SetModelFromLoadout(this CCSPlayerPawn self)
    {
        Natives.CCSPlayerPawn_SetModelFromLoadout.Call(self.Address);
    }

    public static void SetModelFromClass(this CCSPlayerPawn self)
    {
        Natives.CCSPlayerPawn_SetModelFromClass.Call(self.Address);
    }
}
