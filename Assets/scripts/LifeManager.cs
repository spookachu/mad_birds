using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int totalLives = 3;
    public int currentLives;

    public GameObject projectilePrefab; 
    public Transform livesContainer; 
    private GameObject[] lifeIcons;
    public Text statusText;
    
    /// <summary>
    /// Called when the script is first initialized. It calls the 
    // ResetLives() function to initialize and display the life count
    // at the start of the game.
    /// </summary>
    void Start()
    {
        ResetLives();
    }

    /// <summary>
    /// Creates the life count and the in-game visual objects to indicate
    /// the amount of lives left.
    /// </summary>
    private void ResetLives()
    {
        currentLives = totalLives;
        lifeIcons = new GameObject[currentLives];

        foreach (Transform child in livesContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < currentLives; i++)
        {
            lifeIcons[i] = Instantiate(projectilePrefab, livesContainer);
            lifeIcons[i].transform.position = livesContainer.position + new Vector3(0, i* 0.2f, 0);

            lifeIcons[i].transform.eulerAngles = new Vector3(
                lifeIcons[i].transform.eulerAngles.x,
                lifeIcons[i].transform.eulerAngles.y + 180,
                lifeIcons[i].transform.eulerAngles.z
            );
            lifeIcons[i].transform.localScale = Vector3.one * 20.0f; 
        }
        statusText.text = ""; 
    }

    /// <summary>
    /// Reduces the number of remaining lives by 1 and destroyes a 
    //  life icon, checks if the player has lost all lives, in which 
    //  case it triggers the GameOver() function.
    /// </summary>
    public void UseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            Destroy(lifeIcons[currentLives]);

            if (currentLives == 0){
                GameOver();
            }
        }
    }

    /// <summary>
    /// Updates game screen to announce win.
    /// </summary>
    public void WinGame()
    {
        currentLives = 3;
        statusText.text = "Success!";
        statusText.text = "";
        //ResetLives();
    }

    /// <summary>
    /// Updates game screen to announce loss.
    /// </summary>
    public void GameOver()
    {
        statusText.text = "Game Over!";
        //ResetLives();
    }

    /// <summary>
    /// Resets the game by restoring the lives and life icons, 
    // and updates the status text 
    /// </summary>
     public void RestartGame()
    {
        ResetLives();
        statusText.text = "Game Restarted!";
        statusText.text = "";
    }
}
