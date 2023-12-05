using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveCharacterInPath : MonoBehaviour
{
    private CharacterManager cm;
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;
    [SerializeField] Transform targetPoint;

    private void Start()
    {
        cm = FindObjectOfType<CharacterManager>();
    }

    public void CalculatePath(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        CalculatePath();
    }

    [ContextMenu("Calculate Path")]
    public void CalculatePath()
    {
        if (targetPosition == Vector3.zero) targetPosition = targetPoint.position;

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(cm.GetCurrentCharacter().transform.position, targetPosition, 5, path))
        {
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Movement(cm.GetCurrentCharacter().GetComponent<StarterAssets.StarterAssetsInputs>(), path.corners));
        }
        else Debug.Log("Path not found");
    }

    private IEnumerator Movement(StarterAssets.StarterAssetsInputs _charInput ,Vector3[] points)
    {
        _charInput.GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(true);
        Debug.DrawLine(_charInput.transform.position, points[0]);
        for (int i = 0; i < points.Length-1; i++)
        {
            Debug.DrawLine(points[i], points[i+1], Color.red, 10f);
        }

        for (int i = 1; i < points.Length;)
        {
            Vector3 direction = (points[i] - _charInput.transform.position);
            _charInput.move = new Vector2(direction.x, direction.z).normalized;
            yield return null;
            if (Vector3.Distance( _charInput.transform.position, points[i]) < 0.5f)
            {
                i++;
            }
        }
        _charInput.move = Vector2.zero;
        _charInput.GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(false);
    }

    public void SetPath(Transform target)
    {
        targetPoint = target;
        CalculatePath();
    }
}