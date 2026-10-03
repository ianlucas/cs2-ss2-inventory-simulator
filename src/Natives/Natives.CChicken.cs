/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate byte CChicken_InitPetDelegate(nint thisPtr, nint controller);

    public static readonly IUnmanagedFunction<CChicken_InitPetDelegate> CChicken_InitPet =
        GetFunctionBySignature<CChicken_InitPetDelegate>("CChicken::InitPet");

    public static readonly int CChicken_m_bCanRoam = GetOffset("CChicken::m_bCanRoam");
}
