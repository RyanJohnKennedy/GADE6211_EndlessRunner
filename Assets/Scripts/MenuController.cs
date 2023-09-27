using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text scoreText;
    public Slider volumeSlider;
    public AudioSource music;
    private float soundVolume;
    public Button PlayButton;
    public Button highscoreButton;

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject LogInPanel;
    public GameObject HighscorePanel;

    public GameObject loginFailedText;
    public GameObject registeredCompletedText;

    public GameObject tankTop;

    [Header("Database")]
    public InputField userName;
    public InputField password;

    public GameObject logInPanel;
    public GameObject currentUserPanel;
    public Text currentUserText;

    public HighscoreTable hst;

    private string userNameString;
    private string passwordString;

    private DBManager dataBase;

    private void Start()
    {
        dataBase = DBManager.Instance;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        scoreText.text = "Score: " + GameManager.Instance.score;
        soundVolume = music.volume;
        volumeSlider.value = GameManager.Instance.volume;
    }

    private void Update()
    {
        RotateTank();
        CheckLogIn();
        GameManager.Instance.volume = volumeSlider.value;
        music.volume = soundVolume * GameManager.Instance.volume;

        if(currentUserPanel.activeSelf == true)
        {
            currentUserText.text = DBManager.Instance.currentUser.username;
        }
    }

    void CheckLogIn()
    {
        logInPanel.SetActive(!DBManager.Instance.logIn);
        highscoreButton.interactable = DBManager.Instance.logIn;
        currentUserPanel.SetActive(DBManager.Instance.logIn);

        if (dataBase.logIn)
        {
            PlayButton.GetComponentInChildren<Text>().text = "Play";
        }
        else
        {
            PlayButton.GetComponentInChildren<Text>().text = "Play Offline";
        }
    }

    public void StartGame()
    {
        GameManager.Instance.bossesDefeated = 0;
        GameManager.Instance.pickUpsCollected = 0;
        GameManager.Instance.score = 0;
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HighscoreToggle()
    {
        if (!HighscorePanel.activeSelf)
        {
            HighscorePanel.SetActive(true);
            hst.SetUpTable();
        }
        else
        {
            HighscorePanel.SetActive(false);
            hst.ClearList();
        }
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void RotateTank()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float midPoint = (tankTop.transform.position - Camera.main.transform.position).magnitude * 0.5f;
        Vector3 lookPoint = mouseRay.origin + mouseRay.direction * midPoint;

        tankTop.transform.LookAt(new Vector3(lookPoint.x, 92.4f, lookPoint.z));
    }

    public void LogIn()
    {
        if (userName.text != "" && password.text != "")
        {
            userNameString = userName.text;
            passwordString = password.text;
            userName.text = "";
            password.text = "";

            dataBase.LogIn(userNameString, passwordString);
        }
        else
        {
            Debug.Log("Enter valid username and password");
            loginFailedText.SetActive(true);
            registeredCompletedText.SetActive(false);
        }
    }

    public void Register()
    {
        if (userName.text != "" && password.text != "")
        {
            userNameString = userName.text;
            passwordString = password.text;

            dataBase.Register(userNameString, passwordString);
        }
        else
        {
            Debug.Log("Enter valid username and password");
            loginFailedText.SetActive(true);
            registeredCompletedText.SetActive(false);
        }
    }

    public void LogOut()
    {
        DBManager.Instance.logIn = false;
        DBManager.Instance.p = null;
        DBManager.Instance.currentUser = null;
        loginFailedText.SetActive(false);
        registeredCompletedText.SetActive(false);
    }
}
