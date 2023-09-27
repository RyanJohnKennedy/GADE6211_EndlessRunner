using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Text score;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        score.text = "Score: " + GameManager.Instance.score
            + "\nPickups Collected: " + GameManager.Instance.pickUpsCollected
            + "\nBosses Defeated: " + GameManager.Instance.bossesDefeated;
        
        this.GetComponent<AudioSource>().volume = this.GetComponent<AudioSource>().volume * GameManager.Instance.volume;
    }

    public void ReturnToMenu()
    {
        GameManager.Instance.finishedFirstLoop = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        GameManager.Instance.score = 0;
        SceneManager.LoadScene("Level1");
    }
}
