using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassWallBehaviour : BlockedZoneBehaviour
{
    private Collider m_collider;
    private Animator m_animator;
    private void Start()
    {
        m_collider = GetComponent<Collider>();
        m_animator = GetComponentInChildren<Animator>();
    }

    public void SetWall(bool active)
    {
        m_collider.isTrigger = !active;
        m_animator.SetBool("Active", active);
        allowPassage = false;
    }

    protected override void MoveBackReaction()
    {
        SetWall(true);
    }
}