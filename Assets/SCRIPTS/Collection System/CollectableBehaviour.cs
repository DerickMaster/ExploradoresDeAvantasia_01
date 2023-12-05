using UnityEngine;
using UnityEngine.Events;

public class CollectableBehaviour : MonoBehaviour
{
    public string collectableType;
    public string subType;
    public bool Collected = false;
    public bool selfDestroy = true;
    public Animator _myAnim;
    [HideInInspector] public UnityEvent _collectedEvent;

    private void Start()
    {
        _myAnim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectCoin(other.gameObject.GetComponentInChildren<CollectionManagerBehaviour>());
    }

    public void CollectCoin(CollectionManagerBehaviour colManager)
    {
        _collectedEvent.Invoke();
        if (!Collected)
        {
            colManager.CollectItem(collectableType, this);
            _myAnim.SetBool("Collected", true);
            Invoke(nameof(ObjectCollected), 2f);
        }
    }

    protected virtual void ObjectCollected()
    {
        Collected = true;
        if (selfDestroy) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}