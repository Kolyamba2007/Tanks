using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text ScoreText;
    [SerializeField]
    private RectTransform HealthPanel;

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
}
