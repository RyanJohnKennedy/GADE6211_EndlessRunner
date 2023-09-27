using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject menu;
    public GameObject settings;
    public Slider volumeSlider;
    public AudioSource music;
    public LevelController lc;
    private float soundVolume;

    // Start is called before the first frame update
    void Start()
    {
        lc = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        soundVolume = music.volume;
        volumeSlider.value = GameManager.Instance.volume;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.volume = volumeSlider.value;
        music.volume = soundVolume * GameManager.Instance.volume;
    }

    public void Settings()
    {
        settings.SetActive(true);
        menu.SetActive(false);
    }

    public void Back()
    {
        settings.SetActive(false);
        menu.SetActive(true);
    }

    public void ReturnToMenu()
    {
        ResumeGame();
        EventManager.Instance.PostNotification(EVENT_TYPE.UpdateDatabase, this);
        SceneManager.LoadScene("MainMenu");
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
