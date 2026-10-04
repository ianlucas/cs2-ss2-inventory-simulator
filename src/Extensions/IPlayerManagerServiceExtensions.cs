/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Players;

namespace InventorySimulator;

public static class IPlayerManagerServiceExtensions
{
    extension(IPlayerManagerService self)
    {
        public IPlayer? GetPlayerFromSteamID(ulong steamID)
        {
            return self.GetAllPlayers().FirstOrDefault(p => p.SteamID == steamID);
        }
    }
}
