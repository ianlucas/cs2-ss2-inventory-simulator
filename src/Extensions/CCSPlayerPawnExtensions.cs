/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerPawnExtensions
{
    extension(CCSPlayerPawn self)
    {
        public bool IsAbleToApplySpray(IntPtr ptr = 0)
        {
            return Natives.CCSPlayerPawn_IsAbleToApplySpray.Call(self.Address, ptr, 0, 0)
                == nint.Zero;
        }

        public void SetModelFromLoadout()
        {
            Natives.CCSPlayerPawn_SetModelFromLoadout.Call(self.Address);
        }

        public void SetModelFromClass()
        {
            Natives.CCSPlayerPawn_SetModelFromClass.Call(self.Address);
        }

        public void RefreshGloves(bool hasGloves)
        {
            self.AcceptInput("SetBodygroup", "first_or_third_person,0");
            Runtime.Core.Scheduler.NextWorldUpdate(() =>
            {
                if (!self.IsValid)
                    return;
                self.ItemServices?.UpdateWearables();
                if (!hasGloves)
                    return;
                self.EconGlovesChanged++;
                self.EconGlovesChangedUpdated();
            });
        }
    }
}
