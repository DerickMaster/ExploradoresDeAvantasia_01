using UnityEngine;
using UnityEngine.Playables;

public class FruitBombAutoSpawner : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] GameObject fruitBombPrefab;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] PlayableDirector cutscene;

    private FruitbombBehaviour curBomb;
    private float elapsedTime = 0f;

    private void Awake()
    {
        if(cutscene) cutscene.stopped += Cutscene_stopped;
        enabled = false;
    }

    private void Cutscene_stopped(PlayableDirector obj)
    {
        enabled = true;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > delay){
            elapsedTime = 0f;
            Spawn();
            enabled = false;
        }
    }

    public void Spawn()
    {
        FruitbombBehaviour fruit = Instantiate(fruitBombPrefab,CalculateSpawnPoint(), Quaternion.identity).GetComponent<FruitbombBehaviour>();
        fruit.selfActivated = true;
        fruit.bombExplodedEvent.AddListener(StartCountdown);
        curBomb = fruit;
    }

    public void Despawn()
    {
        if(curBomb)
            Destroy(curBomb.gameObject);

        elapsedTime = delay / 2;
        enabled = true;
    }

    private void StartCountdown(FruitbombBehaviour explodedFruit)
    {
        enabled = true;
    }

    [SerializeField] float radius;
    private Vector3 CalculateSpawnPoint()
    {
        Vector3 playerPos = CharacterManager.Instance.GetCurrentCharacter().transform.position;
        Vector3 spawnPos = playerPos;
        int rng = Random.Range(1, 11);
        float degree = DegreeToRadians(rng * 360 / 10);
        spawnPos.x = playerPos.x + (radius * Mathf.Cos(degree));
        spawnPos.z = playerPos.z + (radius * Mathf.Sin(degree));
        RaycastHit hit;
        if (Physics.Raycast(spawnPos + (Vector3.up * 10f), Vector3.down, out hit, 50 , groundLayer))
        {
            spawnPos = hit.point;
        }

        /*
        for (int i = 0; i < 11; i++)
        {
            degree = i * 360 / 10;
            Debug.DrawRay(new Vector3(playerPos.x + (radius * Mathf.Cos(DegreeToRadians(degree) )), spawnPos.y, playerPos.z + (radius * Mathf.Sin(DegreeToRadians(degree)))), Vector3.up * 10f,Color.green,10f);
        }
        */
        return spawnPos;
    }

    private float DegreeToRadians(float degree)
    {
        return degree * (Mathf.PI / 180);
    }
}
