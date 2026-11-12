/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

// Thanks to @samyycX.
public class CCSPlayerInventory(nint address) : INativeHandle
{
    public nint Address { get; set; } = address;
    public bool IsValid => Address != nint.Zero && SOCache.IsValid;

    public void DangerouslySetAddress(nint address) => Address = address;

    public ulong SteamID => SOCache.Owner.SteamID;

    public CGCClientSharedObjectCache SOCache =>
        new(Marshal.ReadIntPtr(Address + Natives.CCSPlayerInventory_m_pSOCache));

    public nint GetItemInLoadout(byte team, loadout_slot_t slot)
    {
        return Natives.CCSPlayerInventory_GetItemInLoadout.Call(Address, team, (int)slot);
    }

    // Only the player's own items, the game's stock weapons have no item ID.
    public nint FindWeaponInLoadout(byte team, ushort def)
    {
        for (
            var slot = loadout_slot_t.LOADOUT_SLOT_FIRST_WHEEL_WEAPON;
            slot <= loadout_slot_t.LOADOUT_SLOT_LAST_WHEEL_WEAPON;
            slot++
        )
        {
            // Unhooked, so it reads the player's game loadout.
            var itemView = Runtime.Core.Memory.ToSchemaClass<CEconItemView>(
                Natives.CCSPlayerInventory_GetItemInLoadout.CallOriginal(Address, team, (int)slot)
            );
            if (itemView.IsValid && itemView.ItemID != 0 && itemView.ItemDefinitionIndex == def)
                return itemView.Address;
        }
        return nint.Zero;
    }

    public void SendInventoryUpdateEvent()
    {
        Natives.CCSPlayerInventory_SendInventoryUpdateEvent.Call(Address);
    }
}
