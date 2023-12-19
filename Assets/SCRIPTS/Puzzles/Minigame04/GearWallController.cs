using UnityEngine;

public class GearWallController : MonoBehaviour
{
    [SerializeField] float delay;
    Animator[] walls;
    int curWall = 0;

    private void Start()
    {
        walls = GetComponentsInChildren<Animator>();

        walls[0].SetBool("Active", false);
    }

    float elapsedTime = 0f;
    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > delay)
        {
            walls[curWall].SetBool("Active", true);

            curWall++;
            if (curWall >= walls.Length) curWall = 0;

            walls[curWall].SetBool("Active", false);
            elapsedTime = 0f;
        }
    }
}
