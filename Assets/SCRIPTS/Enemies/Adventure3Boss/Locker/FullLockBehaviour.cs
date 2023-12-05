using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class FullLockBehaviour : MonoBehaviour
{
    
    LockBehaviour[] dials;
    public int[] curNumbers { private set; get; }
    [SerializeField] bool autoComplete = true;
    [SerializeField] TextMeshPro word;
    public string expectedCombination;
    public UnityEvent lockSolvedEvent;

    
    private void Awake()
    {
        dials = GetComponentsInChildren<LockBehaviour>();
        curNumbers = new int[dials.Length];
    }

    private void Start()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].objectID = i;
            dials[i].faceSwitchedEvent.AddListener(IncreaseNumber);
            curNumbers[i] = 1;
        }
        word = GetComponentInChildren<TextMeshPro>();
    }

    private void OnEnable()
    {
        ResetLockState();
    }

    private void ResetLockState()
    {
        for (int i = 0; i < curNumbers.Length; i++)
        {
            curNumbers[i] = 1;
        }

        foreach(Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
            if (col.gameObject.GetComponent<LockBehaviour>() != null) col.gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
        word.gameObject.SetActive(true);
    }

    public void SetExpectedCombination(string expectedCombination, string word, Material lockMaterial, string[] lettersInMat = null)
    {
        this.expectedCombination = expectedCombination;
        this.word.text = word;
        foreach (LockBehaviour dial in dials)
        {
            dial.ChangeDialMaterial(lockMaterial);
        }
        if(lettersInMat != null)
        {
            characterInMat = lettersInMat;
        }

        ResetLockState();
    }

    private void IncreaseNumber(int id)
    {
        curNumbers[id]++;
        if (curNumbers[id] == 7) curNumbers[id] = 1;

        if (autoComplete && CheckCompletion()) DeactivateLock();
    }

    public bool CheckCompletion()
    {
        return string.Join("", curNumbers).Equals(expectedCombination);
    }

    public void DeactivateLock()
    {
        lockSolvedEvent.Invoke();
        Animator[] animators = GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("Awake", false);
            try
            {
                animators[i].gameObject.GetComponent<LockBehaviour>().Deactive();
            }
            catch { }
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
            word.gameObject.SetActive(false);
        }
    }

    public void SetWord(string word)
    {
        this.word.text = word;
    }

    [SerializeField] string[] characterInMat;
    public string GetWordFromNumbers(string numbers)
    {
        string translatedString = "";
        
        foreach(char num in numbers)
        {
            string toAdd;
            switch (num)
            {
                case '1':
                    toAdd = characterInMat[0];
                    break;
                case '2':
                    toAdd = characterInMat[1];
                    break;
                case '3':
                    toAdd = characterInMat[2];
                    break;
                case '4':
                    toAdd = characterInMat[3];
                    break;
                case '5':
                    toAdd = characterInMat[4];
                    break;
                case '6':
                    toAdd = characterInMat[5];
                    break;
                default:
                    toAdd = "?";
                    break;

            }
            translatedString += toAdd;
        }
        return translatedString;
    }
}
