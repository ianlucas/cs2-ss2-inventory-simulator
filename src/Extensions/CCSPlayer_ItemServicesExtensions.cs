/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayer_ItemServicesExtensions
{
    public static void UpdateWearables(this CCSPlayer_ItemServices self)
    {
        Natives.CCSPlayer_ItemServices_SetWearables.Call(self.Address);
    }
}
