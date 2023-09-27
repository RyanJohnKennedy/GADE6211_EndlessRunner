using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;

    List<GameObject> objects = new List<GameObject>();

    DBManager dataBase;

    private void Awake()
    {
        //SetUpTable();
        //ClearList();
    }
    private void Start()
    {
        
    }

    public void SetUpTable()
    {
        dataBase = DBManager.Instance;
        dataBase.GetPlayers();
        entryTemplate.gameObject.SetActive(false);

        float _templateHeight = 60f;
        for (int i = 0; i < dataBase.players.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -_templateHeight * i);
            entryTransform.gameObject.SetActive(true);
            objects.Add(entryTransform.gameObject);

            int rank = i + 1;
            string rankString;
            switch (rank)
            {
                default: rankString = rank + "TH"; break;
                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }

            entryTransform.Find("PosText").GetComponent<Text>().text = rankString;
            entryTransform.Find("NameText").GetComponent<Text>().text = DBManager.Instance.players[i].username;
            entryTransform.Find("ScoreText").GetComponent<Text>().text = DBManager.Instance.players[i].bestScore.ToString();
            entryTransform.Find("PickUpsText").GetComponent<Text>().text = DBManager.Instance.players[i].pickUpsCollected.ToString();
            entryTransform.Find("BossesText").GetComponent<Text>().text = DBManager.Instance.players[i].bossesDefeated.ToString();
        }
    }

    public void ClearList()
    {
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }
}
