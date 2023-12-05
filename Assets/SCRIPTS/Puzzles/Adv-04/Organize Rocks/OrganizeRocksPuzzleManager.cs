using UnityEngine;

public class OrganizeRocksPuzzleManager : MonoBehaviour
{
    private RecipientBehaviour[] rocks;
    private int recipientFilledCount;

    public InteractableObject _myObject;

    private void Start()
    {
        rocks = GetComponentsInChildren<RecipientBehaviour>();
        foreach(RecipientBehaviour rock in rocks)
        {
            rock.filledEvent.AddListener(RecipientFilled);
            rock.SetExpectedAmount();
        }
    }

    public void RecipientFilled()
    {
        recipientFilledCount++;
        if(recipientFilledCount == rocks.Length)
        {
            GetComponent<ObjectSounds>().playObjectSound(0);
            CanvasBehaviour.Instance.SetActiveTempText("Todos os recipients foram cheios", 2f);
            _myObject.Interact(null);
        }
    }
}