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
    
    void Start()
    {
        ResetLives();
    }

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

    public void WinGame()
    {
        statusText.text = "You Win! Power-Up Earned!";
        //ResetLives();
    }

    public void GameOver()
    {
        statusText.text = "Game Over!";
        //ResetLives();
    }

     public void RestartGame()
    {
        ResetLives();
        statusText.text = "Game Restarted!";
        statusText.text = "";
    }
}
