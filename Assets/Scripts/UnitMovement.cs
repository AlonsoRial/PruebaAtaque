using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class UnitMovement : MonoBehaviour
{



    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;


    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if ( InputSingleton.Instance.inputActions.Player.Attack.WasPressedThisFrame()) 
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>());

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) 
            {
                agent.SetDestination(hit.point);
            }

        }
    }

}
