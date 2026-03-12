using Economy;
using TMPro;
using UnityEngine;
using Zenject;

public class CoinValueView : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;

    private CoinWallet _wallet;

    [Inject]
    private void Construct(CoinWallet wallet)
    {
        _wallet = wallet;
        TrySubscribe();
    }

    private void Awake()
    {
        if (valueText == null)
        {
            valueText = GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        if (_wallet != null)
        {
            _wallet.CoinsChanged -= OnCoinsChanged;
        }
    }

    private void OnCoinsChanged(int coins)
    {
        if (valueText != null)
        {
            valueText.text = coins.ToString();
        }
    }

    private void TrySubscribe()
    {
        if (_wallet == null || !isActiveAndEnabled)
        {
            return;
        }

        _wallet.CoinsChanged -= OnCoinsChanged;
        _wallet.CoinsChanged += OnCoinsChanged;
        OnCoinsChanged(_wallet.Coins);
    }
}
