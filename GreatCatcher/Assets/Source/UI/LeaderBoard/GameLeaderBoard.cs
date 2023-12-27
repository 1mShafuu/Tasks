using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Random = UnityEngine.Random;

public class GameLeaderBoard : MonoBehaviour
{
    private const string LeaderBoardName = "Winnersboard";
    
    [SerializeField] private Game _game;
    [SerializeField] private LeaderBoardUI _leaderBoardUI;
    
    private List<PlayerLeaderboardInfo> _playersInLeaderBoard = new List<PlayerLeaderboardInfo>();
    
    public string PlayerName { get; private set; }

    public event Action PointsEnteredInTheTable;
    
    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _game.GameEnded -= OnGameEnded;
        _game.GameStarted -= OnGameStarted;
    }

    private void SetNewRecord()
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        if (!PlayerAccount.IsAuthorized)
            return;

        Leaderboard.GetPlayerEntry(LeaderBoardName, (result) =>
        {
            if (result == null)
                Debug.Log("Player is not present in the leaderboard.");
            else
                Debug.Log($"My rank = {result.rank}, score = {result.score}");
            
            if(result.score <= _game.ScoreToLeaderboard)
                Leaderboard.SetScore(LeaderBoardName, _game.ScoreToLeaderboard);
                
        });
#endif
        PointsEnteredInTheTable?.Invoke();

    }

    private void GetLeaderboardPlayersScore(bool test = false)
    {
        if (test)
        {
            for (int index = 0; index < 5; index++)
            {
                _playersInLeaderBoard.Add(new PlayerLeaderboardInfo("name", index * 1000000000));
            }
            
            _leaderBoardUI.ConstructLeaderBoard(_playersInLeaderBoard);
            return;
        }
        
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
        {
            _playersInLeaderBoard.Clear();
            Leaderboard.GetEntries(LeaderBoardName, (result) =>
            {
                Debug.Log($"My rank = {result.userRank}");
                
                int resultsAmount = result.entries.Length;

                resultsAmount = Mathf.Clamp(resultsAmount, 1, 5);

                for (int index = 0; index < resultsAmount; index++)
                {
                    string name = result.entries[index].player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymos";

                    int score = result.entries[index].score;
                    Debug.Log(name + " " + score);

                    _playersInLeaderBoard.Add(new PlayerLeaderboardInfo(name, score));
                }
                
                _leaderBoardUI.ConstructLeaderBoard(_playersInLeaderBoard);
            });
        }
#endif
    }
    
    private void OnGameEnded()
    {
        SetNewRecord();
        Debug.Log("--------------------------------");
#if UNITY_WEBGL && !UNITY_EDITOR
        GetLeaderboardPlayersScore();
#else
        GetLeaderboardPlayersScore(true);
#endif
        foreach (var player in _playersInLeaderBoard)
        {
            Debug.Log($"{player.Name}   {player.Score}");
        }
    }

    private void OnGameStarted()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized && YandexGamesSdk.IsInitialized)
        {
            GetLeaderboardPlayersScore();
        }
#else
        GetLeaderboardPlayersScore(true);
#endif
        foreach (var player in _playersInLeaderBoard)
        {
            Debug.Log($"{player.Name}   {player.Score}");
        }
    }
}
