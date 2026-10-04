/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using SwiftlyS2.Shared.EntitySystem;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace InventorySimulator;

public static class IEntitySystemServiceExtensions
{
    extension(IEntitySystemService self)
    {
        public bool IsWarmupPeriod()
        {
            return self.GetGameRules()?.WarmupPeriod == true;
        }

        public SpawnPoint? GetRandomSpawnPoint(byte team)
        {
            if (team != (byte)Team.T && team != (byte)Team.CT)
                return null;
            var spawnPoints = new List<SpawnPoint>();
            var randomSpawn = Runtime.Core.ConVar.Find<int>("mp_randomspawn")?.Value ?? 0;
            if (randomSpawn == 1 || randomSpawn == team)
                spawnPoints = self.GetAllEnabledSpawnPoints("info_deathmatch_spawn");
            if (spawnPoints.Count == 0)
                spawnPoints = self.GetAllEnabledSpawnPoints(
                    team == (byte)Team.T ? "info_player_terrorist" : "info_player_counterterrorist"
                );
            return spawnPoints.Count > 0
                ? spawnPoints[Random.Shared.Next(spawnPoints.Count)]
                : null;
        }

        private List<SpawnPoint> GetAllEnabledSpawnPoints(string designerName)
        {
            return
            [
                .. self.GetAllEntitiesByDesignerName<SpawnPoint>(designerName)
                    .Where(spawnPoint => spawnPoint.Enabled),
            ];
        }
    }
}
