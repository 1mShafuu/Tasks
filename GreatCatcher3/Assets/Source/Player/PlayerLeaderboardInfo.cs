using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaderboardInfo
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    public PlayerLeaderboardInfo(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
