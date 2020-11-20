using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private const string HAS_PLAYED = "HasPlayed";
    private const string HIGHSCORE = "Highscore";
    
    [SerializeField] private Text[] scoreTexts;
    [SerializeField] private Text[] highScoreTexts;
    [SerializeField] private GameObject newRecord;

    private int score = 0;
    private int highScore = 0;
    
    private void Awake()
    {
        int hasPlayed = PlayerPrefs.GetInt(HAS_PLAYED);
 
        if( hasPlayed == 0 )
        {
            PlayerPrefs.SetInt(HIGHSCORE, highScore);
            PlayerPrefs.SetInt( HAS_PLAYED, 1 );
            PlayerPrefs.Save();
        }
        else
        {
            highScore = PlayerPrefs.GetInt(HIGHSCORE);
            UpdateHighScoreText();
        }
    }

    public void AddScore(int count)
    {
        score += count;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HIGHSCORE, highScore);
            PlayerPrefs.Save();
            newRecord.SetActive(true);
            UpdateHighScoreText();
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreTexts != null)
        {
            foreach (Text scoreText in scoreTexts)
            {
                scoreText.text = score.ToString();
            }
        }
    }

    private void UpdateHighScoreText()
    {
        if (highScoreTexts != null)
        {
            foreach (var highScoreText in highScoreTexts)
            {
                highScoreText.text = highScore.ToString();
            }
        }
    }
}
