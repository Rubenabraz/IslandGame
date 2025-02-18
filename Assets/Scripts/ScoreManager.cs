using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; 
    public int score = 0;
    public Text scoreText; 

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
        if (scoreText == null)
        {
            GameObject scoreTextObj = GameObject.FindGameObjectWithTag("scoreText");
            if (scoreTextObj != null)
            {
                scoreText = scoreTextObj.GetComponent<Text>();
            }
            else
            {
                Debug.LogWarning("Não foi encontrado objeto com a tag 'scoreText'");
            }
        }
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        Debug.Log("Score: " + score);
    }

 
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("ScoreText não foi atribuído no ScoreManager!");
        }
    }
}
