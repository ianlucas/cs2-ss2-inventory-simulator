/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Players;

namespace InventorySimulator;

public static class IPlayerManagerServiceExtensions
{
    public static IPlayer? GetPlayerFromSteamID(this IPlayerManagerService manager, ulong steamID)
    {
        return manager.GetAllPlayers().FirstOrDefault(p => p.SteamID == steamID);
    }
}
