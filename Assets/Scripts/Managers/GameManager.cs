using UnityEngine;

public class GameManager : MonoBehaviour
{
    private uint Score { set; get; } = 0;
    private byte BotsCount { set; get; }

    [Header("Managers")]
    [SerializeField]
    private AudioManager AudioManager;
    [SerializeField]
    private UIManager UIManager;

    [Header("Game Options"), SerializeField]
    private byte _botsLimit;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private Transform[] _spawns;

    private void Start()
    {
       // SpawnPlayer();
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
    }

    private void OnEnemyDied()
    {
        BotsCount--;
        Score++;
        UIManager.SetScore(Score);
        AudioManager.PlayAudioShot(AudioManager.TankAudio.Death);
    }
    private void OnPlayerDied()
    {

    }

    private Vector2 GetRandomSpawn() => _spawns[Random.Range(0, _spawns.Length)].position;
    private bool SpawnEnemy()
    {
        if (BotsCount >= _botsLimit) return false;
        var tank = Instantiate(_enemyPrefab, GetRandomSpawn(), Quaternion.identity);
        tank.transform.SetParent(GameObject.Find("[UNITS]").transform);
        var component = tank.GetComponent<BaseTank>();

        component.Died += OnEnemyDied;
        component.RecievedDamage += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Hit);

        BotsCount++;
        return true;
    }
    private void SpawnPlayer()
    {
        var tank = Instantiate(_playerPrefab, GetRandomSpawn(), Quaternion.identity);
        tank.transform.SetParent(GameObject.Find("[UNITS]").transform);
        var component = tank.GetComponent<PlayerController>();

        component.Died += OnPlayerDied;
        component.RecievedDamage += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Hit);
        component.Fire += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Shoot);
    }
}
