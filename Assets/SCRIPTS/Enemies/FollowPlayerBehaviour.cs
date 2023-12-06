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
    [SerializeField] NavMeshPath path;

    float elapsedStep;
    CharacterManager charManager;
    GameObject playerRef;
    public NavMeshAgent agent;

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
            id = 1;
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("Couldnt find second corner, corners length:" + path.corners.Length.ToString());
            id = 0;
        }
        Vector3 movementDiretion = path.corners[id] - transform.position;
        movementDiretion.y = transform.position.y;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, movementDiretion, singleStep * Time.deltaTime, maxMagnituteDelta);
        transform.rotation = Quaternion.LookRotation(newDirection);

        return path.corners[id];
    }

    private void FollowPlayer()
    {
        elapsedStep += Time.deltaTime;
        playerRef = charManager.GetCurrentCharacter();
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

        if (agent.remainingDistance < agent.stoppingDistance && Vector3.Angle(transform.forward, playerRef.transform.position - transform.position) < acceptableAngle)
        {
            playerInRangeEvent.Invoke();
            elapsedStep = 0f;
        }
    }
}
