/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Players;

namespace InventorySimulator;

public class PlayerInventory(EquippedV3Response data)
{
    private readonly EquippedV3Response _data = data;

    public static PlayerInventory Empty() => new(new());

    public WeaponEconItem? GetKnife(byte team, bool fallback)
    {
        if (_data.Knives.TryGetValue(team, out var knife))
        {
            knife.WearOverride = GetWeaponEconItemWear(knife);
            return knife;
        }
        if (fallback && _data.Knives.TryGetValue(TeamHelper.ToggleTeam(team), out knife))
        {
            knife.WearOverride = GetWeaponEconItemWear(knife);
            return knife;
        }
        return null;
    }

    public Dictionary<ushort, WeaponEconItem> GetWeapons(byte team)
    {
        return (Team)team == Team.T ? _data.TWeapons : _data.CTWeapons;
    }

    public WeaponEconItem? GetWeapon(byte team, ushort def, bool fallback)
    {
        if (GetWeapons(team).TryGetValue(def, out var weapon))
        {
            weapon.WearOverride = GetWeaponEconItemWear(weapon);
            return weapon;
        }
        if (fallback && GetWeapons(TeamHelper.ToggleTeam(team)).TryGetValue(def, out weapon))
        {
            weapon.WearOverride = GetWeaponEconItemWear(weapon);
            return weapon;
        }
        return null;
    }

    public BaseEconItem? GetGloves(byte team, bool fallback)
    {
        if (_data.Gloves.TryGetValue(team, out var glove))
        {
            return glove;
        }
        if (fallback && _data.Gloves.TryGetValue(TeamHelper.ToggleTeam(team), out glove))
        {
            return glove;
        }
        return null;
    }

    // It looks like CS2's client caches the weapon materials based on wear and seed. Until we figure out
    // the proper way to force a material update, we need to ensure that every paint has a unique wear so
    // that: 1) it gets regenerated, and 2) there are no rendering issues. As a drawback, every time
    // players use !ws, their weapon's wear will decay, but I think it's a good trade-off since it also
    // forces the sticker to regenerate. This approach is based on workarounds by @stefanx111 and @bklol.

    public Dictionary<int, Dictionary<float, (ushort, string)>> CachedWeaponEconItems = [];

    public float GetWeaponEconItemWear(WeaponEconItem item)
    {
        var wear = item.Wear;
        var stickers = string.Join("_", item.Stickers.Select(s => s.Def));
        var cachedByWear = CachedWeaponEconItems.TryGetValue(item.Paint, out var c) ? c : [];
        while (true)
        {
            (ushort, string)? pair = cachedByWear.TryGetValue(wear, out var p) ? p : null;
            var cached = pair?.Item1 == item.Def && pair?.Item2 == stickers;
            if (pair == null || cached)
            {
                cachedByWear[wear] = (item.Def, stickers);
                CachedWeaponEconItems[item.Paint] = cachedByWear;
                return wear;
            }
            wear += 0.001f;
        }
    }
}
