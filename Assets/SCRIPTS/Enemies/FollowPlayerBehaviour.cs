using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class FollowPlayerBehaviour : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float singleStep;
    [SerializeField] float maxMagnituteDelta;
    [SerializeField] float acceptableAngle;
    [SerializeField] float stepTime;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] bool selfStopping;

    float elapsedStep;
    CharacterManager charManager;
    GameObject playerRef;
    
    NavMeshPath path;

    [HideInInspector] public UnityEvent playerInRangeEvent;

    private void Start()
    {
        charManager = CharacterManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        path = new NavMeshPath();
        enabled = false;
    }

    private void Update()
    {
        FollowPlayer();
    }

    public void SetSpeed(float newSpd)
    {
        agent.speed = newSpd;
    }

    public float GetSpeed()
    {
        return agent.speed;
    }

    public Vector3 AdjustLookDirection(NavMeshPath path)
    {
        int id;
        try
        {
            id = path.corners.Length-1;
            if (id < 0) throw new System.IndexOutOfRangeException("path not calculated");
        }
        catch (System.IndexOutOfRangeException)
        {
            return transform.position;
        }
        Vector3 movementDiretion = path.corners[id] - transform.position;
        movementDiretion.y = transform.position.y;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDiretion, singleStep * Time.deltaTime, maxMagnituteDelta);
        transform.rotation = Quaternion.LookRotation(newDirection);

        return path.corners[id];
    }

    private void FollowPlayer()
    {
        playerRef = charManager.GetCurrentCharacter();

        if (agent.remainingDistance < agent.stoppingDistance && Vector3.Angle(transform.forward, playerRef.transform.position - transform.position) < acceptableAngle)
        {
            playerInRangeEvent.Invoke();
            elapsedStep = 0f;
            if (selfStopping) return;
        }

        elapsedStep += Time.deltaTime;
        
        if (elapsedStep > stepTime)
        {
            elapsedStep = 0f;
            agent.CalculatePath(playerRef.transform.position, path);
        }

        if (path.corners.Length == 0) return;

        agent.SetDestination(playerRef.transform.position);
        agent.Move((agent.speed * Time.deltaTime * transform.forward) * curve.Evaluate(elapsedStep / stepTime));

        AdjustLookDirection(path);

        Debug.DrawRay(transform.position, playerRef.transform.position - transform.position,Color.blue);
    }
}
