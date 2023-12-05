using UnityEngine;

public class StrongBoxBehaviour : EnemyBehaviour
{
    public Collider myCollider;
    public GameObject coinPrefab;

    private new void Start()
    {
        base.Start();
        unitKilled.AddListener(GiveCoin);
    }

    public override void TakeDamage(int damage)
    {
        _myAnimator.SetTrigger("Hit");
        base.TakeDamage(damage);
    }

    public void DisableCollider()
    {
        myCollider.enabled = false;
    }

    public override void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void GiveCoin()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity).GetComponentInChildren<CollectableBehaviour>().CollectCoin(InteractionController.Instance.transform.parent.GetComponentInChildren<CollectionManagerBehaviour>());
    }
}
