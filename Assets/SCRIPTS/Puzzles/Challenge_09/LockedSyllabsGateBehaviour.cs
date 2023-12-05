using UnityEngine;
using UnityEngine.Events;

public class LockedSyllabsGateBehaviour : InteractableObject
{
    [SerializeField] SyllabCompleteWordPuzzle completeWordPuzzle;
    [SerializeField] string[] wordInSyllabs;
    [SerializeField] string[] syllabs;
    Animator m_Animator;

    [HideInInspector] public UnityEvent<LockedSyllabsGateBehaviour> puzzleFinishedEvent;
    private new void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
        try
        {
            //completeWordPuzzle = FindObjectOfType<SyllabCompleteWordPuzzle>();
            
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("couldnt find completeWordPuzzle to use");
        }
    }

    public void OpenGate()
    {
        //GetComponent<Collider>().isTrigger = true;
        DeactivateInteractable();
        m_Animator.SetBool("Open", true);
        puzzleFinishedEvent.Invoke(this);
    }

    public override void Interact(InteractionController interactor)
    {
        completeWordPuzzle.SetPuzzle(wordInSyllabs, syllabs);
        completeWordPuzzle._puzzleFinishedEvent.AddListener(OpenGate);

        completeWordPuzzle.TurnOnPuzzle();
    }

    public int GetSyllabsAmount()
    {
        return wordInSyllabs.Length;
    }
}
