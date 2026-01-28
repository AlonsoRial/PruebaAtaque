using UnityEngine;

public class AttackController : MonoBehaviour
{

    public Transform targetToAttack;

    private void OnTriggerEnter(Collider other)
    {
        targetToAttack = other.transform;
    }


    private void OnTriggerExit(Collider other)
    {
        targetToAttack = null;
    }

 
}
