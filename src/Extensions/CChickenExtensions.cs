/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CChickenExtensions
{
    public static void ApplyPetStyle(this CChicken self, InventoryItem item)
    {
        if (item.Style == null || item.Style <= 0)
            return;
        var skeletonInstance = self.GetSkeletonInstance();
        if (skeletonInstance == null)
            return;
        skeletonInstance.MaterialGroup = new CUtlStringToken(item.Style.Value.ToString());
        skeletonInstance.MaterialGroupUpdated();
    }
}
