using UnityEngine;
using UnityEngine.UI;

public class IncompleteWordManager : MonoBehaviour
{
    [SerializeField] private GameObject letterSlotPrefab;
    private Text[] m_texts;
    int insertedCount = 0;

    public void Initialize(int wordSize)
    {
        insertedCount = 0;
        foreach (var slot in GetComponentsInChildren<Text>())
        {
            Destroy(slot.gameObject);
        }

        m_texts = new Text[wordSize];

        for (int i = 0; i < wordSize; i++)
        {
            Text newLetter = Instantiate(letterSlotPrefab, gameObject.transform).GetComponent<Text>();
            m_texts[i] = newLetter;
            newLetter.text = "_";
        }
    }

    public bool InsertLetter(string letter, int curLetter)
    {
        m_texts[curLetter].text = letter;
        insertedCount++;
        if(insertedCount == m_texts.Length)
        {
            return true;
        }
        return false;
    }

    public void ChangeLetterSlotSize(float width, float height)
    {
        foreach (var text in m_texts)
        {
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        }
    }
}