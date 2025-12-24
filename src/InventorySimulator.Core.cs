/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using SwiftlyS2.Shared.SteamAPI;

namespace InventorySimulator;

public partial class InventorySimulator
{
    public void GivePlayerAgent(IPlayer player)
    {
        if (MinModels.Value > 0)
        {
            if (player.Controller.Team == Team.T)
                player.PlayerPawn?.SetModel("characters/models/tm_phoenix/tm_phoenix.vmdl");
            if (player.Controller.Team == Team.CT)
                player.PlayerPawn?.SetModel("characters/models/ctm_sas/ctm_sas.vmdl");
            return;
        }
    }

    public void GivePlayerGloves(IPlayer player, PlayerInventory inventory)
    {
        var pawn = player.PlayerPawn;
        if (pawn?.IsValid != true)
            return;
        var fallback = IsFallbackTeam.Value;
        var item = inventory.GetGloves(player.Controller.TeamNum, fallback);
        if (item != null)
        {
            // Workaround by @daffyyyy.
            var model = pawn.CBodyComponent?.SceneNode?.GetSkeletonInstance()?.ModelState.ModelName;
            if (!string.IsNullOrEmpty(model))
            {
                pawn.SetModel("characters/models/tm_jumpsuit/tm_jumpsuit_varianta.vmdl");
                pawn.SetModel(model);
            }
            var glove = pawn.EconGloves;
            Core.Scheduler.NextTick(() =>
            {
                if (pawn.IsValid)
                {
                    glove.ApplyAttributes(item);
                    // Thanks to xstage and stefanx111
                    pawn.AcceptInput("SetBodygroup", value: "default_gloves,1");
                }
            });
        }
    }

    public void GivePlayerWeaponSkin(
        CCSPlayerController controller,
        CBasePlayerWeapon weapon,
        bool isMelee
    )
    {
        var isFallbackTeam = IsFallbackTeam.Value;
        if (controller?.SteamID != 0 && controller?.InventoryServices?.IsValid == true)
        {
            var inventory = GetPlayerInventoryBySteamID(controller.SteamID);
            var item = isMelee
                ? inventory.GetKnife(controller.TeamNum, isFallbackTeam)
                : inventory.GetWeapon(
                    controller.TeamNum,
                    weapon.AttributeManager.Item.ItemDefinitionIndex,
                    isFallbackTeam
                );
            if (item != null)
                weapon.AttributeManager.Item.ApplyAttributes(item, weapon, controller);
        }
    }

    public void GivePlayerWeaponStatTrakIncrement(
        IPlayer player,
        string designerName,
        string weaponItemId
    )
    {
        var weapon = player.PlayerPawn?.WeaponServices?.ActiveWeapon.Value;
        if (
            weapon == null
            || !weapon.HasCustomItemID()
            || weapon.AttributeManager.Item.AccountID
                != new CSteamID(player.SteamID).GetAccountID().m_AccountID
            || weapon.AttributeManager.Item.ItemID != ulong.Parse(weaponItemId)
        )
            return;
        var inventory = GetPlayerInventory(player);
        var isFallbackTeam = IsFallbackTeam.Value;
        var item = ItemHelper.IsMeleeDesignerName(designerName)
            ? inventory.GetKnife(player.Controller.TeamNum, isFallbackTeam)
            : inventory.GetWeapon(
                player.Controller.TeamNum,
                weapon.AttributeManager.Item.ItemDefinitionIndex,
                isFallbackTeam
            );
        if (item == null)
            return;
        item.Stattrak += 1;
        var statTrak = TypeHelper.ViewAs<int, float>(item.Stattrak);
        weapon.AttributeManager.Item.NetworkedDynamicAttributes.SetOrAddAttribute(
            "kill eater",
            statTrak
        );
        SendStatTrakIncrement(player.SteamID, item.Uid);
    }

    public void RegivePlayerWeapons(
        IPlayer player,
        PlayerInventory inventory,
        PlayerInventory oldInventory
    )
    {
        var pawn = player.PlayerPawn;
        var weaponServices = pawn?.WeaponServices?.As<CCSPlayer_WeaponServices>();
        if (pawn == null || weaponServices == null)
            return;
        var activeDesignerName = weaponServices.ActiveWeapon.Value?.DesignerName;
        var targets = new List<(string, string, int, int, bool, gear_slot_t)>();
        foreach (var handle in weaponServices.MyWeapons)
        {
            var weapon = handle.Value?.As<CCSWeaponBase>();
            if (weapon == null || weapon.DesignerName.Contains("weapon_") != true)
                continue;
            if (weapon.OriginalOwnerXuidLow != (uint)player.SteamID)
                continue;
            var data = weapon.VData.As<CCSWeaponBaseVData>();
            if (
                data.GearSlot
                is gear_slot_t.GEAR_SLOT_RIFLE
                    or gear_slot_t.GEAR_SLOT_PISTOL
                    or gear_slot_t.GEAR_SLOT_KNIFE
            )
            {
                var entityDef = weapon.AttributeManager.Item.ItemDefinitionIndex;
                var isFallbackTeam = IsFallbackTeam.Value;
                var oldItem =
                    data.GearSlot is gear_slot_t.GEAR_SLOT_KNIFE
                        ? oldInventory.GetKnife(player.Controller.TeamNum, isFallbackTeam)
                        : oldInventory.GetWeapon(
                            player.Controller.TeamNum,
                            entityDef,
                            isFallbackTeam
                        );
                var item =
                    data.GearSlot is gear_slot_t.GEAR_SLOT_KNIFE
                        ? inventory.GetKnife(player.Controller.TeamNum, isFallbackTeam)
                        : inventory.GetWeapon(player.Controller.TeamNum, entityDef, isFallbackTeam);
                if (oldItem == item)
                    continue;
                var clip = weapon.Clip1;
                var reserve = weapon.ReserveAmmo[0];
                targets.Add(
                    (
                        weapon.DesignerName,
                        weapon.GetDesignerName(),
                        clip,
                        reserve,
                        activeDesignerName == weapon.DesignerName,
                        data.GearSlot
                    )
                );
            }
        }
        foreach (var target in targets)
        {
            var designerName = target.Item1;
            var actualDesignerName = target.Item2;
            var clip = target.Item3;
            var reserve = target.Item4;
            var active = target.Item5;
            var gearSlot = target.Item6;
            var oldWeapon = (
                (CHandle<CBasePlayerWeapon>?)
                    weaponServices.MyWeapons.FirstOrDefault(h =>
                        h.Value?.DesignerName == designerName
                    )
            )?.Value;
            if (oldWeapon != null)
            {
                weaponServices.DropWeapon(oldWeapon);
                oldWeapon.Despawn();
            }
            var weapon = player.PlayerPawn?.ItemServices?.GiveItem<CBasePlayerWeapon>(
                actualDesignerName
            );
            if (weapon != null)
                Core.Scheduler.Delay(
                    32,
                    () =>
                    {
                        if (weapon.IsValid)
                        {
                            weapon.Clip1 = clip;
                            weapon.Clip1Updated();
                            weapon.ReserveAmmo[0] = reserve;
                            weapon.ReserveAmmoUpdated();
                            Core.Scheduler.NextWorldUpdate(() =>
                            {
                                if (active && player.IsValid)
                                {
                                    var command = gearSlot switch
                                    {
                                        gear_slot_t.GEAR_SLOT_RIFLE => "slot1",
                                        gear_slot_t.GEAR_SLOT_PISTOL => "slot2",
                                        gear_slot_t.GEAR_SLOT_KNIFE => "slot3",
                                        _ => null,
                                    };
                                    if (command != null)
                                        player.ExecuteCommand(command);
                                }
                            });
                        }
                    }
                );
        }
    }

    public void GiveOnPlayerSpawn(IPlayer player)
    {
        var inventory = GetPlayerInventory(player);
        GivePlayerAgent(player);
        GivePlayerGloves(player, inventory);
    }

    public void GiveOnRefreshPlayerInventory(IPlayer player, PlayerInventory oldInventory)
    {
        var inventory = GetPlayerInventory(player);
        if (IsWsImmediately.Value)
        {
            GivePlayerGloves(player, inventory);
            RegivePlayerWeapons(player, inventory, oldInventory);
        }
    }

    public void OnFileChanged()
    {
        LoadPlayerInventories();
    }

    public void OnIsRequireInventoryChanged()
    {
        if (IsRequireInventory.Value)
            OnActivatePlayerHookGuid = Natives.CServerSideClientBase_ActivatePlayer.AddHook(
                OnActivatePlayer
            );
        else if (OnActivatePlayerHookGuid != null)
            Natives.CServerSideClientBase_ActivatePlayer.RemoveHook(OnActivatePlayerHookGuid.Value);
    }
}
