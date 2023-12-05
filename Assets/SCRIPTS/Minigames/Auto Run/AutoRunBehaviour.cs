using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutoRunBehaviour : MonoBehaviour
{
    CharacterManager cm;
    StarterAssets.StarterAssetsInputs _input;
    Vector3 origin;
    ITrap[] traps;
    List<GameObject> spawnedTraps;

    [SerializeField] GameObject block;
    [SerializeField] GameObject[] trapsPrefab;
    [SerializeField] Animator bossAnimator;

    // Start is called before the first frame update
    void Start()
    {
        cm = FindObjectOfType<CharacterManager>();
        origin = cm.transform.position;
        enabled = false;
        traps = FindObjectsOfType<MonoBehaviour>().OfType<ITrap>().ToArray();
        foreach(ITrap trap in traps)
        {
            trap.GetTrapEvent().AddListener(StopMovement);
            trap.GetTrapFinishedEvent().AddListener(ResumeMovement);
        }
        spawnedTraps = new List<GameObject>();
    }

    [ContextMenu("Start running")]
    public void StarRun()
    {
        _input = cm.GetCurrentCharacter().GetComponent<StarterAssets.StarterAssetsInputs>();
        enabled = true;
        bossOffset = initialBossOffset;
    }

    [SerializeField] float bossSpeed;
    void Update()
    {
        CheckSpawnTraps();
        MoveChar();
        MoveBoss();
        BossAttack();
    }

    [SerializeField][Range(-1f,1f)] float inputYOverwrite;
    private float accumulatedDist = 0f;
    private void MoveChar()
    {
        _input.move.y = inputYOverwrite;
        //accumulatedDist += (_input.moveDirection * bossSpeed * Time.deltaTime).magnitude;
        if (_input.transform.position.z >= trapDistance * 2.5f)
        {
            accumulatedDist += _input.transform.position.z;
            ResetCharPos();
        }
    }

    [SerializeField] Vector3 initialBossOffset;
    Vector3 bossOffset;
    int trapsTriggered = 0;
    private void MoveBoss() 
    {
        if (!bossAnimator) return;

        bossAnimator.gameObject.transform.position = _input.transform.position + bossOffset;
    }

    [SerializeField] GameObject bombSpawnPrefab;
    [SerializeField] float attackDistance;
    bool attacked = false;
    private void BossAttack()
    {
        if (!bossAnimator) return;

        if(!attacked && dist % attackDistance < 1f && !(Mathf.Abs(dist - lastTrapDist) < 35f ))
        {
            bossAnimator.SetTrigger("Shoot");
            Vector3 spawnPos = _input.transform.position + bossAnimator.transform.forward * 17f;
            //CanvasBehaviour.Instance.SetActiveTempText("LA VEM BOMBAAAAAAAAA");
            Instantiate(bombSpawnPrefab, spawnPos, Quaternion.identity).GetComponent<BombSpawn>().Initialize(1f);
            StartCoroutine(AttackDelay());
        }
        
    }

    IEnumerator AttackDelay()
    {
        attacked = true;
        yield return new WaitForSeconds(1f);
        attacked = false;
    }

    [SerializeField] float trapDistance;
    [SerializeField] float trapSpawnOffset;
    float lastTrapDist;
    bool spawned = false;
    float dist;
    //int accumulatedDist = 0;
    private void CheckSpawnTraps()
    {
        dist = _input.transform.position.z + accumulatedDist;
        if(!spawned && dist % trapDistance < 1f)
        {
            lastTrapDist = dist + trapSpawnOffset;
            SpawnTrap();
            StartCoroutine(TrapDelay());
        }
    }

    IEnumerator TrapDelay()
    {
        spawned = true;
        yield return new WaitForSeconds(1f);
        spawned = false;
    }

    private void SpawnTrap()
    {
        int random = Random.Range(0, trapsPrefab.Length);
        TrapBehaviour spawned = Instantiate(trapsPrefab[random], new Vector3(0, 0.5f, _input.transform.position.z + trapSpawnOffset), Quaternion.identity).GetComponent<TrapBehaviour>();
        spawned.Initialize();
        spawned.trapTriggered.AddListener(TrapTriggered);

        for (int i = 0; i < spawnedTraps.Count; i++)
        {
            if (spawnedTraps[i] == null) 
            {
                spawnedTraps[i] = spawned.gameObject;
                return;
            } 
        }
        spawnedTraps.Add(spawned.gameObject);
    }

    private void ResetCharPos()
    {
        foreach (GameObject gameObject in spawnedTraps)
        {
            if(gameObject != null)
            {
                if(Vector3.Distance(_input.transform.position, gameObject.transform.position) < 30f)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, origin.z + (gameObject.transform.position.z - _input.transform.position.z));
                }
                else Destroy(gameObject);
            }
        }

        _input.GetComponent<CharacterController>().enabled = false;
        _input.gameObject.transform.position = new Vector3(_input.gameObject.transform.position.x, _input.gameObject.transform.position.y, origin.z);
;       _input.GetComponent<CharacterController>().enabled = true;
    }

    private void TrapTriggered(bool correct, string message)
    {
        if (!correct)
        {
            trapsTriggered++;
            StartCoroutine(GetCloser());
        }
    }

    [SerializeField] float bossMovementTime; 
    private IEnumerator GetCloser()
    {
        float timeElapsed = 0f;
        float initialOffset = bossOffset.z;
        Debug.Log(trapsTriggered.ToString());
        while (timeElapsed < bossMovementTime)
        {
            timeElapsed += Time.deltaTime;
            bossOffset.z = Mathf.Lerp(initialOffset, initialOffset - initialBossOffset.z/3, timeElapsed / bossMovementTime);
            yield return null;
        }
    }

    private void StopMovement()
    {
        _input.move.y = 0;
        enabled = false;
    }

    private void ResumeMovement()
    {
        enabled = true;
    }
}
