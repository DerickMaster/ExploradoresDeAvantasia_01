using UnityEngine;

public class PlantBossAttackBehaviour : StateMachineBehaviour
{
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Saindo do ataque");
        animator.gameObject.GetComponent<PlantBossBahaviour>().FinishAttack();
    }
}
