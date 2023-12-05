using UnityEngine;
using UnityEngine.Events;

public class WaterPumpBehaviour : MonoBehaviour
{
    private Animator _animator;
    private bool curPressed;

    [SerializeField] int pumpValue;

    [HideInInspector] public UnityEvent<int> pumpPressed;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    float time;
    [SerializeField] float eTime;
    private void OnTriggerEnter(Collider other)
    {
        time = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;
        Debug.Log(other.gameObject.name);
        if (!curPressed && other.gameObject.tag.Equals("Player") && other.gameObject.GetComponent<StarterAssets.ThirdPersonController>().Grounded
            && other.gameObject.transform.position.y > transform.position.y && time > eTime)
        {
            SetPressedButton(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (curPressed && other.gameObject.tag.Equals("Player"))
        {
            SetPressedButton(false);
        }
    }

    public void SetPressedButton(bool press)
    {
        curPressed = press;
        _animator.SetBool("Pressed", curPressed);
    }

    public void SendValue()
    {
        if (_animator.GetBool("Pressed"))
            pumpPressed.Invoke(pumpValue);
    }
}
