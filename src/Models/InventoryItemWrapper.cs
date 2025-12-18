/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace InventorySimulator;

public class InventoryItemWrapper
{
    public WeaponEconItem? WeaponItem { get; set; }
    public AgentItem? AgentItem { get; set; }
    public BaseEconItem? GloveItem { get; set; }
    public uint? PinItem { get; set; }
    public MusicKitItem? MusicKitItem { get; set; }

    public bool HasItem =>
        WeaponItem != null
        || AgentItem != null
        || GloveItem != null
        || PinItem != null
        || MusicKitItem != null;

    public static InventoryItemWrapper FromWeapon(WeaponEconItem item) =>
        new() { WeaponItem = item };

    public static InventoryItemWrapper FromAgent(AgentItem item) => new() { AgentItem = item };

    public static InventoryItemWrapper FromGlove(BaseEconItem item) => new() { GloveItem = item };

    public static InventoryItemWrapper FromPin(uint item) => new() { PinItem = item };

    public static InventoryItemWrapper FromMusicKit(MusicKitItem item) =>
        new() { MusicKitItem = item };

    public static InventoryItemWrapper Empty() => new();
}
