/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace InventorySimulator;

public static class Inventories
{
    private static readonly Dictionary<ulong, PlayerInventory> _loadedInventories = [];
    private static readonly string _inventoryFileDir = "csgo/addons/swiftlycs2/configs";

    public static bool Load()
    {
        try
        {
            var filename = ConVars.File.Value;
            var candidates = new[]
            {
                Path.Combine(Swiftly.Core.GameDirectory, _inventoryFileDir, filename),
                Path.Combine(Swiftly.Core.GameDirectory, "csgo", filename),
            };
            var path =
                candidates.FirstOrDefault(File.Exists)
                ?? (Path.IsPathRooted(filename) && File.Exists(filename) ? filename : null);
            if (path == null)
                return false;
            string json = File.ReadAllText(path);
            var inventories = JsonSerializer.Deserialize<Dictionary<ulong, PlayerInventory>>(json);
            _loadedInventories.Clear();
            if (inventories != null)
                foreach (var pair in inventories)
                    _loadedInventories.TryAdd(pair.Key, pair.Value);
            return true;
        }
        catch
        {
            Swiftly.Core.Logger.LogError("Error when processing \"{File}\".", ConVars.File.Value);
            return false;
        }
    }

    public static bool TryGet(ulong steamId, [MaybeNullWhen(false)] out PlayerInventory inventory)
    {
        if (_loadedInventories.TryGetValue(steamId, out var value))
        {
            inventory = value;
            return true;
        }
        inventory = default;
        return false;
    }

    public static PlayerInventory? Get(ulong steamId)
    {
        return _loadedInventories.TryGetValue(steamId, out var inventory) ? inventory : null;
    }
}
