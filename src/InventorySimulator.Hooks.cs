/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public partial class InventorySimulator
{
    public Natives.CServerSideClientBase_ActivatePlayerDelegate OnActivatePlayer(
        Func<Natives.CServerSideClientBase_ActivatePlayerDelegate> next
    )
    {
        return (thisPtr) =>
        {
            var userid = (ushort)
                Marshal.ReadInt16(thisPtr + Natives.CServerSideClientBase_m_UserID);
            var player = Core.PlayerManager.GetPlayer(userid);
            if (player != null && !player.IsFakeClient && player.Controller != null)
                if (!PlayerInventoryManager.ContainsKey(player.SteamID))
                {
                    PlayerPostFetchManager[player.SteamID] = () =>
                        Core.Scheduler.NextWorldUpdate(() =>
                        {
                            if (player.Controller.IsValid)
                                Natives.CServerSideClientBase_ActivatePlayer.CallOriginal(thisPtr);
                        });
                    if (!PlayerInFetchManager.ContainsKey(player.SteamID))
                        RefreshPlayerInventory(player);
                    return;
                }
            next()(thisPtr);
        };
    }

    public Natives.CCSPlayer_ItemServices_GiveNamedItemDelegate OnGiveNamedItem(
        Func<Natives.CCSPlayer_ItemServices_GiveNamedItemDelegate> next
    )
    {
        return (thisPtr, pchName, a3, pScriptItem, a5, a6) =>
        {
            var designerName = Marshal.PtrToStringUTF8(pchName);
            var isMelee = designerName != null && ItemHelper.IsMeleeDesignerName(designerName);
            var ret = next()(thisPtr, pchName, a3, pScriptItem, a5, a6);
            var weapon = Core.Memory.ToSchemaClass<CBasePlayerWeapon>(ret);
            var itemServices = Core.Memory.ToSchemaClass<CCSPlayer_ItemServices>(thisPtr);
            var controller = itemServices.GetController();
            if (controller != null)
                GivePlayerWeaponSkin(controller, weapon, isMelee);
            return ret;
        };
    }
}
