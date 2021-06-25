
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    //variable deplacement joueur
    private Vector3 velocity;
    private Vector3 rotation;
    
    private Vector3 jumpVelocity;
    private bool canJump = true;
    
    //variable deplacement cam
    [SerializeField]
    private Camera cam;
    [SerializeField] 
    private float cameraRotationLimit = 85f;
    
    private float camerarotationX = 0f;
    private float currentCameraRotationX = 0f;
    private float timeWalk;
    
    public AudioClip marcheAudio;
    private AudioSource audioSource;
    
    //acceder au rb
    private Rigidbody rb;

    private void Start()
    {
        timeWalk = Time.time;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    //MOVEMMENT

    //stocker la velocity de controller (WASD) dans une variable
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    
    //stocker le saut dans une variable
    public void Jump(Vector3 _jumpVelocity)
    {
        jumpVelocity = _jumpVelocity;
    }

    //attendre de toucher le sol pour pouvoir de nouveau sauter
    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider)
        {
            canJump = true;
        }
    }

    // fonction d'application du mouvement (WASD et Jump)
    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            if (timeWalk<Time.time)
            {
                audioSource.PlayOneShot(marcheAudio);
                timeWalk = Time.time + marcheAudio.length;
            }
        }

        if (jumpVelocity != Vector3.zero && canJump)
        {
            rb.AddForce(jumpVelocity * Time.fixedDeltaTime, ForceMode.Impulse);
            canJump = false;
        }
    }

    

    //MOUVEMENT SOURIS
    
    //stocker la rotation de controller (droite et gauche) dans une variable
    public void RotateRightLeft(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    
    //stocker la rotation de controller (haut bas) dans une variable
    public void RotateUpDawn(float _camerarotationX)
    {
        camerarotationX = _camerarotationX;
    }
    
    //fonction d'application du mouvement de la camera
    private void PerformRotation()
    {
        //on calcule la rotation de la camera
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -=camerarotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        
        //on applique la rotation de la camera
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    

    //UPDATE DE MOTOR
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
}
