using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tanks.Configuration;
using Tanks.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tanks.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private GameConfig _gameConfig;

        [Space, SerializeField]
        private TMP_Text BotsLimitText;

        private void Start()
        {
            BotsLimitText.text = "5";
        }
        public void StartGame_UnityEditor()
        {
            if (BotsLimitText.text.IsNullOrEmpty())
            {
                EditorExtensioins.LogError("Bots amount can't be null/empty or zero!", EditorExtensioins.EditorMessageType.Game);
                return;
            }

            string limit = BotsLimitText.text;
            int index = limit.IndexOf((char)8203);
            while (index >= 0)
            {
                limit = limit.Remove(index, 1);
                index = limit.IndexOf((char)8203);
            }
            _gameConfig.SetBotsLimit(byte.Parse(limit));
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        public void Quit_UnityEditor()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}