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

    public static bool UpdatePet(this CChicken self, InventoryItem item, nint itemView)
    {
        if (item.Model == null)
            return false;
        var model = $"{item.Model}.vmdl";
        Natives.CEconItemView_OperatorEquals.Call(self.AttributeManager.Item.Address, itemView);
        if (!string.Equals(self.GetModel(), model, StringComparison.OrdinalIgnoreCase))
            self.SetModel(model);
        self.ApplyPetStyle(item);
        return true;
    }
}
