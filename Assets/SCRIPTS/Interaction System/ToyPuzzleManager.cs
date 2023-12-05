using UnityEngine;

public class ToyPuzzleManager : MonoBehaviour
{
    public GrabbableObject[] toys;
    public LauraChestBehaviour toyChest;

    private void Start()
    {
        toyChest._puzzleFinished.AddListener(PuzzleFinished);
        foreach(GrabbableObject toy in toys)
        {
            toy.obj_Grab_Drop.AddListener(OnObjectGrab);
        }
        toyChest.blockInteract = true;
    }

    private void OnObjectGrab(GrabbableObject obj)
    {
        toyChest.SetActiveOutline(obj.grabbed);
        toyChest.blockInteract = !obj.grabbed;
    }

    void PuzzleFinished()
    {
        GetComponent<ObjectSounds>().playObjectSound(0);
    }
}
