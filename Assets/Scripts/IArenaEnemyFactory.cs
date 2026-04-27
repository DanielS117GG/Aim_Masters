using UnityEngine;

public interface IArenaEnemyFactory
{
    ArenaMovingEnemy CreateMovingEnemy(Vector3 position, ArenaSpawner ownerSpawner, Vector3 arenaMinBounds, Vector3 arenaMaxBounds);
    ArenaDiskEnemy CreateDiskEnemy(Vector3 position, ArenaSpawner ownerSpawner);
}