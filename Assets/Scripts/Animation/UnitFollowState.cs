using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{

    AttackController attackController;

    NavMeshAgent agent;

    public float attackingDistance = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        attackController.SetFollowMaterial();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Should Unit Transition to Idle State?

        if (attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
        }
        else 
        {
            //If there is no other direct command to move
            if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false) 
            {
                // Should Unit toward State?
                agent.SetDestination(attackController.targetToAttack.position);
                animator.transform.LookAt(attackController.targetToAttack);

                // Should Unit Transition to Attack State?
                float distanceFromTarge = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

                if (distanceFromTarge < attackingDistance)
                {
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking",true); //Move to Attacking state
                }
            }
        }

    }


}
