using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class UnitMovement : MonoBehaviour
{



    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;


    public bool isCommandedToMove;

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
                isCommandedToMove = true;
                agent.SetDestination(hit.point);
            }

        }

        //Agent reached destination
        if (agent.hasPath == false || agent.remainingDistance <=agent.stoppingDistance) 
        {
            isCommandedToMove=false;
        }

    }



}
