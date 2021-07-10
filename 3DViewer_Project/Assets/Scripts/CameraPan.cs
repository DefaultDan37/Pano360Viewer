using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraPan : MonoBehaviour
{
#if UNITY_ANDROID

    protected ControlScheme controlScheme;

#endif

    /// <summary>
    /// Text UI element array:
    /// Used for updating the onscreen axis coordinates with the current rotation of the transform attached.
    /// size = 3.
    /// </summary>
    public Text[] axis_valueArray = new Text[3];

    /// <summary>
    /// Reference to the Camera camera component attached to this script.
    /// Will create a new camera if this script is not attached to one.
    /// </summary>
    protected Camera mainCamera;

    /// <summary>
    /// Quaternion initialised with the rotation component of this transform.
    /// Used for quickly updating the onscreen UI.
    /// </summary>
    protected Quaternion rotation;

    /// <summary>
    /// Float FOV value reference used to manipulate the FOV of the camera.
    /// Inialised with the camera's default fov values.
    /// </summary>
    protected float fov_value = 0;

    /// <summary>
    /// String used to encode the x axis of the camera's rotation
    /// </summary>
    protected string x = "";

    /// <summary>
    /// String used to encode the y axis of the camera's rotation
    /// </summary>
    protected string y = "";

    /// <summary>
    /// String used to encode the z axis of the camera's rotation
    /// </summary>
    protected string z = "";

    /// <summary>
    /// String Array used for encoding rotation axis information into the Text UI elements.
    /// size = 3
    /// </summary>
    protected string[] strArray = new string[3];

    /// <summary>
    /// Serialized fov scale factor.
    /// Used to adjust fov of camera on a more controlled scale.
    /// Range: 0f - 2f.
    /// </summary>
    [SerializeField]
    [Range(0, 2)]
    protected float fov_scale = 0.5f;

    /// <summary>
    /// Serialized rotationSpeed variable.
    /// Used the adjust the speed of the camera's rotation.
    /// Range: 0f - 10f.
    /// </summary>
    [SerializeField]
    [Range(0, 10)]
    protected float rotationSpeed = 1;

    private void Awake()
    {
#if UNITY_EDITOR_WIN
        if ((axis_valueArray != null) || (axis_valueArray.Length > 0))
        {
            for (int i = 0; i < axis_valueArray.Length; i++)
            {
                axis_valueArray[i].gameObject.SetActive(true);
            }
        }
#endif

#if UNITY_ANDROID
        controlScheme = new ControlScheme();
    
#endif
    }

#if UNITY_ANDROID
    private void OnEnable()
    {
        controlScheme.Enable();
    }

    private void OnDisable()
    {
        controlScheme.Disable();
    }

#endif

    void Start()
    {

#if UNITY_ANDROID

        //Subscribe OnTouch() method to TouchPress input.
        controlScheme.Player.TouchPress.started += ctx => OnTouch(ctx);

#endif
        // Searches for a reference to the camera component this script is attached to.
        mainCamera = gameObject.GetComponent<Camera>();

        // Initialises the fov_value with the fov of the camera.
        fov_value = mainCamera.fieldOfView;

        // Scales the fov by the fov_scale factor.
        mainCamera.fieldOfView = fov_value * fov_scale;

    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Updates the Quaternion with the camera's current transform rotation values.
            rotation = mainCamera.transform.rotation;
        }
 #if UNITY_EDITOR_WIN

        float xf = rotation.eulerAngles.x;
        float yf = rotation.eulerAngles.y;
        float zf = rotation.eulerAngles.z;

        if (xf < 0)
        {
            xf = 0f;
        }
        if (yf < 0)
        {
            yf = 0f;
        }
        if (zf < 0)
        {
            zf = 0f;
        }

        // x, y, z values are encoded with rotation values in degrees from the rotation Quaternion.
        x = $"X: {xf}";
        y = $"Y: {yf}";
        z = $"Z: {zf}";

        // strArray is updated with new axis values.
        strArray = new string[] { x, y, z };


        // For loop that updates the Text UI elements with rotation values
        for (int i = 0; i < axis_valueArray.Length; i++)
        {
            if (axis_valueArray[i] != null)
            {
                axis_valueArray[i].text = strArray[i];
            }
        }

        #endif
    }


#if UNITY_ANDROID

    private void OnTouch(InputAction.CallbackContext context)
    {
        // Reads the position value of the primaryTouch as Vector2 touchedPos
        Vector2 touchedPos = controlScheme.Player.TouchPosition.ReadValue<Vector2>();

        float x = 0;
        float y = 0;

        x = touchedPos.x;
        y = touchedPos.y;


        // Normalises x and y values to either -1, 0 or 1
        if (x > (Screen.width / 2))
        {
            x = 1;
        }
        else if (x < (Screen.width / 2))
        {
            x = -1;
        }

        if (y > (Screen.height / 2))
        {
            y = 1;
        }
        else if (y < (Screen.height / 2))
        {
            y = -1;
        }

        // rotatates the y axis of the transform in x direction in World space, * rotationSpeed.
        transform.Rotate(0f, x * rotationSpeed, 0f, Space.World);

        // rotates the x axis of the transform in y direction in Local space, * rotationSpeed.
        transform.Rotate(-y * rotationSpeed, 0f, 0f, Space.Self);
    }

#endif

#if !UNITY_ANDROID

    /// <summary>
    /// Unity action event OnPan.
    /// Invoked when the 'Pan' Input action is triggered by any of its 2D composite bindings (up, down, left and right arrow keys).
    /// Rotates the transform in a direction based on the InputValue (Vector2) by rotationSpeed.
    /// Up: (0, 1)
    /// Down: (0, -1)
    /// Left: (-1, 0)
    /// Right: (1, 0)
    /// No Key: (0, 0)
    /// Up and Left: (1, -1)
    /// Rotations on the x axis are applied in World space.
    /// Rotations on the y axis are applied in Local space.
    /// </summary>
    /// <param name="input"></param>
    public void OnPan(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        transform.Rotate(0f, inputVec.x * rotationSpeed, 0f, Space.World);
        transform.Rotate(-inputVec.y * rotationSpeed, 0f, 0f, Space.Self);

    }

#endif
}
