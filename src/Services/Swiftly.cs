/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared;

namespace InventorySimulator;

public static class Swiftly
{
    [SwiftlyInject]
    public static ISwiftlyCore Core { get; set; } = null!;

    public static void Initialize()
    {
        _ = Core;
    }
}
