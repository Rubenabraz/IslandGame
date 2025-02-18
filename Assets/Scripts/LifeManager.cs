using UnityEngine;
using UnityEngine.UI; 

public class LifeManager : MonoBehaviour
{
    public int lives = 3; 
    public Text livesText; 

    void Start()
    {
        UpdateLivesText(); 
    }

    public void DecreaseLives()
    {
        lives--; 
        UpdateLivesText(); 
    }

    public void IncreaseLives(int amount)
    {
        lives += amount; 
        UpdateLivesText(); 
    }

    
    void UpdateLivesText()
    {
        livesText.text = "Vidas: " + lives;
    }

    public int GetLives()
    {
        return lives;
    }
}
