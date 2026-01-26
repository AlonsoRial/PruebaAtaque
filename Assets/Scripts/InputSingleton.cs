using UnityEngine;

public class InputSingleton : MonoBehaviour
{
    [HideInInspector]
    public MyInputs inputActions;

    public static InputSingleton Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            inputActions = new MyInputs();
            inputActions.Player.Enable();

        }



    }

}
