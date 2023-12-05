using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AmarelinhaTileBehaviour : MonoBehaviour, ITrap
{
    private bool passed = false;
    private Animator _animator;

    [SerializeField] private bool passable;

    [SerializeField] private Vector3 throwPosition;
    [SerializeField] float throwHeight;
    [SerializeField] float throwForce;
    [SerializeField] float throwTime;

    [HideInInspector] public UnityEvent trapTriggered;
    [HideInInspector] public UnityEvent finishedThrow;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!passed && other.tag.Equals("Player") && other.GetComponent<StarterAssets.ThirdPersonController>().Grounded)
        {
            other.GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(true);
            StartCoroutine(TileCheck(other));
        }
    }

    private IEnumerator TileCheck(Collider other)
    {
        //yield return new WaitForSeconds(0.5f);
        CheckTile();
        if (!passable)
        {
            trapTriggered.Invoke();

            yield return new WaitForSeconds(0.2f);
            other.GetComponent<StarterAssets.ThirdPersonController>().TakeKnockback(throwPosition - other.gameObject.transform.position, throwHeight, throwForce, throwTime);
            yield return new WaitForSeconds(0.07f);
            _animator.SetTrigger("Activated");
            yield return new WaitForSeconds(throwTime * 1.5f);

            finishedThrow.Invoke();
        }
        else passed = true;
        other.GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(false);
    }

    private void CheckTile()
    {
        if (passable) gameObject.GetComponentInChildren<Renderer>().material.color = Color.green;
        else gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    public void SetThrowPosition(Vector3 position)
    {
        throwPosition = position;
    }

    public void ResetTile()
    {
        passed = false;
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.gray;
    }

    [ContextMenu("Calc force from distance")]
    public void CalcForce()
    {
        throwForce = Vector3.Distance(transform.position, throwPosition);
    }

    public UnityEvent GetTrapEvent()
    {
        return trapTriggered;
    }

    public UnityEvent GetTrapFinishedEvent()
    {
        return finishedThrow;
    }
}