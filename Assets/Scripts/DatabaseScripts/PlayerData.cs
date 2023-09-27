using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public string username;
    public string password;
    public int bestScore;
    public int pickUpsCollected;
    public int bossesDefeated;

    public PlayerData(string _user_name, string _password, int _bestScore, int _pickUpsCollected, int _bossesDefeated)
    {
        username = _user_name;
        password = _password;
        bestScore = _bestScore;
        pickUpsCollected = _pickUpsCollected;
        bossesDefeated = _bossesDefeated;
    }
}
