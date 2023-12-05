using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombArea : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject warningAreaPrefab;

    [SerializeField] Collider areaCollider;
    [SerializeField] float cellSize;
    [SerializeField] int bombAmount;

    public float timeBetweenWaves;
    [SerializeField] float baseWarningDelayTime;
    [SerializeField] float delayToSpawnBomb;

    private GameObject[] warnings;
    private List<Vector3> cellList;
    private List<int> positions;

    private void Start()
    {
        //player = FindObjectOfType<StarterAssets.ThirdPersonController>().gameObject;
        cellList = new List<Vector3>();
        GenerateBombArea();
        enabled = false;
        warnings = new GameObject[bombAmount];
        for (int i = 0; i < bombAmount; i++)
        {
            warnings[i] = Instantiate(warningAreaPrefab, transform);
            warnings[i].SetActive(false);
        }
    }

    [ContextMenu("Start bombing")]
    public void StartBombing()
    {
        enabled = true;
    }

    private float elapsedTime = 0f;
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > timeBetweenWaves)
        {
            SpawnBomb();
            elapsedTime = 0f;
        }
    }

    [ContextMenu("Spawn bombs")]
    public void SpawnBomb()
    {
        StartCoroutine(SpawnBombCoroutine());
    }

    [SerializeField] LayerMask layerMask;
    private IEnumerator SpawnBombCoroutine()
    {
        positions = new List<int>(bombAmount);
        int curAmount = 0;
        

        while (curAmount < bombAmount)
        {
            int newPos = Random.Range(0, cellList.Count);
            if (!positions.Contains(newPos))
            {
                positions.Insert(curAmount, newPos);
                curAmount++;
            }
        }

        
        int count = 0;
        foreach (int pos in positions)
        {
            RaycastHit hit;
            if(Physics.Raycast(cellList[pos] + (Vector3.up * 10f), Vector3.down, out hit, 15f,layerMask))
            {
                cellList[pos] = hit.point;
                Debug.DrawRay(cellList[pos], Vector3.up * 10f, Color.green, 15f);
            }
            warnings[count].SetActive(true);
            warnings[count].transform.position = cellList[pos];
            warnings[count].GetComponent<BombSpawn>().Initialize(delayToSpawnBomb);
            count++;

            float elapsedTime = 0f;
            float warningDelay = baseWarningDelayTime * Random.Range(baseWarningDelayTime / 2, baseWarningDelayTime * 2);
            while (elapsedTime < warningDelay)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
            }
        }
    }

    /*
    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(delayTime);

        int count = 0;
        foreach (int pos in positions)
        {
            bombs[count].SetActive(true);
            bombs[count].transform.position = cellList[pos];
            //Instantiate(bombPrefab, cellList[pos], Quaternion.identity);
            count++;
        }

        yield return new WaitForSeconds(delayToClean);
        foreach (GameObject warning in warnings)
        {
            warning.SetActive(false);
        }
    }
    */
    [ContextMenu("Generate bomb area")]
    public void GenerateBombArea()
    {
        Bounds bounds = areaCollider.bounds;
        Vector3 firstPoint = (bounds.center - new Vector3(bounds.extents.x, 0f, -bounds.extents.z));
        firstPoint.y = 0f;
        Vector3 firstPos = firstPoint + new Vector3(cellSize, 0f, -cellSize);
        GenerateCellsPos(firstPos);
    }

    private void GenerateCellsPos(Vector3 firstCell)
    {
        Vector3 newCell = firstCell;
        float xLimit = areaCollider.bounds.center.x + areaCollider.bounds.extents.x;
        float zLimit = areaCollider.bounds.center.z - areaCollider.bounds.extents.z;
        Debug.DrawRay(new Vector3(xLimit, 0f, zLimit), Vector3.up * 10f, Color.yellow, 10f);
        for (float x = firstCell.x; x < xLimit;)
        {
            for (float z = firstCell.z; z > zLimit;)
            {
                cellList.Add(newCell);
                Debug.DrawRay(newCell, Vector3.up, Color.blue, 10f);
                z -= cellSize;
                newCell.z = z;
            }
            x += cellSize;
            newCell.x = x;
            newCell.z = firstCell.z;
        }
    }
}