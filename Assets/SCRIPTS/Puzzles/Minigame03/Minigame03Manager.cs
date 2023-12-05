using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Minigame03Manager : MonoBehaviour, IMakeMistakes
{
    [System.Serializable]
    private struct LockCombination
    {
        public string word;
        public string expectedCombination;
        public Material lockMaterial;
        public string[] lettersInMat;
    }

    [SerializeField] private LockCombination[] combinations;
    [SerializeField] private PlayableDirector introCinematic;
    private FullLockBehaviour[] locks;
    private OperationButtonBehaviour[] buttons;
    private System.Random rng;
    [HideInInspector] public UnityEvent<string, MistakeData> wrongWordSubmitted;

    private void Start()
    {
        rng = new System.Random();
        locks = GetComponentsInChildren<FullLockBehaviour>();
        buttons = GetComponentsInChildren<OperationButtonBehaviour>();
        rng.Shuffle(combinations);
        for (int i = 0; i < combinations.Length; i++)
        {
            string letters = combinations[i].lockMaterial.name.Substring(8);
            combinations[i].lettersInMat = letters.ToCharArray().Select(c => c.ToString()).ToArray();
        }
        for (int i = 0; i < locks.Length; i++)
        {
            locks[i].SetExpectedCombination(combinations[i].expectedCombination, combinations[i].word, combinations[i].lockMaterial, combinations[i].lettersInMat);
            locks[i].gameObject.SetActive(false);
            buttons[i].SetMod(i);
            buttons[i].pressedEvent.AddListener(WordSubmitted);
        }

        locks[0].gameObject.SetActive(true);
        introCinematic.Play();
    }

    [TextArea] public string tempText;
    [SerializeField] float tempTextTime = 2f;
    [SerializeField] FMODUnity.EventReference _errorDub;
    [SerializeField] Sprite _professorSprite;
    private void WordSubmitted(int buttonId)
    {
        if (locks[buttonId].CheckCompletion())
        {
            locks[buttonId].DeactivateLock();
        }
        else
        {
            CanvasBehaviour.Instance.SetActiveTempText(tempText, tempTextTime, _professorSprite, _errorDub);
            MistakeData data = new MistakeData { mistakeMade = locks[buttonId].GetWordFromNumbers(string.Join("", locks[buttonId].curNumbers)) , rightAnswer = locks[buttonId].GetWordFromNumbers(locks[buttonId].expectedCombination) };
            wrongWordSubmitted.Invoke("Cadeados e Letras", data);
        }
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return wrongWordSubmitted;
    }
}
