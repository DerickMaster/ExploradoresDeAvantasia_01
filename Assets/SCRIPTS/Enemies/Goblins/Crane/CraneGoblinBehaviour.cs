using UnityEngine;

public class CraneGoblinBehaviour : MonoBehaviour
{
    Vector3 direction;
    private Vector3 initialPos;
    [SerializeField] private float speed;
    void Awake()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
#endif
    }
    private void Start()
    {
        direction = transform.right;
        initialPos = transform.position;
    }
    private float addionationalMov;
    [SerializeField] private float movementDuration;
    [SerializeField] private int maxDistance;
    private void Update()
    {
        addionationalMov += (speed * Time.deltaTime);
        transform.position += (speed * Time.deltaTime * direction);
        if (addionationalMov > maxDistance)
        {
            addionationalMov *= -1;
            direction *= -1;
        }

    }
}
