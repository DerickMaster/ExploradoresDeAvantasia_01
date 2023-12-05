using UnityEngine;
using UnityEngine.UI;

public class UIHeartsManager : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private HealthManager hpManager;
    [SerializeField] private CharacterManager charManager;

    public GameObject heartPrefab;
    [SerializeField] private Sprite filledHeart;
    [SerializeField] private Sprite emptyHeart;
    

    private void Start()
    {
        charManager = FindObjectOfType<CharacterManager>();
        if (charManager)
        {
            InitializeHearts(charManager.GetCurrentHp());
            charManager.hpModified.AddListener(SetCurrentHearts);
        }
        else
        {
            hpManager = FindObjectOfType<HealthManager>();
            InitializeHearts(hpManager.maxHealthAmount);
            hpManager.hpModified.AddListener(SetCurrentHearts);
        }
    }

    private void InitializeHearts(int maxHealthAmount)
    {
        int heartAmount = maxHealthAmount / 2;
        hearts = new Image[heartAmount];

        for (int i = 0; i < heartAmount; i++)
        {
            hearts[i] = Instantiate(heartPrefab, gameObject.transform).GetComponent<Image>();
        }

        SetCurrentHearts(maxHealthAmount);
    }

    private void SetCurrentHearts(int curHealthAmount)
    {
        int currentHearts = curHealthAmount / 2;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= currentHearts) hearts[i].sprite = emptyHeart;
            else hearts[i].sprite = filledHeart;
        }
    }
}
