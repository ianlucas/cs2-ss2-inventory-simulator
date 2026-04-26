/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using SwiftlyS2.Shared.Sounds;
using SwiftlyS2.Shared.SteamAPI;

namespace InventorySimulator;

public static class IPlayerExtensions
{
    private static readonly SoundEvent _sprayCanShakeSound = new("SprayCan.Shake");
    private static readonly SoundEvent _sprayCanPaintSound = new("SprayCan.Paint");

    extension(IPlayer self)
    {
        public void HandleConnect()
        {
            self.Controller.Revalidate();
            self.RefreshInventory();
        }

        public async void RefreshInventory(bool force = false)
        {
            if (!force)
            {
                await self.FetchInventory();
                Swiftly.Core.Scheduler.NextWorldUpdate(() =>
                {
                    if (self.IsValid)
                        self.HandleInventoryLoad();
                });
                return;
            }
            var oldInventory = self.Controller.GetState().Inventory;
            await self.FetchInventory(force: true);
            Swiftly.Core.Scheduler.NextWorldUpdate(() =>
            {
                if (self.IsValid)
                {
                    self.SendChat(
                        Swiftly.Core.Localizer[
                            "invsim.ws_completed",
                            InventorySimulatorCtx.GetChatPrefix()
                        ]
                    );
                    self.HandleInventoryLoad();
                    self.HandlePostRefreshInventory(oldInventory);
                }
            });
        }

        public async Task FetchInventory(bool force = false)
        {
            var controllerState = self.Controller.GetState();
            var existing = controllerState.Inventory;
            if (!force && controllerState.Inventory != null)
                return;
            if (controllerState.IsFetching)
                return;
            controllerState.IsFetching = true;
            var response = await Api.FetchEquippedAsync(self.SteamID);
            if (response != null)
            {
                var inventory = new PlayerInventory(response);
                if (existing != null)
                    inventory.WeaponWearCache = existing.WeaponWearCache;
                inventory.InitializeWearOverrides();
                controllerState.WsUpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                controllerState.Inventory = inventory;
            }
            controllerState.IsFetching = false;
            controllerState.TriggerPostFetch();
        }

        public void HandleInventoryLoad()
        {
            var inventory = self.Controller.InventoryServices?.GetInventory();
            if (inventory?.IsValid == true)
                inventory.SendInventoryUpdateEvent();
        }

        public void HandlePostRefreshInventory(PlayerInventory? oldInventory)
        {
            var inventory = self.Controller.GetState().Inventory;
            if (inventory != null && ConVars.IsWsImmediately.Value)
            {
                self.RegiveAgent(inventory, oldInventory);
                self.RegiveGloves(inventory, oldInventory);
                self.RegiveWeapons(inventory, oldInventory);
            }
        }

        public bool IsUseCmdBusy()
        {
            if (self.PlayerPawn?.IsBuyMenuOpen == true)
                return true;
            if (self.PlayerPawn?.IsDefusing == true)
                return true;
            var weapon = self.PlayerPawn?.WeaponServices?.ActiveWeapon.Value;
            if (weapon?.DesignerName != "weapon_c4")
                return false;
            var c4 = weapon.As<CC4>();
            return c4.IsPlantingViaUse;
        }

        public void HandleProcessUsercmds()
        {
            if (
                (self.PressedButtons & GameButtonFlags.E) != 0
                && self.PlayerPawn?.IsAbleToApplySpray() == true
            )
            {
                var controllerState = self.Controller.GetState();
                if (self.IsUseCmdBusy())
                    controllerState.IsUseCmdBlocked = true;
                controllerState.DisposeUseCmdTimer();
                controllerState.UseCmdTimer = Swiftly.Core.Scheduler.DelayBySeconds(
                    0.1f,
                    () =>
                    {
                        if (controllerState.IsUseCmdBlocked)
                            controllerState.IsUseCmdBlocked = false;
                        else if (self.IsValid && !self.IsUseCmdBusy())
                            self.TrySprayGraffiti();
                    }
                );
            }
        }

        public void RegiveAgent(PlayerInventory inventory, PlayerInventory? oldInventory)
        {
            if (ConVars.MinModels.Value > 0)
                return;
            var pawn = self.PlayerPawn;
            if (pawn == null)
                return;
            var teamNum = self.Controller.TeamNum;
            var item = inventory.Agents.TryGetValue(teamNum, out var a) ? a : null;
            var oldItem =
                oldInventory != null && oldInventory.Agents.TryGetValue(teamNum, out a) ? a : null;
            if (oldItem == item)
                return;
            pawn.SetModelFromLoadout();
            pawn.SetModelFromClass();
            pawn.AcceptInput("SetBodygroup", "default_gloves,1");
        }

        public void RegiveGloves(PlayerInventory inventory, PlayerInventory? oldInventory)
        {
            var pawn = self.PlayerPawn;
            var itemServices = pawn?.ItemServices;
            if (pawn == null || itemServices == null)
                return;
            var isFallbackTeam = ConVars.IsFallbackTeam.Value;
            var teamNum = self.Controller.TeamNum;
            var item = inventory.GetGloves(teamNum, isFallbackTeam);
            var oldItem = oldInventory?.GetGloves(teamNum, isFallbackTeam);
            if (oldItem == item)
                return;
            itemServices.UpdateWearables();
            // Thanks to @samyycX.
            pawn.AcceptInput("SetBodygroup", "first_or_third_person,0");
            Swiftly.Core.Scheduler.NextWorldUpdate(() =>
            {
                if (pawn.IsValid && itemServices.IsValid)
                    pawn.AcceptInput("SetBodygroup", "first_or_third_person,1");
            });
        }

        public void RegiveWeapons(PlayerInventory inventory, PlayerInventory? oldInventory)
        {
            var pawn = self.PlayerPawn;
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
                if (weapon.OriginalOwnerXuidLow != (uint)self.SteamID)
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
                    var isFallbackTeam = ConVars.IsFallbackTeam.Value;
                    var oldItem =
                        data.GearSlot is gear_slot_t.GEAR_SLOT_KNIFE
                            ? oldInventory?.GetKnife(self.Controller.TeamNum, isFallbackTeam)
                            : oldInventory?.GetWeapon(
                                self.Controller.TeamNum,
                                entityDef,
                                isFallbackTeam
                            );
                    var item =
                        data.GearSlot is gear_slot_t.GEAR_SLOT_KNIFE
                            ? inventory.GetKnife(self.Controller.TeamNum, isFallbackTeam)
                            : inventory.GetWeapon(
                                self.Controller.TeamNum,
                                entityDef,
                                isFallbackTeam
                            );
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
                var weapon = self.PlayerPawn?.ItemServices?.GiveItem<CBasePlayerWeapon>(
                    actualDesignerName
                );
                if (weapon != null)
                    Swiftly.Core.Scheduler.Delay(
                        32,
                        () =>
                        {
                            if (weapon.IsValid)
                            {
                                weapon.Clip1 = clip;
                                weapon.Clip1Updated();
                                weapon.ReserveAmmo[0] = reserve;
                                weapon.ReserveAmmoUpdated();
                                Swiftly.Core.Scheduler.NextWorldUpdate(() =>
                                {
                                    if (active && self.IsValid)
                                    {
                                        var command = gearSlot switch
                                        {
                                            gear_slot_t.GEAR_SLOT_RIFLE => "slot1",
                                            gear_slot_t.GEAR_SLOT_PISTOL => "slot2",
                                            gear_slot_t.GEAR_SLOT_KNIFE => "slot3",
                                            _ => null,
                                        };
                                        if (command != null)
                                            self.ExecuteCommand(command);
                                    }
                                });
                            }
                        }
                    );
            }
        }

        public async void SignIn()
        {
            var controllerState = self.Controller.GetState();
            if (controllerState.IsFetching)
                return;
            controllerState.IsFetching = true;
            var response = await Api.SendSignIn(self.SteamID.ToString());
            controllerState.IsFetching = false;
            Swiftly.Core.Scheduler.NextWorldUpdate(() =>
            {
                var prefix = InventorySimulatorCtx.GetChatPrefix();
                if (response == null)
                {
                    self?.SendChat(Swiftly.Core.Localizer["invsim.login_failed", prefix]);
                    return;
                }
                self?.SendChat(
                    Swiftly.Core.Localizer[
                        "invsim.login",
                        prefix,
                        $"{Api.GetUrl("/api/sign-in/callback")}?token={response.Token}"
                    ]
                );
            });
        }

        public void TrySprayGraffiti()
        {
            if (!ConVars.IsSprayEnabled.Value)
                return;
            var controllerState = self.Controller.GetState();
            var cooldown = ConVars.SprayCooldown.Value;
            var diff = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - controllerState.SprayUsedAt;
            if (diff < cooldown)
            {
                self.SendChat(
                    Swiftly.Core.Localizer[
                        "invsim.spray_cooldown",
                        InventorySimulatorCtx.GetChatPrefix(),
                        cooldown - diff
                    ]
                );
                return;
            }
            self.SprayGraffiti();
        }

        public unsafe void SprayGraffiti()
        {
            if (!self.IsValid)
                return;
            var item = self.Controller.GetState().Inventory?.Graffiti;
            if (item == null || item.Def == null || item.Tint == null)
                return;
            var pawn = self.PlayerPawn;
            if (pawn == null || pawn.LifeState != (int)LifeState_t.LIFE_ALIVE)
                return;
            var movementServices = pawn.MovementServices?.As<CCSPlayer_MovementServices>();
            if (movementServices == null)
                return;
            var trace = stackalloc CGameTrace[1];
            if (!pawn.IsAbleToApplySpray((nint)trace) || (nint)trace == nint.Zero)
                return;
            _sprayCanShakeSound.Recipients.AddRecipient(self.PlayerID);
            _sprayCanShakeSound.Emit();
            _sprayCanShakeSound.Recipients.RemoveRecipient(self.PlayerID);
            self.Controller.GetState().SprayUsedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var sprayDecal =
                Swiftly.Core.EntitySystem.CreateEntityByDesignerName<CPlayerSprayDecal>(
                    "player_spray_decal"
                );
            if (sprayDecal != null)
            {
                sprayDecal.EndPos += trace->EndPos;
                sprayDecal.Start += trace->EndPos;
                sprayDecal.Left += movementServices.Left;
                sprayDecal.Normal += trace->HitNormal;
                sprayDecal.AccountID = (uint)self.SteamID;
                sprayDecal.Player = item.Def.Value;
                sprayDecal.TintID = item.Tint.Value;
                sprayDecal.DispatchSpawn();
                _sprayCanPaintSound.Recipients.AddRecipient(self.PlayerID);
                _sprayCanPaintSound.Emit();
                _sprayCanPaintSound.Recipients.RemoveRecipient(self.PlayerID);
            }
        }

        public void HandleSprayDecalCreated(CPlayerSprayDecal sprayDecal)
        {
            var item = self.Controller.GetState().Inventory?.Graffiti;
            if (item != null && item.Def != null && item.Tint != null)
            {
                sprayDecal.Player = item.Def.Value;
                sprayDecal.PlayerUpdated();
                sprayDecal.TintID = item.Tint.Value;
                sprayDecal.TintIDUpdated();
            }
        }

        public void IncrementWeaponStatTrak(string designerName, string weaponItemId)
        {
            var weapon = self.PlayerPawn?.WeaponServices?.ActiveWeapon.Value;
            if (
                weapon == null
                || !weapon.HasCustomItemID()
                || !ulong.TryParse(weaponItemId, out var parsedItemId)
                || weapon.AttributeManager.Item.AccountID
                    != new CSteamID(self.SteamID).GetAccountID().m_AccountID
                || weapon.AttributeManager.Item.ItemID != parsedItemId
            )
                return;
            var inventory = self.Controller.GetState().Inventory;
            var isFallbackTeam = ConVars.IsFallbackTeam.Value;
            var item = ItemHelper.IsMeleeDesignerName(designerName)
                ? inventory?.GetKnife(self.Controller.TeamNum, isFallbackTeam)
                : inventory?.GetWeapon(
                    self.Controller.TeamNum,
                    weapon.AttributeManager.Item.ItemDefinitionIndex,
                    isFallbackTeam
                );
            if (item == null || item.Stattrak == null || item.Stattrak < 0 || item.Uid == null)
                return;
            item.Stattrak += 1;
            var statTrak = TypeHelper.ViewAs<int, float>(item.Stattrak.Value);
            weapon.AttributeManager.Item.NetworkedDynamicAttributes.SetOrAddAttribute(
                "kill eater",
                statTrak
            );
            Api.SendStatTrakIncrement(self.SteamID, item.Uid.Value);
        }

        public void IncrementMusicKitStatTrak(EventRoundMvp @event)
        {
            var item = self.Controller.GetState().Inventory?.MusicKit;
            if (item != null && item.Uid != null && item.Stattrak != null && item.Stattrak >= 0)
            {
                item.Stattrak += 1;
                @event.MusickItMvps = item.Stattrak.Value;
                Api.SendStatTrakIncrement(self.SteamID, item.Uid.Value);
            }
        }

        public void HandleDisconnect()
        {
            if (!ConVars.IsPersistInventory.Value && !Inventories.Has(self.SteamID))
                self.Controller.GetState().Inventory = null;
        }
    }
}
