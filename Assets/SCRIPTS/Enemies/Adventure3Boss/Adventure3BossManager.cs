using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Adventure3BossManager : MonoBehaviour
{
    private FurnaceBossBehaviour boss;
    private FurnaceSocketBehaviour[] sockets;

    [SerializeField] GameObject flamingGround;
    [SerializeField] GameObject[] powerObjs;
    [SerializeField] FullLockBehaviour lockObj;

    [SerializeField] string[] expectedCombinations;
    [SerializeField] string[] expectedWords;

    [SerializeField] PlayableDirector cutscene;

    // Start is called before the first frame update
    void Start()
    {
        sockets = GetComponentsInChildren<FurnaceSocketBehaviour>();
        foreach(FurnaceSocketBehaviour socket in sockets)
        {
            socket.objInsertedEvent.AddListener(SocketFilled);
        }

        boss = GetComponentInChildren<FurnaceBossBehaviour>();
        boss.rageModeFinished.AddListener(EnterCalmPhase);
        boss.pushFinished.AddListener(EnterRagePhase);
        lockObj.lockSolvedEvent.AddListener(SpawnPowerObjs);
    }

    [ContextMenu("Start boss fight")]
    public void StartBossFight()
    {
        boss.SetRageMode(true);
    }

    public PlayableDirector _bossVictoryCin;
    [SerializeField] int socketsFilledAmount = 0;
    private void SocketFilled()
    {
        boss.TakeDamage();
        socketsFilledAmount++;
        if (socketsFilledAmount >= 2)
        {
            _bossVictoryCin.Play();
        }
        else
        {
            SetSpawnPowerObjs(false);
            boss.SetRageMode(true);
        }
    }

    private void SpawnPowerObjs()
    {
        lockObj.gameObject.SetActive(false);
        SetSpawnPowerObjs(true);
    }

    private void SetSpawnPowerObjs(bool active)
    {
        if (powerObjs[socketsFilledAmount] != null)
        {
            powerObjs[socketsFilledAmount].SetActive(active);
            powerObjs[socketsFilledAmount].transform.position = transform.TransformPoint(objPos);
        }
    }

    [SerializeField] private Vector3 objPos;
    private void ControlPhaseObjs(bool active)
    {
        flamingGround.SetActive(active);
        lockObj.gameObject.SetActive(!active);
        if (!active) 
        {
            lockObj.expectedCombination = expectedCombinations[socketsFilledAmount];
            lockObj.SetWord(expectedWords[socketsFilledAmount]);
        } 
    }

    private void EnterRagePhase()
    {
        ControlPhaseObjs(true);
    }

    private void EnterCalmPhase()
    {
        Invoke(nameof(ActivateCalmPhaseObjs), 1f);
    }

    private void ActivateCalmPhaseObjs()
    {
        ControlPhaseObjs(false);
        lockObj.gameObject.SetActive(true);
    }
}