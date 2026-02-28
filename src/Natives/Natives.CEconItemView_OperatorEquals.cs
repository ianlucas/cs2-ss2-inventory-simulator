/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate nint CEconItemView_OperatorEqualsDelegate(nint thisPtr, nint other);

    public static readonly IUnmanagedFunction<CEconItemView_OperatorEqualsDelegate> CEconItemView_OperatorEquals =
        GetFunctionBySignature<CEconItemView_OperatorEqualsDelegate>("CEconItemView::operator=");
}
