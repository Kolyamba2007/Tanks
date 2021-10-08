using System;
using System.Collections;
using System.Collections.Generic;
using Tanks.Configuration;
using Tanks.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private GameConfig _gameConfig;

        [Header("Managers")]
        [SerializeField]
        private AudioManager AudioManager;
        [SerializeField]
        private UIManager UIManager;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField]
        private GameObject _playerPrefab;

        [Header("Game Options")]
        [SerializeField, Range(1f, 5f)]
        private float _enemySpawnInterval;

        #region Properties
        private PlayerController Player;
        private uint Score { set; get; } = 0;
        private byte BotsCount { set; get; }
        private bool Paused { set; get; } = false;
        private byte BotsLimit => _gameConfig.BotsLimit;
        public static byte PlayerHealth { private set; get; }
        #endregion

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
            AudioManager.PlayAudioShot(AudioManager.GameAudio.LevelStart);
        }
        private IEnumerator SpawnCoroutine()
        {
            while (!Player.Dead)
            {
                yield return new WaitForSeconds(_enemySpawnInterval);
                SpawnEnemy();
            }
        }

        private void OnGameStarted()
        {
            Player = SpawnPlayer();
            for (int i = 0; i < BotsLimit; i++) SpawnEnemy();
            StartCoroutine(SpawnCoroutine());
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
            if (BotsCount >= BotsLimit) return false;

            var spawn = GetRandomSpawn();

            var tank = Instantiate(_enemyPrefab, spawn.Position, Quaternion.identity);
            tank.transform.SetParent(GameObject.Find("[UNITS]").transform);
            var component = tank.GetComponent<BaseTank>();

            component.Died += OnEnemyDied;
            component.RecievedDamage += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Hit);

            BotsCount++;
            return true;
        }
        private PlayerController SpawnPlayer()
        {
            var spawn = GetRandomSpawn();

            var tank = Instantiate(_playerPrefab, spawn.Position, Quaternion.identity);
            tank.transform.SetParent(GameObject.Find("[UNITS]").transform);
            var component = tank.GetComponent<PlayerController>();

            component.Died += OnPlayerDied;
            component.RecievedDamage += () => OnPlayerRecievedDamage(component);
            component.Fire += () => AudioManager.PlayAudioShot(AudioManager.TankAudio.Shoot);
            component.Pause += () => Pause(!Paused);
            return component;
        }
        private void Pause(bool isPaused)
        {
            if (AudioManager == null) return;

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
        public void BackToMainMenu_UnityEditor()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            Time.timeScale = 1;
        }
        public void Restart_UnityEditor()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            OnGameStarted();
            Time.timeScale = 1;
        }
        #endregion
    }
}