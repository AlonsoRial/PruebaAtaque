using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 1.2f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false ) 
        {

            LookAtPlayer();

            //Keep moving towards enemy
            agent.SetDestination(attackController.targetToAttack.position);

            var damageToInflick = attackController.unitDamage;

            //Actually Attack Unit
            attackController.targetToAttack.GetComponent<Enemy>().ReceieveDamage(damageToInflick);

            //Should unit still attack
            float distanceFromTarge = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

            if (distanceFromTarge > stopAttackingDistance || attackController.targetToAttack == null)
            {
              
                animator.SetBool("isAttacking", false); //Move to follow state
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction); //OJITO A ESTO POR QUE PUEDE SER LA CAUSA DEL PROBLEMA

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation,0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
