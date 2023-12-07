using System.Collections.Generic;
using UnityEngine;

public class LibraryBossBehaviour : MonoBehaviour
{
    [System.Serializable]
    private struct DangerArea
    {
        public string moveName;
        public GameObject areaObj;
    }

    [SerializeField] private CombatUnit m_CombatUnit;
    [SerializeField] private MoveChain[] moves;
    [SerializeField] private DangerArea[] dangerAreas;
    [SerializeField] Dictionary<string, GameObject> dangerAreasDict;


    private void Start()
    {
        GetComponentInChildren<CombatAnimationsListener>().startCheckingArea.AddListener(m_CombatUnit.CheckHit);
        GetComponentInChildren<CombatAnimationsListener>().stopCheckingArea.AddListener(m_CombatUnit.FinishCheckHit);

        GetComponent<CombatUnit>().MoveUsedEvent.AddListener(AnalyzeMoveUsed);
        GetComponent<CombatUnit>().MoveFinishedEvent.AddListener(MoveAnimationFinished);

        dangerAreasDict = new Dictionary<string, GameObject>();
        foreach (DangerArea area in dangerAreas)
        {
            dangerAreasDict.Add(area.moveName, area.areaObj);
        }
        dangerAreas = null;
    }

    [SerializeField] int moveToUse;
    [ContextMenu("Chain attack")]
    public void ChainAttack()
    {
        m_CombatUnit.UseChain(moves[moveToUse].attackNames);
    }

    GameObject activeArea;
    private void AnalyzeMoveUsed(string move)
    {
        GameObject area;
        if (dangerAreasDict.TryGetValue(move, out area))
        {
            area.SetActive(true);
            activeArea = area;
        }
    }

    private void MoveAnimationFinished()
    {
        activeArea.SetActive(false);
    }
    
}