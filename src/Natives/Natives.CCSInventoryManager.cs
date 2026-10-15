/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public static readonly nint CSInventoryManager = GetGameSystemByName("CCSInventoryManager");

    public delegate nint CCSInventoryManager_GetBaseItemForTeamDelegate(
        nint thisPtr,
        int iTeam,
        int iSlot
    );

    public static readonly IUnmanagedFunction<CCSInventoryManager_GetBaseItemForTeamDelegate> CCSInventoryManager_GetBaseItemForTeam =
        GetFunctionBySignature<CCSInventoryManager_GetBaseItemForTeamDelegate>(
            "CCSInventoryManager::GetBaseItemForTeam"
        );
}
