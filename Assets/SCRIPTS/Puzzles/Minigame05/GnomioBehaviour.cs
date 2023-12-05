using UnityEngine;
using UnityEngine.AI;

public class GnomioBehaviour : InteractableObject
{
    enum GnomioState
    {
        Following,
        Idle,
        Waiting
    }

    [SerializeField] bool follower = false;
    [SerializeField] Sprite gnomioSprite;
    Animator m_Anim;
    SphereCollider nearbyArea;
    GnomioState curState;
    NavMeshAgent agent;
    NavMeshPath path;

    private new void Start()
    {
        m_Anim = GetComponent<Animator>();
        nearbyArea = GetComponent<SphereCollider>();

        if (!follower)
        {
            enabled = false;
            nearbyArea.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        playerRef = CharacterManager.Instance.GetCurrentCharacter();
        agent.SetDestination(playerRef.transform.position);

        path = new NavMeshPath();
        curState = GnomioState.Waiting;
    }

    [SerializeField] FMODUnity.EventReference _gnomioSound;
    public override void Interact(InteractionController interactor)
    {
        CanvasBehaviour.Instance.SetActiveTempText("Coloque as pilhas de livros no portal correto.", 2f, gnomioSprite, _gnomioSound);
    }

    private void Update()
    {
        switch (curState)
        {
            case GnomioState.Following:
                FollowingBehaviour();
                break;
            case GnomioState.Idle:
                IdleBehaviour();
                break;
            case GnomioState.Waiting:
                break;
        }
    }

    private void IdleBehaviour()
    {
        playerRef = CharacterManager.Instance.GetCurrentCharacter();
        agent.SetDestination(playerRef.transform.position);

        if (agent.remainingDistance > agent.stoppingDistance * 1.5f)
        {
            curState = GnomioState.Following;
            agent.CalculatePath(playerRef.transform.position, path);
        }
    }

    GameObject playerRef;
    float elapsedStep;
    [SerializeField] float updatePathDelay;
    [SerializeField] float singleStep;
    [SerializeField] float maxMagnituteDelta;
    [SerializeField] float baseSpeed;
    [SerializeField] float maxDistanceForSpeed;
    private void FollowingBehaviour()
    {
        playerRef = CharacterManager.Instance.GetCurrentCharacter();
        agent.SetDestination(playerRef.transform.position);
        elapsedStep += Time.deltaTime;
        agent.Move(agent.speed * Time.deltaTime * transform.forward);
        if (elapsedStep > updatePathDelay)
        {
            elapsedStep = 0f;
            agent.CalculatePath(playerRef.transform.position, path);
        }

        if(path.corners.Length < 1)
        {
            Debug.Log("Path not found");
            return;
        }

        Vector3 movementDiretion = path.corners[1] - transform.position;
        movementDiretion.y = transform.position.y;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDiretion, (singleStep * Mathf.Lerp(0, 1f, agent.remainingDistance / maxDistanceForSpeed)) * Time.deltaTime, maxMagnituteDelta);
        transform.rotation = Quaternion.LookRotation(newDirection);

        agent.speed = baseSpeed * Mathf.Lerp(0.2f,1f,agent.remainingDistance/maxDistanceForSpeed);
        m_Anim.SetFloat("Speed", agent.speed);

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            EnterIdleState();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterIdleState();
        }
    }

    public Sprite GetGnomioSprite()
    {
        return gnomioSprite;
    }

    private void EnterIdleState()
    {
        curState = GnomioState.Idle;
        elapsedStep = 0f;
        m_Anim.SetFloat("Speed", 0);
        nearbyArea.enabled = true;
    }

    public void PlayCorrectAnimation()
    {
        m_Anim.SetTrigger("Correct");
    }
    
    public void PlayWrongAnimation()
    {
        m_Anim.SetTrigger("Wrong");
    }
}