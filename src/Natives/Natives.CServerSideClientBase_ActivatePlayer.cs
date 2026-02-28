/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Memory;

namespace InventorySimulator;

public static partial class Natives
{
    public delegate void CServerSideClientBase_ActivatePlayerDelegate(nint thisPtr);

    public static readonly IUnmanagedFunction<CServerSideClientBase_ActivatePlayerDelegate> CServerSideClientBase_ActivatePlayer =
        GetFunctionBySignature<CServerSideClientBase_ActivatePlayerDelegate>(
            "CServerSideClientBase::ActivatePlayer"
        );
}
