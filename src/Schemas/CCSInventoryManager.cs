/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public class CCSInventoryManager(nint address) : INativeHandle
{
    public nint Address { get; set; } = address;
    public bool IsValid => Address != nint.Zero;

    public void DangerouslySetAddress(nint address) => Address = address;

    public nint GetBaseItemForTeam(byte team, loadout_slot_t slot)
    {
        return Natives.CCSInventoryManager_GetBaseItemForTeam.Call(Address, team, (int)slot);
    }
}
