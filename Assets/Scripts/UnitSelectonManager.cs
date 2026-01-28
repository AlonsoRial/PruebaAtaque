using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitSelectonManager : MonoBehaviour
{
    public static UnitSelectonManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();


    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask attackable;
    public GameObject groundMaker;

    private Camera cam;

    private Material mateDefault;
   public Material mateSelected;

    public bool attackCursorVisible;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }

    }


    private void Start()
    {
        cam = Camera.main;

        if (allUnitsList.Count > 0)
        {
            mateDefault = allUnitsList[0].GetComponent<Renderer>().material;
        }

    }

    private void Update()
    {

        if (InputSingleton.Instance.inputActions.Player.Interact.WasPressedThisFrame())
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>());

            //Si clicka a un objecto clickable
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {

                if (InputSingleton.Instance.inputActions.Player.Attack.WasPressedThisFrame())
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else 
                {
                    SelectByClicking(hit.collider.gameObject);
                }
                    
            }
            else //Si no clicka, se deseleciona
            {
                if (!InputSingleton.Instance.inputActions.Player.Attack.WasPressedThisFrame())
                {
                    DeselectAll();
                }
            }

        }


        if (InputSingleton.Instance.inputActions.Player.Attack.WasPressedThisFrame()  && unitsSelected.Count>0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>());

            //Si clicka a un objecto clickable
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMaker.transform.position = hit.point;
                groundMaker.SetActive(false);
                groundMaker.SetActive(true);
            }
        }



        //Attack Targe

        if (unitsSelected.Count>0 && AtleastOneOffensiveUnit(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>());

            //Si clicka a un objecto clickable
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                attackCursorVisible = true;
                Debug.Log("Enemy Hoverd with mouse");
                if (InputSingleton.Instance.inputActions.Player.Attack.WasPressedThisFrame())
                {
                    Transform target = hit.transform;
                    foreach (GameObject unit in unitsSelected)
                    {
                        if (unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            }
            else 
            {
                attackCursorVisible=false;
            }
        }
    }

    private bool AtleastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }

    private void MultiSelect(GameObject gameObject)
    {
        if (unitsSelected.Contains(gameObject) == false)
        {
            unitsSelected.Add(gameObject);
            SelectUnit(gameObject, true);
        }
        else 
        {
            SelectUnit(gameObject, false);
            unitsSelected.Remove(gameObject);
        }
    }

    public void DeselectAll()
    {
        

        foreach (var unit in unitsSelected) 
        {
            SelectUnit(unit, false);
        }

        groundMaker.SetActive(false);

        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject gameObject)
    {
        //DeselectAll();

        unitsSelected.Add(gameObject);

        SelectUnit(gameObject, true);

    }

    private void SelectUnit(GameObject gameObject, bool shouldMove)
    {

        var rend = gameObject.GetComponent<Renderer>();
        var movement = gameObject.GetComponent<UnitMovement>();

        movement.enabled = shouldMove;

        if (shouldMove)
        {
            rend.material = mateSelected;
        }
        else
        {
            rend.material = mateDefault;
        }

    }

    internal void DragSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit)==false) 
        {
            unitsSelected.Add (unit);
            SelectUnit(unit, true);
        }
    }
}
