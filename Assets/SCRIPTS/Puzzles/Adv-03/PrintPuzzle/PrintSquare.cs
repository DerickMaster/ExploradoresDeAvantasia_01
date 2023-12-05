using TMPro;
using UnityEngine;

public class PrintSquare : MonoBehaviour
{
    private string word;
    private TextMeshPro m_text;
    private Outline m_outline;

    private void Start()
    {
        m_text = GetComponentInChildren<TextMeshPro>();
        m_outline = GetComponentInChildren<Outline>();
        m_outline.enabled = false;
    }

    public void SetWord(string text)
    {
        word = text;
    }

    public void SetActiveText(bool active)
    {
        if (active) m_text.text = word;
        else m_text.text = "";
    }

    public void SetActiveOutline(bool active)
    {
        m_outline.enabled = active;
    }
}