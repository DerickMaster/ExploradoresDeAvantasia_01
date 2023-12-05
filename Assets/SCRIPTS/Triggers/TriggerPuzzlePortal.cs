using UnityEngine;
using UnityEngine.Playables;

public class TriggerPuzzlePortal : MonoBehaviour
{
    public BoardPuzzleBehaviour[] _boards;
    public DoorBehaviour _portal;
    public PlayableDirector _portalCutscene;
    bool _cinematicHappened = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            CheckBoards();
    }

    int _boardsSolved;
    void CheckBoards()
    {
        foreach (BoardPuzzleBehaviour board in _boards)
        {
            if (board.solved)
                _boardsSolved++;
        }
        if (_boardsSolved == _boards.Length && !_cinematicHappened)
        {
            _portal.OpenDoor();
            _portalCutscene.Play();
            _cinematicHappened = true;
        }
        else
        {
            if(!_cinematicHappened) _boardsSolved = 0;
        }
    }
}
