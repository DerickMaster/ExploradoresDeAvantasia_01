using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionBookManager : MonoBehaviour, ISaveable
{
    [SerializeField] float delayTime;
    [SerializeField] Material lockedAdvMaterial;

    bool waiting = false;
    int curButtonId = 0;
    StageLoaderButton curButton;
    List<StageLoaderButton> sceneButtons;
    SelectionBookInputs _input;
    
    private void Start()
    {
        sceneButtons = new List<StageLoaderButton>(GetComponentsInChildren<StageLoaderButton>());
        _input = GetComponent<SelectionBookInputs>();
        SetButtonAsSelected(sceneButtons[0]);

        foreach(StageLoaderButton button in sceneButtons)
        {
            button.btnClicked.AddListener(LoadScene);
        }
    }

    private void Update()
    {
        Navigate();
        Confirm();
    }

    private void Navigate()
    {
        if (waiting) return;

        if (_input.navigate.x > 0.5f)
        {
            ChangeCurrentButton(true);
        }
        else if (_input.navigate.x < -0.5f)

        {
            ChangeCurrentButton(false);
        }
    }

    private void Confirm()
    {
        if (_input.confirm)
        {
            _input.confirm = false;
            curButton.OnPointerClick(null);
        }
    }

    private void ChangeCurrentButton(bool right)
    {
        StartCoroutine(CountDelayTime());

        if (right)
        {
            curButtonId++;
            if (curButtonId >= sceneButtons.Count) curButtonId = 0;
        }else
        {
            curButtonId--;
            if (curButtonId < 0) curButtonId = sceneButtons.Count-1;
        }
        SetButtonAsSelected(sceneButtons[curButtonId]);
    }

    IEnumerator CountDelayTime()
    {
        waiting = true;

        float elapsedTime = 0;
        while(elapsedTime < delayTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        waiting = false;
    }

    private void SetButtonAsSelected(StageLoaderButton newBtn)
    {
        if (curButton != null) 
        {
            curButton.m_Outline.enabled = false;
        }

        curButton = newBtn;
        curButton.m_Outline.enabled = true;
    }

    private void LoadScene(string sceneName)
    {
        GameManager.instance.LoadScene(sceneName);
    }
    private struct BookData
    {
        public string[] names;
        public bool[] locks;
    }

    public object CaptureState()
    {
        string[] namesTemp = new string[sceneButtons.Count];
        bool[] locksTemp = new bool[sceneButtons.Count];

        for (int i = 0; i < sceneButtons.Count; i++)
        {
            namesTemp[i] = sceneButtons[i].GetSceneName();
            locksTemp[i] = sceneButtons[i].locked;
        }

        return new BookData
        {
            names = namesTemp,
            locks = locksTemp
        };
    }

    public void RestoreState(object state)
    {
        BookData data = (BookData)state;

        for (int i = 0; i < sceneButtons.Count; i++)
        {
            if (data.locks[i]) sceneButtons[i].SetLockButton(true , lockedAdvMaterial);
        }

        if (GameManager.instance.lastStageCompleted != "")
        {
            for (int i = 0; i < sceneButtons.Count; i++)
            {
                if (data.names[i].Equals(GameManager.instance.lastStageCompleted))
                {
                    if(i+1 < sceneButtons.Count) sceneButtons[i+1].SetLockButton(false);
                }
            }
        }

        GameManager.instance.lastStageCompleted = "";
    }
}