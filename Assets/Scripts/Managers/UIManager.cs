using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Serializable]
    private struct TextProperties
    {
        public TMP_FontAsset Font;
        public int FontSize;
        public Color Color;
    }

    [SerializeField]
    private TMP_Text ScoreText;
    [SerializeField]
    private RectTransform HealthPanel;

    [Header("Text Properties")]
    [SerializeField]
    private Transform TextRoot;
    [SerializeField]
    private TextProperties Properties;

    [Header("Game Menu")]
    [SerializeField]
    private GameObject GameMenu;
    [SerializeField]
    private Button CloseButton;
    [SerializeField]
    private TMP_Text MenuLabel;

    private void Awake()
    {
        GameManager.GameOver += OnGameOver;
        GameManager.GamePaused += (isPaused) => ShowMenu(isPaused);
    }
    
    private void OnGameOver()
    {
        MenuLabel.text = "GAME OVER";
        CloseButton.interactable = false;
    }

    public void SetScore(uint value) => ScoreText.text = $"SCORE: {value}";
    public void SetHealth(byte value)
    {
        var count = HealthPanel.childCount;
        var diff = Math.Abs(count - value);

        if (value > count)
        {
            for (int i = 0; i < diff; i++)
            {
                var heart = Resources.Load("\\Prefabs\\Heart") as GameObject;
                heart.transform.SetParent(HealthPanel);
            }
        }
        else
        {
            for (int i = 0; i < diff; i++) Destroy(HealthPanel.GetChild(count - 1).gameObject);
        } 
    }

    private void ShowText(string text, float time = 3f)
    {
        StartCoroutine(ShowTextCoroutine(text, time));
    }
    private IEnumerator ShowTextCoroutine(string text, float time)
    {
        GameObject textObj = new GameObject($"[TEXT] {text}");
        textObj.AddComponent<RectTransform>();
        textObj.AddComponent<TextMeshPro>().text = text;
        textObj.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        textObj.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

        var component = textObj.GetComponent<TextMeshPro>();
        component.font = Properties.Font;
        component.fontSize = Properties.FontSize;
        Color color = Properties.Color;
        color.a = 0f;
        component.color = color;
        textObj.transform.SetParent(TextRoot);

        float factor = 0f;
        Color startColor = component.color;
        Color endColor = startColor;
        endColor.a = 1f;
        while (factor < 1f)
        {
            component.color = Color.Lerp(startColor, endColor, factor += Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(time);

        factor = 0f;
        startColor = component.color;
        endColor = startColor;
        endColor.a = 0f;
        while (factor < 1f)
        {
            component.color = Color.Lerp(startColor, endColor, factor += Time.deltaTime);
            yield return null;
        }
        Destroy(textObj);
    }

    private void ShowMenu(bool isActive)
    {
        GameMenu.SetActive(isActive);
    }
}
