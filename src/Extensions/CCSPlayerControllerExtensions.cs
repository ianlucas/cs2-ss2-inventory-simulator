/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Concurrent;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class CCSPlayerControllerExtensions
{
    private static readonly ConcurrentDictionary<
        uint,
        CCSPlayerControllerState
    > _controllerStateManager = [];

    public static IEnumerable<CCSPlayerControllerState> GetAllStates() =>
        _controllerStateManager.Values;

    extension(CCSPlayerController self)
    {
        public CCSPlayerControllerState GetState()
        {
            return _controllerStateManager.GetOrAdd(self.Index, _ => new(self.SteamID));
        }

        public void Revalidate()
        {
            if (self.GetState().SteamID != self.SteamID)
                self.RemoveState();
        }

        public void RemoveState()
        {
            var controllerState = self.GetState();
            controllerState.DisposeUseCmdTimer();
            controllerState.ClearEconItemView();
            _controllerStateManager.TryRemove(self.Index, out var _);
        }

        public CChicken? GetPetChicken()
        {
            var chicken = Runtime.Core.Memory.ToSchemaClass<CChicken>(
                Natives.CCSPlayerController_GetPetChicken.Call(self.Address)
            );
            return chicken.IsValid ? chicken : null;
        }

        public void SetPetChicken(CChicken chicken)
        {
            Natives.CCSPlayerController_SetPetChicken.Call(self.Address, chicken.Address);
        }
    }
}
