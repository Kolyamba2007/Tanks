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

        #region UnityEditor
        public void BotsLimitChanged_UnityEditor()
        {
            if (BotsLimitText.text.IsNullOrEmpty()) return;
            string limit = BotsLimitText.text;
            int index = limit.IndexOf((char)8203);
            while (index >= 0)
            {
                limit = limit.Remove(index, 1);
                index = limit.IndexOf((char)8203);
            }
            byte value;
            if (!byte.TryParse(limit, out value)) BotsLimitText.text = "0";
            else
            {
                int a = 0;
            }
        }
        public void StartGame_UnityEditor()
        {
            if (BotsLimitText.text.IsNullOrEmpty())
            {
                EditorExtensions.LogError("Bots amount can't be null/empty or zero!", EditorExtensions.EditorMessageType.Game);
                return;
            }

            string limit = BotsLimitText.text;
            int index = limit.IndexOf((char)8203);
            while (index >= 0)
            {
                limit = limit.Remove(index, 1);
                index = limit.IndexOf((char)8203);
            }
            byte value;
            if (byte.TryParse(limit, out value))
            {
                _gameConfig.SetBotsLimit(value);
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            else
            {
#if UNITY_EDITOR
                Editor.EditorExtensions.Log("Bots limit must be an integer value.", EditorExtensions.EditorMessageType.Game);
#endif
            }
        }
        public void Quit_UnityEditor()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        #endregion
    }
}