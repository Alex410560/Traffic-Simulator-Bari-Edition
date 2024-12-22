using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
    
    [SerializeField]
    [Header("Sensibilità del mouse")]
    float sensitivity;

    [SerializeField]
    [Header("Velocità di spostamento orizzonale")]
    float moveSpeed;

    [SerializeField]
    [Header("Velocità di spostamento verticale")]
    float verticalSpeed = 1f;

    float xRotation;
    float yRotation;

    [SerializeField] KeyCode upKey;
    [SerializeField] KeyCode downKey;

    [SerializeField] PostProcessLayer l;

    void Start()
    {
        upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "LeftShift"));
        downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "LeftControl"));
        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        moveSpeed = PlayerPrefs.GetFloat("HorizontalSpeed", 50f);

        l.antialiasingMode = (PostProcessLayer.Antialiasing)PlayerPrefs.GetInt("Antialiasing", 3);
    }

    void Update()
    {
        xRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        yRotation += Input.GetAxis("Mouse X") * sensitivity;

        //  Impedisce alla fotocamera di ruotare oltre gli 89° sull'asse X
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        //  Applica rotazione
        transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);

        //  Prende l'input di WASD
        float horizontalInput = Input.GetAxis("Horizontal"); // Restituisce 1 se premo D e -1 se premo A
        float verticalInput = Input.GetAxis("Vertical");     // Restituisce 1 se premo W e -1 se premo S


        //  Calcolare il movimento sugli assi X e Y in base all'orientamento della camera
        Vector3 right = transform.right * horizontalInput;                                                          // Movimento sull'asse X locale
        Vector3 forward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized * verticalInput;     // Movimento sull'asse Y locale

        //  Calcolare il movimento combinato
        Vector3 movement = right + forward;

        if (Input.GetKey(upKey))    //  Se shift è premuto...
        {
            movement.y += verticalSpeed;        //  ...calcola lo spostamento in alto sull'asse Y
        }
        if (Input.GetKey(downKey))  //  Se Ctrl è premuto...
        {
            movement.y -= verticalSpeed;        //  ...calcola lo spostamento in basso sull'asse Y
        }

        //  Applica movimento
        transform.position += movement * moveSpeed * Time.deltaTime;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 190, 750),
            Mathf.Clamp(transform.position.y, 63, 500),
            Mathf.Clamp(transform.position.z, -520, 484)
        );
    }
}