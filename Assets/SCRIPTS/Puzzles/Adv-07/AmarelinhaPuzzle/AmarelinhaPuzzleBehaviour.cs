using UnityEngine;

public class AmarelinhaPuzzleBehaviour : MonoBehaviour
{
    [SerializeField] Transform throwbackPoint;
    private AmarelinhaTileBehaviour[] tiles;

    private void OnValidate()
    {
        Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        tiles = GetComponentsInChildren<AmarelinhaTileBehaviour>();
        foreach (AmarelinhaTileBehaviour tileBehaviour in tiles)
        {
            tileBehaviour.SetThrowPosition(throwbackPoint.position);
            tileBehaviour.finishedThrow.AddListener(ResetTiles);
        }
    }

    private void ResetTiles()
    {
        foreach (AmarelinhaTileBehaviour tileBehaviour in tiles)
        {
            tileBehaviour.ResetTile();
        }
    }
}
