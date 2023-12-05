using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InsertPuzzleManager : MonoBehaviour, IMakeMistakes
{
    private GridSquareBehaviour[] m_squares;
    private int curSquare = 0;
    
    [SerializeField] private GameObject grid1;
    [SerializeField] private string[] forms;
    [SerializeField] private GameObject[] holograms;
    [SerializeField] private InsertObjectsManager _objsManager;
    [HideInInspector] public UnityEvent puzzleFinished;

    [HideInInspector] public UnityEvent<string, MistakeData> _wrongForm;
    public string _puzzleName;
    public MistakeData _mistakeDescription;

    private void Start()
    {
        m_squares = GetComponentsInChildren<GridSquareBehaviour>();
        foreach(GridSquareBehaviour square in m_squares)
        {
            square.objectInsertedEvent.AddListener(ObjectInserted);
            square.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Start Puzzle")]
    public void StartPuzzle()
    {
        foreach (GridSquareBehaviour square in m_squares)
        {
            square.gameObject.SetActive(true);
        }

        m_squares[curSquare].SetActiveSquare(true);
        ShowTempMessage(forms[0]);
        holograms[0].SetActive(true);
        foreach(GrabbableObject grabObj in _objsManager.GetObjs())
        {
            grabObj.obj_Grab_Drop.AddListener(UnlockSlot);
        }
    }

    private void UnlockSlot(GrabbableObject obj)
    {
        if (obj.grabbed) 
        {
            m_squares[curSquare].blockInteract = false;
        } 
    }

    private void ObjectInserted(bool correct, int objID)
    {
        if (correct)
        {
            holograms[curSquare].SetActive(false);

            m_squares[curSquare].SetActiveSquare(false);

            curSquare++;
            if (curSquare >= m_squares.Length) 
            {
                Invoke(nameof(PuzzleCompleted), 0.1f);
                return;
            }
            
            ShowTempMessage(forms[curSquare]);
            m_squares[curSquare].SetActiveSquare(true);
            holograms[curSquare].SetActive(true);
        }
        else
        {
            _wrongForm.Invoke(_puzzleName,_mistakeDescription);
            _objsManager.ResetObjPos(objID);
            CanvasBehaviour.Instance.SetActiveTempText("Objeto errado tente novamente", 2f);
        }
        
    }

    private int curPart;
    private void PuzzleCompleted()
    {
        puzzleFinished.Invoke();
    }

    private void ShowTempMessage(string form)
    {
        if(curPart == 1)
            CanvasBehaviour.Instance.SetActiveTempText("Agora pegue o objecto que se parece com um" + form, 2f);
        else
            CanvasBehaviour.Instance.SetActiveTempText("Agora pegue " + form, 4f);
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return _wrongForm;
    }
}