/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CEconItemSchema_GetItemDefinitionByNameDelegate(
        nint thisPtr,
        nint pchName
    );

    public static readonly IUnmanagedFunction<CEconItemSchema_GetItemDefinitionByNameDelegate> CEconItemSchema_GetItemDefinitionByName =
        GetFunctionBySignature<CEconItemSchema_GetItemDefinitionByNameDelegate>(
            "CEconItemSchema::GetItemDefinitionByName"
        );
}
