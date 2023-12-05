using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightSpiritBehaviour : MonoBehaviour
{
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void ShowDirection(int direction)
    {
        m_animator.SetInteger("Direction", direction);
        m_animator.SetTrigger("Point");
    }
}