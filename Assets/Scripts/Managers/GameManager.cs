using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private uint Score { set; get; } = 0;
    private byte BotsCount { set; get; }
    private bool Paused { set; get; } = false;
    public static byte PlayerHealth { private set; get; }

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

    #region Spawns
    private struct Spawn
    {
        private bool _active;
        public readonly int Index;
        public readonly Vector2 Position;

        public Spawn(int index, Vector2 position)
        {
            _active = true;
            Index = index;
            Position = position;
        }

        public bool IsActive => _active;
        public void SetActive(bool isActive) => _active = isActive;
    }

    [SerializeField]
    private Transform[] _spawns;
    private List<Spawn> Spawns;
    #endregion

    #region Events
    public static event Action GameOver;
    public static event Action<bool> GamePaused;
    #endregion

    private void Awake()
    {
        Spawns = new List<Spawn>(_spawns.Length);
        for (int i = 0; i < _spawns.Length; i++) Spawns.Add(new Spawn(i, _spawns[i].position));
    }
    private void Start()
    {
        OnGameStarted();
    }

    private void OnGameStarted()
    {
        SpawnPlayer();
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();

        AudioManager.PlayAudioShot(AudioManager.GameAudio.LevelStart);
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
        Pause(true);
        UIManager.SetHealth(0);
        AudioManager.PlayAudioShot(AudioManager.GameAudio.GameOver);
        GameOver?.Invoke();
    }
    private void OnPlayerRecievedDamage(PlayerController player)
    {
        PlayerHealth = player.Health;
        UIManager.SetHealth(PlayerHealth);
        AudioManager.PlayAudioShot(AudioManager.TankAudio.Hit);
    }

    private Spawn GetRandomSpawn()
    {
        var index = UnityEngine.Random.Range(0, _spawns.Length);
        while (!Spawns[index].IsActive) index = UnityEngine.Random.Range(0, _spawns.Length);
        Spawns[index].SetActive(false);
        StartCoroutine(SpawnCoroutine(index));
        return Spawns[index];
    }
    private IEnumerator SpawnCoroutine(int index)
    {
        yield return new WaitForSeconds(5f);
        Spawns[index].SetActive(false);
    }

    private bool SpawnEnemy()
    {
        if (BotsCount >= _botsLimit) return false;

        var spawn = GetRandomSpawn();

        var tank = Instantiate(_enemyPrefab, spawn.Position, Quaternion.identity);
        tank.transform.SetParent(GameObject.Find("[UNITS]").transform);
        var component = tank.GetComponent<BaseTank>();

        component.Died += OnEnemyDied;
        component.RecievedDamage += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Hit);

        BotsCount++;
        return true;
    }
    private void SpawnPlayer()
    {
        var spawn = GetRandomSpawn();

        var tank = Instantiate(_playerPrefab, spawn.Position, Quaternion.identity);
        tank.transform.SetParent(GameObject.Find("[UNITS]").transform);
        var component = tank.GetComponent<PlayerController>();

        component.Died += OnPlayerDied;
        component.RecievedDamage += () => OnPlayerRecievedDamage(component);
        component.Fire += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Shoot);
        component.Pause += () => Pause(!Paused);
    }
    private void Pause(bool isPaused)
    {
        Paused = isPaused;
        GamePaused?.Invoke(isPaused);
        AudioManager.PlayAudioShot(AudioManager.GameAudio.Pause);
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    #region Unity Editor Buttons
    public void Unpause_UnityEditor() => Pause(false);
    public void Quit_UnityEditor()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    public void LoadLevel_UnityEditor()
    {
        if (SceneManager.sceneCount > 1 && SceneManager.GetSceneAt(0) != null) SceneManager.LoadScene(0);
    }
    public void Restart_UnityEditor()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        OnGameStarted();
    }
    #endregion
}
