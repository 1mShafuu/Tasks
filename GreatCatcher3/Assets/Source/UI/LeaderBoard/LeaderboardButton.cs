using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardButton : OpenMenuButton
{
    [SerializeField] private LeaderboardMenu _leaderboardMenu;
    
    protected override void OnButtonClicked()
    {
        _leaderboardMenu.Open();   
    }
}
