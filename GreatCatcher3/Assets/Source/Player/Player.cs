using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int MaxLevel = 3;
    
    [SerializeField] private GameObject _prarieEnterance;
    [SerializeField] private PlayerInfoHolder _playerInfoHolder;
    
    private int _level = 1;
    private PlayerUpgrader _upgrader;
    
    public int Level => _level;

    public event Action LevelChanged;
    
    private void Awake()
    {
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), _prarieEnterance.GetComponent<BoxCollider>(), true);
    }

    private void OnEnable()
    {
        _playerInfoHolder.AddFieldChangedCallback(OnStatsGained);
    }

    private void OnDisable()
    {
        if (_upgrader != null)
        {
            _upgrader.LevelIncreased -= OnLevelChanged;
        }
    }

    public void InitUpgrader(PlayerUpgrader upgrader)
    {
        _upgrader = upgrader.GetComponent<PlayerUpgrader>();
        _upgrader.LevelIncreased += OnLevelChanged;
    }
    
    private void OnLevelChanged()
    {
        if (_level >= MaxLevel) return;
        _level++;
        LevelChanged?.Invoke();
    }

    private void OnStatsGained()
    {
        if (_playerInfoHolder.PlayerInfoStats.Level > 0)
        {
            _level = _playerInfoHolder.PlayerInfoStats.Level;
        }
    }
}
