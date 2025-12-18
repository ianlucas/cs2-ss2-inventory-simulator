/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Memory;
using SwiftlyS2.Shared.Schemas;

namespace InventorySimulator;

public static partial class Natives
{
    private static ISwiftlyCore? _core;

    public static void Initialize(ISwiftlyCore core)
    {
        _core = core;
    }

    public static T ToSchemaClass<T>(nint address)
        where T : class, ISchemaClass<T>
    {
        if (_core is null)
            throw new InvalidOperationException(
                "Natives not initialized. Call Initialize() first."
            );
        return _core.Memory.ToSchemaClass<T>(address);
    }

    private static IUnmanagedFunction<TDelegate> FromSignature<TDelegate>(string signature)
        where TDelegate : Delegate
    {
        if (_core is null)
            throw new InvalidOperationException(
                "Natives not initialized. Call Initialize() first."
            );
        nint? address = _core.GameData.GetSignature(signature);
        if (address is null)
            throw new InvalidOperationException(
                $"Failed to locate game function signature '{signature}'. The function may not exist in the current game version or the signature pattern may be outdated."
            );
        return _core.Memory.GetUnmanagedFunctionByAddress<TDelegate>(address.Value);
    }

    private static int FromOffset(string offset)
    {
        if (_core is null)
            throw new InvalidOperationException(
                "Natives not initialized. Call Initialize() first."
            );
        return _core.GameData.GetOffset(offset);
    }
}
