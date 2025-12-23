/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CS2Lib;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CBasePlayerWeaponExtensions
{
    public static string GetDesignerName(this CBasePlayerWeapon weapon)
    {
        var designerName = weapon.AttributeManager.Item.GetDesignerName() ?? weapon.DesignerName;
        return CS2Items.IsMeleeDesignerName(designerName) ? "weapon_knife" : designerName;
    }

    public static bool HasCustomItemID(this CBasePlayerWeapon weapon)
    {
        return weapon.AttributeManager.Item.ItemID >= CEconItemViewExtensions.MinimumCustomItemID;
    }
}
