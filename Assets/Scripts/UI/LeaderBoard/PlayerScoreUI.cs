using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI position;
    [SerializeField] private Image positionImage;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Image scoreImage;
    
    
    public void SetPlayerScore(int newPosition, string newPlayerName, int newScore)
    {
        position.text = newPosition.ToString();
        playerName.text = newPlayerName;
        score.text = newScore.ToString();
    }

    public void UpdateScoreAndPosition(int newPosition, int newScore)
    {
        position.text = newPosition.ToString();
        score.text = newScore.ToString();
    }
    public void UpdateColor(Color color)
    {
        positionImage.color = color;
        scoreImage.color = color;
    }
}
