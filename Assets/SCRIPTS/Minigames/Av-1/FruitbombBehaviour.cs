using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FruitbombBehaviour : MonoBehaviour
{
    Animator _myAnimator;
    Animator bombAnimator;
    NavMeshAgent _myAgent;
    GameObject _player;

    [SerializeField] bool _hasTarget;
    [SerializeField] Collider _triggerCol;
    [SerializeField] Collider _Col;
    [HideInInspector] public UnityEvent<FruitbombBehaviour> bombExplodedEvent;
    public bool selfActivated = false;
    public void Initialize(GameObject interactor)
    {
        _player = interactor;
    }

    public void InitiateTargeting()
    {
        _hasTarget = true;
    }

    Coroutine _coroutine;
    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAgent = GetComponent<NavMeshAgent>();
        _coroutine = StartCoroutine(Countdown());
    }

    void Update()
    {
        if (selfActivated) _player = CharacterManager.Instance.GetCurrentCharacter();
        if (_hasTarget)
            _myAgent.SetDestination(_player.transform.position);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PrepareToExplode();
        }
    }

    public float _timer;
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(_timer);
        PrepareToExplode();
    }

    void PrepareToExplode()
    {
        _myAgent.enabled = false;
        _hasTarget = false;
        _triggerCol.enabled = false;
        _Col.enabled = true;

        _myAnimator.Play("Windup");
    }

    public LayerMask _playerLM;
    public GameObject _explosionObject;
    public void Explode()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _myAnimator.Play("Explode");
        ExplosionDamage();
        bombExplodedEvent.Invoke(this);
    }

    public void ExplosionDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, _playerLM);

        foreach (Collider collider in hitColliders)
        {
            collider.GetComponent<HurtBox>().TakeDamage(2, gameObject, 15f);
        }
    }

    public void PlaceExplosion()
    {
        _explosionObject.transform.position = transform.position;
        _explosionObject.GetComponent<Animator>().Play("Exploding");
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}
