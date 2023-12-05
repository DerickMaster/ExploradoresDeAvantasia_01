using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UICoinsManager : MonoBehaviour
{

    private UICoinCounter[] coinCounters;
    public CoinsManager coinsManager;

    [SerializeField] private float slideDuration;
    [SerializeField] private float onScreenDuration;
    private void Start()
    {
        try
        {
            coinsManager = FindObjectOfType<CoinsManager>();
            coinsManager.coinCollected.AddListener(UpdateCoinAmount);
            coinCounters = GetComponentsInChildren<UICoinCounter>();
            coinCounters = coinCounters.Reverse().ToArray();
            foreach (UICoinCounter coinCounter in coinCounters)
            {
                coinCounter.SetDurations(slideDuration, onScreenDuration);
            }
        }
        catch
        {
            Debug.Log("CoinManager not Found");
        }
    }

    public void UpdateCoinAmount(int newAmount)
    {
        string amountString = newAmount.ToString("000");

        for (int i = 0; i < coinCounters.Length; i++)
        {
            if (!coinCounters[i].amount.Equals(amountString[i]))
            {
                coinCounters[i].UpdateAmount(amountString[i]);
            }
        }
    }

    public Text GetCoinValue(int coinType)
    {
        return coinCounters[coinType - 1].m_Text;
    }
}
