using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseImage; 
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;  
                isPaused = true;
                pauseImage.SetActive(true);  
            }
            else
            {
                Time.timeScale = 1f;  
                isPaused = false;
                pauseImage.SetActive(false);
            }
        }
    }
}