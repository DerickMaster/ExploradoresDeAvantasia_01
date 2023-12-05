using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowsBehaviour : MonoBehaviour
{
    public Animator _myAnimator;
    public bool _right = true;
    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
        ArrowDirectionRight(_right);
    }
    public void ArrowDirectionRight(bool right)
    {
        if (right)
        {
            _myAnimator.SetBool("rightActive", true);
        }
        else
        {
            _myAnimator.SetBool("rightActive", false);
        }
    }
}
