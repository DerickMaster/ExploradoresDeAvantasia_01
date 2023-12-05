using UnityEngine;
using UnityEngine.Events;

public class OperationButtonBehaviour : MonoBehaviour
{
    //[HideInInspector] public class ButtonPressedEvent : UnityEvent<int> { };
    [HideInInspector] public UnityEvent<int> pressedEvent;
    [SerializeField] int m_mod;
    [SerializeField] GameObject highesPoint;
    private Animator m_Animator;
    private bool curPressed;
    private bool blocked;
    private float time;
    [SerializeField] private float eTime;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        time = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;
        if(!curPressed && other.gameObject.tag.Equals("Player") && other.gameObject.GetComponent<StarterAssets.ThirdPersonController>().Grounded  
            && other.gameObject.transform.position.y > highesPoint.transform.position.y && time > eTime)
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
        if(!blocked) m_Animator.SetBool("Pressed", curPressed);
    }
    

    public void SendValue()
    {
        if(m_Animator.GetBool("Pressed"))
            this.pressedEvent.Invoke(m_mod);
    }

    public void SetBlocked(bool blocked)
    {
        this.blocked = blocked;
        if(!curPressed) m_Animator.SetBool("Pressed", blocked);
    }

    public void SetMod(int mod)
    {
        m_mod = mod;
    }
}
