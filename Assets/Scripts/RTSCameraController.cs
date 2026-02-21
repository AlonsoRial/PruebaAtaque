using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class RTSCameraController : MonoBehaviour
{
    public static RTSCameraController instance;


    [Header("General")]
    [SerializeField] Transform cameraTransform;
    public Transform followTransform;
    Vector3 newPosition;


    [Header("Optional Functionality")]
    [SerializeField] bool moveWithKeyboad;
    [SerializeField] bool moveWithEdgeScrolling;


    [Header("Keyboard Movement")]
    [SerializeField] float fastSpeed = 0.05f;
    [SerializeField] float normalSpeed = 0.01f;
    [SerializeField] float movementSensitivity = 1f; // Hardcoded Sensitivity
    float movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] float edgeSize = 50f;
    bool isCursorSet = false;
    public Texture2D cursorArrowUp;
    public Texture2D cursorArrowDown;
    public Texture2D cursorArrowLeft;
    public Texture2D cursorArrowRight;

    [Header("Rotacion")]
    [SerializeField] private float angle = 45f;       // ángulo Y inicial
    [SerializeField] private float rotationSensitivity = 20f;  // Sensibilidad ratón
    private Vector2 vector_rotacion; //Vector que guarda el movmimiento del raton
    [SerializeField] private float distance;    // Distancia de la cámara al jugador

    CursorArrow currentCursor = CursorArrow.DEFAULT;
    enum CursorArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT
    }

    private void Start()
    {
        instance = this;
        angle = transform.eulerAngles.y;
        newPosition = transform.position;

        movementSpeed = normalSpeed;
    }

    private void Update()
    {
        HandleCameraMovement();

        //if ( InputSingleton.Instance.inputActions.Player.Salir.WasPressedThisFrame()  ) 
        //{
        //    followTransform = null;
        //}

        vector_rotacion = InputSingleton.Instance.inputActions.Player.MouseDelta.ReadValue<Vector2>();

    }


    private void LateUpdate()
    {
        if (followTransform == null) return;

        bool rotating = InputSingleton.Instance.inputActions.Player.Rotacion.IsPressed();


        if (rotating)
        {

            angle += vector_rotacion.x * rotationSensitivity ;
        }

        Quaternion rotation = Quaternion.Euler(30f, angle, 0f);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = followTransform.position + offset;
        transform.rotation = rotation;
    }


    void HandleCameraMovement()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Eliminamos componente vertical
        forward.y = 0f;
        right.y = 0f;

        // Normalizamos para evitar velocidades raras
        forward.Normalize();
        right.Normalize();

        // Keyboard Control
        if (moveWithKeyboad)
        {
            if (/*Input.GetKey(KeyCode.LeftCommand) */InputSingleton.Instance.inputActions.Player.Control.IsPressed()  )
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }


            Vector2 input = InputSingleton.Instance.inputActions.Player.Move.ReadValue<Vector2>();

            Vector3 moveDir = (forward * input.y + right * input.x).normalized;


            //newPosition += moveDir * movementSpeed;
            if (followTransform != null)
            {
                followTransform.position += moveDir * movementSpeed;
            }
            else
            {
                newPosition += moveDir * movementSpeed;
            }

        }

        // Edge Scrolling
        if (moveWithEdgeScrolling)
        {

            Vector2 mousePos = InputSingleton.Instance.inputActions.Player.MoveMouse.ReadValue<Vector2>();

            // Move Right
            if (mousePos.x > Screen.width - edgeSize)
            {
                //newPosition += right * movementSpeed;

                if (followTransform != null)
                {
                    followTransform.position += right * movementSpeed;
                }
                else
                {
                    newPosition += right * movementSpeed;
                }

                ChangeCursor(CursorArrow.RIGHT);
                isCursorSet = true;
            }

            // Move Left
            else if (mousePos.x < edgeSize)
            {
                //newPosition += -right * movementSpeed;

                if (followTransform != null)
                {
                    followTransform.position += -right * movementSpeed;
                }
                else
                {
                    newPosition += -right * movementSpeed;
                }

                ChangeCursor(CursorArrow.LEFT);
                isCursorSet = true;
            }

            // Move Up
            else if (mousePos.y > Screen.height - edgeSize)
            {
                // newPosition += forward * movementSpeed;
                if (followTransform != null)
                {
                    followTransform.position += forward * movementSpeed;
                }
                else
                {
                    newPosition += forward * movementSpeed;
                }

                ChangeCursor(CursorArrow.UP);
                isCursorSet = true;
            }

            // Move Down
            else if (mousePos.y < edgeSize)
            {
                //newPosition += -forward * movementSpeed;

                if (followTransform != null)
                {
                    followTransform.position += -forward * movementSpeed;
                }
                else
                {
                    newPosition += -forward * movementSpeed;
                }

                ChangeCursor(CursorArrow.DOWN);
                isCursorSet = true;
            }
            else
            {
                if (isCursorSet)
                {
                    ChangeCursor(CursorArrow.DEFAULT);
                    isCursorSet = false;
                }
            }

        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSensitivity);

        Cursor.lockState = CursorLockMode.Confined; // If we have an extra monitor we don't want to exit screen bounds
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        // Only change cursor if its not the same cursor
        if (currentCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowRight.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }

            currentCursor = newCursor;
        }
    }


}