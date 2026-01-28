using UnityEngine;

public class AttackController : MonoBehaviour
{

    public Transform targetToAttack;

    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;

    public int unitDamage;

    private void OnTriggerEnter(Collider other)
    {
        targetToAttack = other.transform;
    }


    private void OnTriggerExit(Collider other)
    {
        targetToAttack = null;
    }


    public void SetIdleMaterial() 
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }

    public void SetFollowMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }

    public void SetAttackMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }

    private void OnDrawGizmos()
    {
        //Follow Distance / Area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,10f * 0.2f);

        //Attack Distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,1f);

        //Stop Attack Distance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,1.2f);

    }

}
