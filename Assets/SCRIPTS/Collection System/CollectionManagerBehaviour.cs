using UnityEngine;

public class CollectionManagerBehaviour : MonoBehaviour
{
    public HealthManager hpManager;
    public CoinsManager coinsManager;

    private void Start()
    {
        hpManager = GetComponentInParent<HealthManager>();
        coinsManager = FindObjectOfType<CoinsManager>();
    }

    public void CollectItem(string collectableType, CollectableBehaviour collectable)
    {
        switch (collectableType)
        {
            case "Heart":
                CollectedHeart();
                break;
            case "Coin":
                CollectedCoin(collectable);
                break;
        }
    }

    private void CollectedHeart()
    {
        Debug.Log("Coracao adquirido");
        hpManager.HealDamage();
    }
    
    private void CollectedCoin(CollectableBehaviour collectable)
    {
        Debug.Log("Moeda +1");
        coinsManager.CoinCollected(collectable);
    }
}