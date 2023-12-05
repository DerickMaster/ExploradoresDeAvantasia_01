using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    public int lastSavedCheckpoint;
    public CheckpointBehaviour[] checkpoints;
    public Vector3 playerSavedPosition;

    private void OnValidate()
    {
        checkpoints = GetComponentsInChildren<CheckpointBehaviour>();
    }
    private void Awake()
    {
        checkpoints = GetComponentsInChildren<CheckpointBehaviour>();
        int count = 1;
        foreach(CheckpointBehaviour checkpoint in checkpoints)
        {
            checkpoint.Id = count;
            checkpoint.m_CheckpointTriggered.AddListener(TriggerCheckpoint);
            count++;
        }
        //SaveSystemManager.Instance.Load();
    }
    public void TriggerCheckpoint(int Id)
    {
        lastSavedCheckpoint = Id;
        AdventureManager.Instance.SaveGame(Id);
        //SaveSystemManager.Instance.Save();
    }

    public void PutPlayerOnCheckPoint(int id)
    {
        checkpoints[id-1].MovePlayer();
    }
}