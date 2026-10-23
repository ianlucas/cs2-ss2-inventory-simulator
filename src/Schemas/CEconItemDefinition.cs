/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

// Thanks @Kxnrl.
public class CEconItemDefinition(nint address) : INativeHandle
{
    public nint Address { get; set; } = address;
    public bool IsValid => Address != nint.Zero;

    public void DangerouslySetAddress(nint address) => Address = address;

    // 1. Search for the "Prohibiting %s\n" string, its only xref is the mp_items_prohibited
    //    callback.
    // 2. For each item in the ConVar, it compares the definition's index (a word at 0x10) and
    //    prints its name (a pointer at 0x260) with "Prohibiting %s\n".
    public ushort DefIndex => (ushort)Marshal.ReadInt16(Address + 0x10);

    public string? DefinitionName
    {
        get
        {
            var ptr = Marshal.ReadIntPtr(Address + 0x260);
            return ptr != nint.Zero ? Marshal.PtrToStringUTF8(ptr) : null;
        }
    }

    // 1. Search for the "Player.Respawn" string, its only xref is in the pawn's spawn function.
    // 2. It calls SetModelFromLoadout and then SetModelFromClass, back to back, with only the pawn.
    // 3. SetModelFromLoadout checks the agent's definition with cmp dword ptr [def + offset], 26h
    //    (LOADOUT_SLOT_CLOTHING_CUSTOMPLAYER).
    public loadout_slot_t DefaultLoadoutSlot => (loadout_slot_t)Marshal.ReadInt32(Address + 0x338);
}
