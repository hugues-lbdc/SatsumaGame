using UnityEditor.Experimental;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    //vitesse et force du saut
    [SerializeField]
    private float speed = 3f;
    [SerializeField] 
    private float jumpForce = 1000f;
    
    //snesibilite souris
    /*
    [SerializeField]
    private float mouseSensitivityX = 3f;
    [SerializeField]
    private float mouseSensitivityY = 3f;
    */

    //acceder au motor du joueur
    private PlayerMotor motor;

    //animation du joueur
    private Animator Anim;
    
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // MOUVEMENT PHYSIQUE PERSONNAGE ( mouvement + saut )
        
        // calculer velocite du mvt de joueur
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;
        
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);
        
        
        //calcule force saut
        Vector3 jumpVelocity = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            jumpVelocity = Vector3.up * jumpForce;
            Anim.SetBool("Jump", true);
        }
        //application de la force du saut
        motor.Jump(jumpVelocity);

        
        /*
        //MOUVEMENT CAMERA PERSONNAGE
        
        //calcule de rotation du joueur en un vector 3
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        motor.RotateRightLeft(rotation);
        
        
        //calcule de rotation de la camera en un vector 3

        float xRot = Input.GetAxisRaw("Mouse Y");
        float camerarotationX = xRot * mouseSensitivityY;

        motor.RotateUpDawn(camerarotationX);
        */

        //animations du joueur
        if (Input.GetKey(KeyCode.W))
        {
            Anim.SetBool("Run", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Anim.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Anim.SetBool("Sword", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Anim.SetBool("Sword", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Anim.SetBool("RunL", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Anim.SetBool("RunL", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Anim.SetBool("RunR", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Anim.SetBool("RunR", false);
        }

        if (Input.GetButtonUp("Jump"))
        {
            Anim.SetBool("Jump", false);
        }
       
    }
    



}
