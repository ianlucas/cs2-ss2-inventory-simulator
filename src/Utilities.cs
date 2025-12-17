/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CS2Lib;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static partial class Utilities
{
    public static string FormatUrl(string format, string urlString)
    {
        if (!Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
            return format;
        return UrlFormatPattern()
            .Replace(
                format,
                match =>
                {
                    var prop = uri.GetType()
                        .GetProperty(
                            match.Groups[1].Value,
                            BindingFlags.Public | BindingFlags.Instance
                        );
                    return prop?.GetValue(uri)?.ToString() ?? match.Value;
                }
            );
    }

    [GeneratedRegex(@"\{(\w+)\}")]
    private static partial Regex UrlFormatPattern();

    public static TTo ViewAs<TFrom, TTo>(TFrom value)
        where TFrom : unmanaged
        where TTo : unmanaged
    {
        if (Unsafe.SizeOf<TFrom>() != Unsafe.SizeOf<TTo>())
            throw new ArgumentException("Size mismatch");
        return Unsafe.As<TFrom, TTo>(ref value);
    }

    public static byte ToggleTeam(byte team) =>
        team > (byte)Team.Spectator ? (byte)((Team)team == Team.T ? Team.CT : Team.T) : team;
}

public static class PlayerManagerExtensions
{
    public static IPlayer? GetPlayerFromSteamID(this IPlayerManagerService manager, ulong steamID)
    {
        return manager.GetAllPlayers().FirstOrDefault(p => p.SteamID == steamID);
    }
}

public static class CBasePlayerWeaponExtensions
{
    public static string GetDesignerName(this CBasePlayerWeapon weapon)
    {
        var designerName = weapon.AttributeManager.Item.GetDesignerName() ?? weapon.DesignerName;
        return CS2Items.IsMeleeDesignerName(designerName) ? "weapon_knife" : designerName;
    }
}

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
