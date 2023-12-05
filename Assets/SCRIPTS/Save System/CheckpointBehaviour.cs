using UnityEngine;
using UnityEngine.Events;


public class CheckpointBehaviour : MonoBehaviour
{
    public int Id;
    public bool Triggered = false;
    public UnityEvent<int> m_CheckpointTriggered;

    public Animator _checkPointAnimator;

    public void SetActive(bool active)
    {
        gameObject.GetComponent<Collider>().enabled = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            ActivateCheckpoint();
            m_CheckpointTriggered.Invoke(Id);
            Debug.Log("salvei");
        }
    }

    private void ActivateCheckpoint()
    {
        Triggered = true;
        _checkPointAnimator.SetTrigger("Active");
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void MovePlayer()
    {
        ActivateCheckpoint();
        GameObject playerGO = FindObjectOfType<CharacterManager>().GetCurrentCharacter();
        playerGO.GetComponent<CharacterController>().enabled = false;
        playerGO.transform.position = transform.position;
        playerGO.GetComponent<CharacterController>().enabled = true;
    }
}