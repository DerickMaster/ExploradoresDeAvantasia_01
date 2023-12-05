using System.Collections;
using UnityEngine;

public class BombSpawn : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject warningSign;

    private GameObject m_Bomb;

    private void Start()
    {
        m_Bomb = Instantiate(bombPrefab, transform);
        m_Bomb.SetActive(false);
    }

    public void Initialize(float spawnDelay)
    {
        warningSign.SetActive(true);
        warningSign.GetComponent<CircleWarningBehaviour>().timeToFill = spawnDelay + 0.8f;
        StartCoroutine(SpawnBomb(spawnDelay));
    }

    IEnumerator SpawnBomb(float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        m_Bomb.transform.localPosition = Vector3.zero;
        m_Bomb.SetActive(true);
        yield return new WaitForSeconds(1);
        warningSign.SetActive(false);
    }
}
