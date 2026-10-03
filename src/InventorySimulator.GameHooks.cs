/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public partial class InventorySimulator
{
    public void OnProcessUsercmdsPre(ref ProcessUsercmdsPreContext ctx)
    {
        if (!ConVars.IsSprayOnUse.Value)
            return;
        ctx.Params.Player.HandleProcessUsercmds();
    }

    public void OnTakeDamagePre(ref TakeDamageEntityPreContext ctx)
    {
        if (!ConVars.IsPetImmortal.Value)
            return;
        var entity = ctx.Params.Entity;
        if (entity.DesignerName != "chicken" || entity.As<CChicken>().Owner.Value == null)
            return;
        ctx.SetHookResult(HookResult.CancelOriginal);
    }
}
