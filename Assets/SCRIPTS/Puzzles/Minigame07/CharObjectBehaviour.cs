using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;

public class CharObjectBehaviour : EnemyBehaviour
{
    NavMeshAgent m_Agent;
    NavMeshObstacle[] spots;
    MeshFilter letterMesh;

    public string myChar = "0";
    public bool isLetter = false;
    [SerializeField] private GameObject explosion;
    [HideInInspector] public UnityEvent<CharObjectBehaviour> refUnitKilled;

    private void Awake()
    {
        //textMesh = GetComponentInChildren<TextMeshPro>();
        letterMesh = GetComponentInChildren<MeshFilter>();
    }

    private new void Start()
    {
        base.Start();

        _myAnimator = GetComponentInChildren<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        spots = transform.root.GetComponentsInChildren<NavMeshObstacle>();
        m_Agent.SetDestination(GetRandomSpot().transform.position);

        unitKilled.AddListener(DeathEvent);
    }

    private void Update()
    {
        if(m_Agent.remainingDistance < 2.2f)
        {
            m_Agent.SetDestination(GetRandomSpot().transform.position);
        }
    }

    private void LateUpdate()
    {
        letterMesh.transform.parent.rotation = Quaternion.identity;
        //textMesh.transform.parent.rotation = Quaternion.identity;
    }

    private NavMeshObstacle GetRandomSpot()
    {
        return spots[Random.Range(0, spots.Length)];
    }

    public void UpdateChar(string newChar, Mesh newMesh)
    {
        myChar = newChar;
        letterMesh.mesh = newMesh;
    }

    private void DeathEvent()
    {
        _myAnimator.SetTrigger("Death");
        StartCoroutine(PlaceSmoke());
        if (!isLetter) Explode();

        enabled = false;
        refUnitKilled.Invoke(this);

        Invoke(nameof(DestroyMe), 0.3f);
    }

    [SerializeField]GameObject _smokeBreak;
    IEnumerator PlaceSmoke()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(_smokeBreak, transform.position, Quaternion.identity);
    }

    private void Explode()
    {
        ExplosionBehaviour instance = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<ExplosionBehaviour>();
        instance.PlayExplosion(2, 5f);
    }
}
