using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCForestSpirit : MonoBehaviour
{
    Animator _myAnim;

    private void Start()
    {
        _myAnim = GetComponent<Animator>();
    }

    public string tempText;
    [SerializeField] float tempTextTime;
    [SerializeField] int direction;

    public Sprite _portrait;
    private void OnTriggerEnter(Collider other)
    {
        CanvasBehaviour.Instance.SetActiveTempText(tempText, tempTextTime, _portrait);
        _myAnim.SetInteger("Direction", direction);
    }

    public void Disappear()
    {
        _myAnim.Play("Disappear");
    }

    private void OnEnable()
    {
        _myAnim = GetComponent<Animator>();
        _myAnim.Play("Appear");
    }

    public void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}
