/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using SwiftlyS2.Shared.Natives;

namespace InventorySimulator;

public class CServerSideClientBase(nint address) : INativeHandle
{
    public nint Address { get; set; } = address;
    public bool IsValid => Address != nint.Zero;
    public ushort UserID =>
        (ushort)Marshal.ReadInt16(Address + Natives.CServerSideClientBase_m_UserID);
}
