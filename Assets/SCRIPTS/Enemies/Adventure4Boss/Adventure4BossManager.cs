using UnityEngine;
using UnityEngine.Playables;

public class Adventure4BossManager : MonoBehaviour
{
    private PorcuMomBehaviour boss;
    [SerializeField] private Mesh[] meshes;
    private GameObject[] letters;
    [SerializeField] private GameObject grabbableLetterPrefab;
    [SerializeField] private int keyIdInit;
    [SerializeField] private GameObject gridParent;
    private Vector3 dropPoint;
    
    private int currentCount = 0;
    [SerializeField] private int expectedCount;

    [SerializeField] GameObject bossFightCamera;


    private void Start()
    {
        boss = GetComponentInChildren<PorcuMomBehaviour>();
        boss.hitWallChargingEvent.AddListener(DropNextLetter);

        letters = new GameObject[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            letters[i] = Instantiate(grabbableLetterPrefab);
            letters[i].GetComponentInChildren<MeshFilter>().mesh = meshes[i];
            letters[i].GetComponentInChildren<GrabbableObject>().objectID = keyIdInit + i;
            letters[i].GetComponentInChildren<GrabbableObject>().name = "Letra " + meshes[i].name.Substring(meshes[i].name.Length-1);
            letters[i].SetActive(false);
        }
        expectedCount = meshes.Length;

        int count = 0;
        foreach (var item in gridParent.GetComponentsInChildren<GridSquareBehaviour>())
        {
            item.m_keyObjectID = keyIdInit + count;
            item.objectInsertedEvent.AddListener(ActivatePorcupimMom);
            count++;
        }

        dropPoint = GetComponent<Collider>().bounds.center + Vector3.up * 10f;
        GetComponent<Collider>().enabled = false;
    }

    [ContextMenu("Startto")]
    public void StartBossFight()
    {
        boss.StartFight();
        bossFightCamera.SetActive(true);
    }

    private void DropNextLetter()
    {
        letters[currentCount].transform.position = dropPoint;
        letters[currentCount].SetActive(true);
    }

    private void ActivatePorcupimMom(bool objInsetCorrect,int id)
    {
        if (objInsetCorrect)
        {
            currentCount++;
            if (currentCount < expectedCount) boss.EnterSeekingMode();
            else PuzzleCompleted();
        }
    }

    public PlayableDirector _bossVictoryCin;
    private void PuzzleCompleted() 
    {
        bossFightCamera.SetActive(false);
        _bossVictoryCin.Play();
    }
}
