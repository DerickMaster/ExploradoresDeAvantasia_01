using System.Linq;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Minigame06Manager_V1 : MonoBehaviour, IMakeMistakes
{
    [SerializeField] GameObject platformParent;
    GameObject[] platforms;
    [SerializeField]int curNum = 1;
    int curPlatformId;
    [SerializeField] DoorBehaviour _puzzleDoor;
    [SerializeField] GameObject _mainCamera, _secondCamera;
    private JesterThrowBehaviour jester;
    [HideInInspector] public UnityEvent<string, MistakeData> mistakeEvent;
    private void Start()
    {
        Collider[] temp = platformParent.GetComponentsInChildren<Collider>();
        platforms = new GameObject[temp.Length]; 
        for (int i = 0; i < temp.Length; i++)
        {
            platforms[i] = temp[i].transform.parent.gameObject;
        }
        enabled = false;
        GetComponentInChildren<FallPitBehaviour>().playerTriggeredZoneEvent.AddListener(RespawnCharacter);
        jester = transform.parent.GetComponentInChildren<JesterThrowBehaviour>();
    }

    [SerializeField] float delay;
    float elapsedTime = 0f;
    private void Update()
    {
        if(elapsedTime > delay)
        {
            elapsedTime = 0f;
            enabled = false;
            StartCoroutine(LowerPlatforms());
        }
        elapsedTime += Time.deltaTime;
    }

    private void Reenable()
    {
        enabled = true;
    }

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        StartCoroutine(CountdownOnScreen());
    }

    readonly string sysmbolsString = "$%#@!*abcdefghijklmnpqrstuvwxyz?;:ABCDEFGHIJKLMNPQRSTUVWXYZ^&";
    private void SetSymbols()
    {
        curPlatformId = Random.Range(0, platforms.Length);

        for (int i = 0; i < platforms.Length; i++)
        {
            if (i == curPlatformId) platforms[i].GetComponentInChildren<TextMeshPro>().text = curNum.ToString();
            else platforms[i].GetComponentInChildren<TextMeshPro>().text = char.ToString(sysmbolsString.ElementAt(Random.Range(0, sysmbolsString.Length)));
        }
        curNum++;
        jester.ActivateShooting();
        if (curNum > 10)
        {
            EndGame();
        }
    }

    [SerializeField, TextArea] string _endgameText;
    private void EndGame()
    {
        StopAllCoroutines();
        enabled = false;
        CanvasBehaviour.Instance.SetActiveTempText(_endgameText, 3f, _jesterSprite, _jesterDubs[4]);
        for (int i = 0; i < platforms.Length; i++)
        { 
            platforms[i].gameObject.GetComponentInChildren<Animator>().SetBool("Active", false);
            platforms[i].GetComponentInChildren<Collider>().enabled = true;
        }
        _jester.gameObject.SetActive(false);
        _puzzleDoor.OpenDoor();
        _mainCamera.SetActive(true);
        _secondCamera.SetActive(false);
    }

    [SerializeField] Sprite _jesterSprite;
    [SerializeField] FMODUnity.EventReference[] _jesterDubs;
    IEnumerator CountdownOnScreen()
    {
        _mainCamera.SetActive(false);
        _secondCamera.SetActive(true);
        int countdown = 3;
        while(countdown > 0)
        {
            CanvasBehaviour.Instance.SetActiveTempText(countdown.ToString(), 1f, _jesterSprite, _jesterDubs[countdown - 1]);
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        CanvasBehaviour.Instance.SetActiveTempText("Começar!", 2f, _jesterSprite, _jesterDubs[3]);
        Reenable();
        StartCoroutine(Shuffle());
    }

    [SerializeField] float moveTime;
    private readonly Vector3 loweredCenter = new Vector3(0f,-0.5f,0f);
    IEnumerator LowerPlatforms()
    {
        int lastId = curPlatformId;
        float tempTime = 0f;
        Vector3[] initialPos = new Vector3[platforms.Length];
        for (int i = 0; i < initialPos.Length; i++)
        {
            initialPos[i] = platforms[i].transform.position;
        }
        Vector3[] finalPos = new Vector3[platforms.Length];
        for (int i = 0; i < initialPos.Length; i++)
        {
            finalPos[i] = platforms[i].transform.position;
            finalPos[i].y = 0f;
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            if (i != lastId)
            {
                platforms[i].gameObject.GetComponentInChildren<Animator>().SetBool("Active", true);
                platforms[i].GetComponentInChildren<Collider>().isTrigger = true;
                //platforms[i].GetComponentInChildren<Collider>().bounds = new Bounds
            }
        }

        while (tempTime < moveTime)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Shuffle());

        tempTime = 0f;

        while (tempTime < moveTime)
        {
            tempTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < platforms.Length; i++)
        {
            if (i != lastId)
            {
                platforms[i].gameObject.GetComponentInChildren<Animator>().SetBool("Active", false);
                platforms[i].GetComponentInChildren<Collider>().isTrigger = false;
            }
        }
        Reenable();
    }

    [SerializeField] float _shuffleTime = 1;
    IEnumerator Shuffle()
    {
        float timer = 0;
        while (timer < _shuffleTime)
        {
            curPlatformId = Random.Range(0, platforms.Length);

            for (int i = 0; i < platforms.Length; i++)
            {
                if (i == curPlatformId) platforms[i].GetComponentInChildren<TextMeshPro>().text = curNum.ToString();
                else platforms[i].GetComponentInChildren<TextMeshPro>().text = char.ToString(sysmbolsString.ElementAt(Random.Range(0, sysmbolsString.Length)));
            }
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SetSymbols();
    }

    [SerializeField] Animator _jester;
    private void RespawnCharacter()
    {
        
        _jester.Play("Laughing");
        GameObject player = CharacterManager.Instance.GetCurrentCharacter();
        

        player.GetComponent<CharacterController>().enabled = false;

        string mistakeMade = "caiu fora";
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position + (Vector3.up * 3f), Vector3.down, out hit, 100f))
        {
            try
            {
                mistakeMade = hit.collider.GetComponentInChildren<TextMeshPro>().text;
            }
            catch (System.NullReferenceException)
            {
                mistakeMade = "caiu fora";
            }
        }
        mistakeEvent.Invoke("Numeros e letras", new MistakeData { mistakeMade = mistakeMade , rightAnswer = curNum.ToString()});

        player.transform.position = platforms[curPlatformId].transform.position + (Vector3.up * 3f);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponentInChildren<HurtBox>().TakeDamage(2);
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return mistakeEvent;
    }
}