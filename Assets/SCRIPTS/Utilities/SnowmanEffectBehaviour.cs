using UnityEngine;

public class SnowmanEffectBehaviour : MonoBehaviour
{
    public float duration;
    public GameObject playerRef;

    private float elapsedTime;
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Debug.Log(playerRef.name);

        //if(playerRef)playerRef.GetComponentInChildren<Outline>().enabled = false;
    }

    private void Update()
    {
        if(elapsedTime > duration)
        {
            m_Animator.SetTrigger("Break");
        }
        elapsedTime += Time.deltaTime;
    }

    public void DeactivateMe()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //if (playerRef) playerRef.GetComponentInChildren<Outline>().enabled = true;
        elapsedTime = 0f;
    }
}
