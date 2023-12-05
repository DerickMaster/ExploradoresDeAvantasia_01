using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TouchDraw : MonoBehaviour
{
    private StarterAssets.StarterAssetsInputs _inputs;
    private Coroutine drawing;
    [SerializeField] private GameObject LinePrefab;
    private GameObject currentLine;
    private Camera m_camera;
    private List<GameObject> lines;
    [HideInInspector] public UnityEvent drawingSubmited;
    private bool touchMode = false;

    [ContextMenu("touch debug")]
    public void DebugTouch()
    {
        Debug.Log( JsonUtility.ToJson(_inputs.touchPosition));
    }

    private void Start()
    {
        //LinePrefab = Resources.Load("Line") as GameObject;
        lines = new List<GameObject>();
    }

    public void Initialize(InteractionController interactor, Camera camera)
    {
        _inputs = interactor._input;
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            _inputs.ActivateTouchDetection(true);
            touchMode = true;
            CanvasBehaviour.Instance.SetActiveBaseMobileUI(false);
            CanvasBehaviour.Instance._uiMobileManager.SetActiveElement("InterBtn", true);
        }
        
        m_camera = camera;
    }

    float lastValue;
    float value;
    private void Update()
    {
        lastValue = value;
        value = _inputs.mouseHold;

        if (value > 0f && lastValue < 1f)
        {
            Debug.Log("entro");
            StarLine();
        }
        else if (lastValue > 0f && value < 1f)
        {
            Debug.Log("Saiu");
            FinishLine();
        }

        if (_inputs.submit) 
        {
            _inputs.submit = false;
            Submit();
        }
        if (_inputs.cancel) 
        {
            _inputs.cancel = false;
            Cancel();
        }
    }

    private void StarLine()
    {
        if (drawing != null) StopCoroutine(drawing);
        drawing = StartCoroutine(DrawLine());
    }

    private void Submit()
    {
        int totalCount = 0;
        foreach (GameObject line in lines)
        {
            totalCount += line.GetComponent<LineRenderer>().positionCount;
        }
        if (totalCount > 100)
        {
            CanvasBehaviour.Instance.SetActiveBaseMobileUI(true);
            _inputs.ActivateTouchDetection(true);
            drawingSubmited.Invoke();
        }else CanvasBehaviour.Instance.SetActiveTempText("palavra curta de mais", 3f);
    }
    
    private void Cancel()
    {
        ClearLines();
    }

    public void ClearLines()
    {
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
    }

    private void FinishLine()
    {
        StopCoroutine(drawing);
    }

    private IEnumerator DrawLine()
    {
        currentLine = Instantiate(LinePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer line = currentLine.GetComponent<LineRenderer>();
        lines.Add(currentLine);
        line.positionCount = 0;

        while (true)
        {
            Vector3 pos;
            if (touchMode) pos = m_camera.ScreenToWorldPoint(_inputs.touchPosition);
            else pos = m_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, pos);
            yield return null;
        }
    }

}
