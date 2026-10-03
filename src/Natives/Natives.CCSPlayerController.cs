/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CCSPlayerController_GetPetChickenDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CCSPlayerController_GetPetChickenDelegate> CCSPlayerController_GetPetChicken =
        GetFunctionBySignature<CCSPlayerController_GetPetChickenDelegate>(
            "CCSPlayerController::GetPetChicken"
        );

    public delegate void CCSPlayerController_SetPetChickenDelegate(nint thisPtr, nint chicken);

    public static readonly IUnmanagedFunction<CCSPlayerController_SetPetChickenDelegate> CCSPlayerController_SetPetChicken =
        GetFunctionBySignature<CCSPlayerController_SetPetChickenDelegate>(
            "CCSPlayerController::SetPetChicken"
        );
}
