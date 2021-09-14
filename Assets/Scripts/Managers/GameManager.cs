using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private byte BotsCount { set; get; }

    [Header("Game Options"), SerializeField]
    private byte _botsLimit;

    [Header("Prefabs"), SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private Transform[] _spawns;

    private void Start()
    {
        SpawnEnemy();
    }

    private void OnEnemyDied()
    {
        BotsCount--;
    }
    private void OnPlayerDied()
    {

    }

    private Vector2 GetRandomSpawn() => _spawns[Random.Range(0, _spawns.Length)].position;
    private bool SpawnEnemy()
    {
        if (BotsCount >= _botsLimit) return false;
        var tank = Instantiate(_enemyPrefab, GetRandomSpawn(), Quaternion.identity);
        var component = tank.GetComponent<BaseTank>();

        component.Died += OnEnemyDied;

        BotsCount++;
        return true;
    }
}
