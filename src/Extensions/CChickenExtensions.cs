/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CChickenExtensions
{
    extension(CChicken)
    {
        // Mirrors how the game spawns pets on round_start.
        public static CChicken? CreatePet(
            CCSPlayerController controller,
            Vector? position,
            QAngle? angles
        )
        {
            var inventory = controller.InventoryServices?.Inventory;
            if (inventory?.IsValid != true)
                return null;
            // InitPet doesn't check that the pet slot is equipped, the game does it before calling.
            var itemView = Runtime.Core.Memory.ToSchemaClass<CEconItemView>(
                inventory.GetItemInLoadout(0, loadout_slot_t.LOADOUT_SLOT_PET)
            );
            if (!itemView.IsValid || !itemView.Initialized)
                return null;
            var chicken = Runtime.Core.EntitySystem.CreateEntityByDesignerName<CChicken>("chicken");
            if (chicken == null)
                return null;
            if (Natives.CChicken_InitPet.Call(chicken.Address, controller.Address) == nint.Zero)
            {
                chicken.Despawn();
                return null;
            }
            controller.SetPetChicken(chicken);
            chicken.Teleport(position, angles, null);
            chicken.DispatchSpawn();
            return chicken;
        }
    }

    extension(CChicken self)
    {
        // The game stops pets from roaming shortly after freeze time ends.
        public bool CanRoam
        {
            get => Marshal.ReadByte(self.Address + Natives.CChicken_m_bCanRoam) != 0;
            set =>
                Marshal.WriteByte(
                    self.Address + Natives.CChicken_m_bCanRoam,
                    (byte)(value ? 1 : 0)
                );
        }

        public void ApplyPetStyle(InventoryItem item)
        {
            var skeletonInstance = self.GetSkeletonInstance();
            if (skeletonInstance == null)
                return;
            var materialGroup =
                item.Style > 0 ? new CUtlStringToken(item.Style.Value.ToString()) : default;
            if (skeletonInstance.MaterialGroup.HashCode == materialGroup.HashCode)
                return;
            skeletonInstance.MaterialGroup = materialGroup;
            skeletonInstance.MaterialGroupUpdated();
        }
    }
}
