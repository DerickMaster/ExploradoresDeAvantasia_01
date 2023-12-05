using UnityEngine;

public class BiaHouseManager : InteractableObject
{
    [SerializeField] private InsertPuzzleManager[] insertPuzzles;
    [SerializeField] private DoorBehaviour[] doors;
    [SerializeField] private GameObject dressPuzzleArea;
    [SerializeField] private CompleteWordPuzzleManager _dressPuzzle;
    private int curPhase = 0;

    private new void Start()
    {
        foreach(InsertPuzzleManager insertPuzzle in insertPuzzles)
        {
            insertPuzzle.puzzleFinished.AddListener(PuzzleFinished);
        }
        _dressPuzzle._puzzleFinished.AddListener(PuzzleFinished);
    }

    public override void Interact(InteractionController interactor)
    {
        Debug.Log("dale");
        switch (curPhase)
        {
            case 0:
                ActivateInsertFormPuzzle(0);
                break;
            case 1:
                doors[0].OpenDoor();
                ActivateDressPuzzle();
                break;
            case 2:
                ActivateInsertFormPuzzle(1);
                break;
            case 3:
                doors[1].OpenDoor();
                break;
        }
        curPhase++;
    }

    private void ActivateInsertFormPuzzle(int insertFormId)
    {
        insertPuzzles[insertFormId].StartPuzzle();
    }

    private void ActivateDressPuzzle()
    {
        dressPuzzleArea.SetActive(true);

    }
    

    private void PuzzleFinished()
    {
        Interact(null);
    }
}
