using TMPro;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _text;
    
    private Wallet _wallet;

    public Wallet Wallet => _wallet;
    
    private void Awake()
    {
        _wallet = _player.GetComponent<Wallet>();
    }

    private void OnEnable()
    {
        _wallet.BalanceChanged += OnBalanceChanged;
    }

    private void OnDisable()
    {
        _wallet.BalanceChanged -= OnBalanceChanged;
    }

    private void OnBalanceChanged(int value)
    {
        var currentBalance = _wallet.Money;
        _text.text = currentBalance.ToString();
    }
}
