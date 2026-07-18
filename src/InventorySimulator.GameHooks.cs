/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.GameHooks;

namespace InventorySimulator;

public partial class InventorySimulator
{
    public void OnProcessUsercmdsPre(ref ProcessUsercmdsPreContext ctx)
    {
        if (!ConVars.IsSprayOnUse.Value)
            return;
        ctx.Params.Player.HandleProcessUsercmds();
    }
}
