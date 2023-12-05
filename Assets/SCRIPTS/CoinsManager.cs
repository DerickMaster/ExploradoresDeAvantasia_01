using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CoinCollectedEvent : UnityEvent<string, int>{}

public class CoinsManager : MonoBehaviour
{
    [SerializeField] private int CoinsValue;

    public UnityEvent<int> coinCollected;

    public void CoinCollected(CollectableBehaviour coin)
    {
        string coinType = coin.subType;

        switch (coinType)
        {
            case "Bronze":
                CoinsValue += 1;
                break;
            case "Silver":
                CoinsValue += 10;
                break;
            case "Gold":
                CoinsValue += 100;
                break;
        }

        coinCollected.Invoke(CoinsValue);
    }

    public void SetCoinsAmount(int amount)
    {
        CoinsValue = amount;
        coinCollected.Invoke(CoinsValue);
    }

    public int GetCoinsAmount()
    {
        return CoinsValue;
    }
}
