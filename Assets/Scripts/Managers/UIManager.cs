using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text ScoreText;

    public void SetScore(uint value) => ScoreText.text = $"SCORE: {value}";
}
