/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CEconItemSchema_GetItemDefinitionDelegate(
        nint thisPtr,
        uint defIndex,
        byte flag
    );

    public static readonly IUnmanagedFunction<CEconItemSchema_GetItemDefinitionDelegate> CEconItemSchema_GetItemDefinition =
        GetFunctionBySignature<CEconItemSchema_GetItemDefinitionDelegate>(
            "CEconItemSchema::GetItemDefinition"
        );
}
