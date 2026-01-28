using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 1.2f;

    public float attackRate = 1f;
    private float attackTimer;

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

            LookAtTarget();

            //Keep moving towards enemy
            agent.SetDestination(attackController.targetToAttack.position);


            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = 1 / attackRate;
            }
            else 
            {
                attackTimer -= Time.deltaTime;
            }

                //Should unit still attack
                float distanceFromTarge = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

            if (distanceFromTarge > stopAttackingDistance || attackController.targetToAttack == null)
            {
              
                animator.SetBool("isAttacking", false); //Move to follow state
            }
        }
    }

    private void Attack() 
    {

        var damageToInflick = attackController.unitDamage;

        //Actually Attack Unit
        attackController.targetToAttack.GetComponent<Unit>().TakeDamage(damageToInflick);
    }

    private void LookAtTarget()
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
