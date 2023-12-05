using UnityEngine;
using TMPro;

public class LetterScreenBehaviour : MonoBehaviour
{
    int curSlot = 0;
    TextMeshPro[] textMeshSlots;

    private void Start()
    {
        textMeshSlots = GetComponentsInChildren<TextMeshPro>();
    }

    public void UpdateCurSlot(string newChar)
    {
        textMeshSlots[curSlot].text = newChar;
        textMeshSlots[curSlot].color = Color.black;
        curSlot++;
    }

    public void RemoveCurSlot(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            curSlot--;
            textMeshSlots[curSlot].text = "";
        }
    }
}
