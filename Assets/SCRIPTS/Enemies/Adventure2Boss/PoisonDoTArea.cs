using UnityEngine;

public class PoisonDoTArea : DoTTriggerZone
{
    Animator[] pools_animators;
    Animator[] projectiles_Anim;

    private void Awake()
    {
        pools_animators = new Animator[2];
        projectiles_Anim = new Animator[2];

        int id = 0;
        int count = 1;

        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            if (count % 2 != 0)
                pools_animators[id] = animator;
            else
            {
                projectiles_Anim[id] = animator;
                id++;
            }
            count++;
        }
    }

    [ContextMenu("ActivateArea")]
    public void ActivateArea()
    {
        for (int i = 0; i < pools_animators.Length; i++)
        {
            pools_animators[i].SetBool("Active", true);
            projectiles_Anim[i].SetTrigger("Fall");
            pools_animators[i].GetComponentInChildren<ParticleSystem>().Play();
        }
        Invoke(nameof(ActivateCollider), 0.8f);
    }
    
    private void ActivateCollider()
    {
        SetActiveCollider(true);
    }
    
    private void DeactivateCollider()
    {
        SetActiveCollider(false);
    }

    public void DeactivateArea()
    {
        foreach (Animator animator in pools_animators)
        {
            animator.SetBool("Active", false);
            animator.GetComponentInChildren<ParticleSystem>().Stop();
        }
        Invoke(nameof(DeactivateCollider), 0.5f);
    }
}