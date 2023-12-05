using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LettersButtonManager : MonoBehaviour
{
    public class LetterButtonEvent : UnityEvent<string, Button> { }
    [SerializeField] GameObject LetterButtonPrefab;
    private Button[] m_buttons;
    public LetterButtonEvent buttonClickedEvent;
    private void Awake()
    {
        buttonClickedEvent = new LetterButtonEvent();
    }
    public void Initialize(string letters)
    {
        string[] arr = new string[letters.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = letters[i].ToString();
        }
        SetTexts(arr);
    }

    public void Initialize(string[] syllabs)
    {
        SetTexts(syllabs);
    }

    private void SetTexts(string[] strs)
    {
        foreach (var button in GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }

        m_buttons = new Button[strs.Length];

        for (int i = 0; i < strs.Length; i++)
        {
            Button newButton = Instantiate(LetterButtonPrefab, gameObject.transform).GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = strs[i].ToString();
            newButton.onClick.AddListener(() => OnLetterButtonClicked(newButton.GetComponentInChildren<Text>().text, newButton));
            m_buttons[i] = newButton;
        }
    }

    public void OnLetterButtonClicked(string letter, Button btnClicked)
    {
        buttonClickedEvent.Invoke(letter, btnClicked);
    }

    public void BlockButton(Button btn, bool correct)
    {
        if (correct) btn.GetComponentInChildren<Text>().color = Color.green;
        else btn.GetComponentInChildren<Text>().color = Color.red;
        btn.gameObject.GetComponent<Image>().color = Color.gray;
        btn.enabled = false;
    }

    public void DeactiveAllButtons()
    {
        foreach(Button btn in m_buttons)
        {
            btn.enabled = false;
        }
    }
}