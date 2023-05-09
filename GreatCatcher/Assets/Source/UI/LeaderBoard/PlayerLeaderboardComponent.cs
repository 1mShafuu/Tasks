using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLeaderboardComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerPlace;
    [SerializeField] private TMP_Text _playerNickName;
    [SerializeField] private TMP_Text _playerScore;
    
    public void Initialize(string place, string nickname, int score)
    {
        _playerPlace.text = place;
        _playerNickName.text = nickname;
        _playerScore.text = score.ToString();
    }
}
