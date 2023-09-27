using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance = null;
    public string baseURL = "";

    public bool AttemptlogIn = false;
    public bool logIn = false;
    bool EntryExists = false;
    bool updateingData = false;

    public List<PlayerData> players = new List<PlayerData>();

    public PlayerData p;

    public PlayerData currentUser;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogIn(string userName, string password)
    {
        AttemptlogIn = true;
        p = new PlayerData(userName, password, 0, 0, 0);
        StartCoroutine(FetchObjetcsAPIcall());
    }

    public void UpdatePlayerData()
    {
        updateingData = true;
        AttemptlogIn = false;
        EntryExists = true;
        StartCoroutine(FetchObjetcsAPIcall());
    }

    public void Register(string userName, string password)
    {
        EntryExists = false;
        AttemptlogIn = false;
        updateingData = false;
        p = new PlayerData(userName, password, 0, 0, 0);

        StartCoroutine(FetchObjetcsAPIcall());
    }

    public void GetPlayers()
    {
        AttemptlogIn = false;
        EntryExists = true;
        updateingData = false;
        StartCoroutine(FetchObjetcsAPIcall());
    }

    IEnumerator FetchObjetcsAPIcall()
    {

        UnityWebRequest _www = UnityWebRequest.Get(baseURL + "PlayerData.json");

        yield return _www.SendWebRequest();
        if (_www.isNetworkError || _www.isHttpError)
        {
            Debug.Log(_www.error);
        }
        else
        {
            Debug.Log("Found Database");

            ParseFetchObject(_www.downloadHandler.text);
        }
    }



    IEnumerator AddObjetcsAPIcall(PlayerData data)
    {
        UnityWebRequest _www = new UnityWebRequest(baseURL + "PlayerData.json", "POST");

        byte[] _body = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

        _www.uploadHandler = new UploadHandlerRaw(_body);
        _www.downloadHandler = new DownloadHandlerBuffer();

        _www.SetRequestHeader("Content-Type", "application/json");

        yield return _www.SendWebRequest();

        if (_www.isNetworkError || _www.isHttpError)
        {
            Debug.Log(_www.error);
        }
        else
        {
            Debug.Log("Object added to database");

            if(!EntryExists)
            { 
                currentUser = new PlayerData(p.username, p.password, 0, 0, 0);
            }
            logIn = true;
        }
    }

    IEnumerator UpdateObjetcsAPIcall(string playerID)
    {
        UnityWebRequest _www = UnityWebRequest.Delete(baseURL + "PlayerData/" + playerID + ".json");

        yield return _www.SendWebRequest();

        StartCoroutine(AddObjetcsAPIcall(currentUser));
    }

    void ParseFetchObject(string jsonString)
    {
        players.Clear();
        int counter = 0;
        JSONNode rootNode = JSON.Parse(jsonString);
        string[] node = jsonString.Split('"');

        foreach (JSONNode field in rootNode)
        {
            string _user_name = field["username"];
            string _password = field["password"];
            int _bestScore = field["bestScore"];
            int _pickUpsCollected = field["pickUpsCollected"];
            int _bossesDefeated = field["bossesDefeated"];

            players.Add(new PlayerData(_user_name, _password, _bestScore, _pickUpsCollected, _bossesDefeated));

            if (updateingData && _user_name.ToLower() == currentUser.username.ToLower())
            {
                int i = 1;
                if(counter == 0)
                {
                    i = 1;
                }
                else
                {
                    i = (counter * 16) + 1;
                }

                StartCoroutine(UpdateObjetcsAPIcall(node[i]));
            }

            if (_user_name.ToLower() == p.username.ToLower())
            {
                if (AttemptlogIn && _password == p.password)
                {
                    logIn = true;
                    currentUser = new PlayerData(_user_name, _password, _bestScore, _pickUpsCollected, _bossesDefeated);
                    p.username = "";
                    p.password = "";
                }

                EntryExists = true;
            }
            counter++;
        }

        if (EntryExists == false && AttemptlogIn == false)
        {
            StartCoroutine(AddObjetcsAPIcall(p));
        }

        SortPlayers();
    }

    private void SortPlayers()
    {
        for (int i = players.Count - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if (players[j].bestScore < players[j + 1].bestScore)
                {
                    PlayerData temp;

                    temp = players[j];
                    players[j] = players[i];
                    players[i] = temp;
                }
            }
        }
    }
}
