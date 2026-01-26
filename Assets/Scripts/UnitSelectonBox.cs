using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        // When Clicked
        if ( /*Input.GetMouseButtonDown(0) */ InputSingleton.Instance.inputActions.Player.Interact.WasPressedThisFrame()  )
        {
            startPosition = /*Input.mousePosition;*/ InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>();

            // For selection the Units
            selectionBox = new Rect();
        }

        // When Dragging
        if (/*Input.GetMouseButton(0) */ InputSingleton.Instance.inputActions.Player.Interact.IsPressed())
        {
            if (boxVisual.rect.width>0 || boxVisual.rect.height>0) 
            {
                UnitSelectonManager.Instance.DeselectAll();
                SelectUnits();
            }



            endPosition = /*Input.mousePosition;*/ InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>();
            DrawVisual();
            DrawSelection();
        }

        // When Releasing
        if (/*Input.GetMouseButtonUp(0)*/  InputSingleton.Instance.inputActions.Player.Interact.WasReleasedThisFrame())
        {
            SelectUnits();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        // Calculate the starting and ending positions of the selection box.
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        // Calculate the center of the selection box.
        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        // Set the position of the visual selection box based on its center.
        boxVisual.position = boxCenter;

        // Calculate the size of the selection box in both width and height.
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        // Set the size of the visual selection box based on its calculated size.
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>().x < startPosition.x)
        {
            selectionBox.xMin = InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>().x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>().x;
        }


        if (InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>().y < startPosition.y)
        {
            selectionBox.yMin = InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>().y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>().y;
        }
    }

    void SelectUnits()
    {
        foreach (var unit in UnitSelectonManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectonManager.Instance.DragSelect(unit);
            }
        }
    }
}