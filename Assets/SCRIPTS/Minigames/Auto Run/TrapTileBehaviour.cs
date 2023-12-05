using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TrapTileBehaviour : MonoBehaviour
{
    public int id = -1;
    Renderer m_Render;
    TextMeshPro m_TextMesh;
    //public bool correct = false;
    [HideInInspector] public UnityEvent<TrapTileBehaviour> tileStepped;
    private void Awake()
    {
        m_Render = GetComponentInChildren<Renderer>();
        m_TextMesh = GetComponentInChildren<TextMeshPro>();
    }

    public void SetColor(Color newColor)
    {
        m_Render.material.color = newColor;
    }

    public void SetText(string newText)
    {
        m_TextMesh.text = newText;
    }

    public string GetText()
    {
        return m_TextMesh.text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            tileStepped.Invoke(this);
        }
    }

}
