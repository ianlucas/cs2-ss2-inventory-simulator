/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Text.Json.Serialization;

namespace InventorySimulator;

public class WeaponEconItem : BaseEconItem
{
    [JsonPropertyName("legacy")]
    public bool Legacy { get; set; }

    [JsonPropertyName("nametag")]
    public required string Nametag { get; set; }

    [JsonPropertyName("stattrak")]
    public required int Stattrak { get; set; }

    [JsonPropertyName("stickers")]
    public required List<StickerItem> Stickers { get; set; }

    [JsonPropertyName("uid")]
    public required int Uid { get; set; }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not WeaponEconItem other)
            return false;

        return Def == other.Def
            && Paint == other.Paint
            && Seed == other.Seed
            && Wear == other.Wear
            && Legacy == other.Legacy
            && Nametag == other.Nametag
            && Stattrak == other.Stattrak
            && Stickers.SequenceEqual(other.Stickers);
    }

    public static bool operator ==(WeaponEconItem? left, WeaponEconItem? right)
    {
        if (ReferenceEquals(left, right))
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(WeaponEconItem? left, WeaponEconItem? right)
    {
        return !(left == right);
    }
}
