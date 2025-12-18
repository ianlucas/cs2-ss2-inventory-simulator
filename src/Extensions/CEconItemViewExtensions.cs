/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CS2Lib;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CEconItemViewExtensions
{
    public static string? GetDesignerName(this CEconItemView item)
    {
        var designerName = CS2Items.GetItemByDef(item.ItemDefinitionIndex)?.Model;
        return designerName != null ? $"weapon_{designerName}" : null;
    }

    public static bool IsMelee(this CEconItemView item) =>
        CS2Items.IsMeleeDef(item.ItemDefinitionIndex);
}
