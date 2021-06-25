using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerControllerCam : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivityX = 3f;
    [SerializeField]
    private float mouseSensitivityY = 3f;

    private PlayerMotor motor;
    
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }
    
    private void Update()
    {
        //MOUVEMENT CAMERA PERSONNAGE
        
        //calcule de rotation du joueur en un vector 3
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        motor.RotateRightLeft(rotation);
        
        
        //calcule de rotation de la camera en un vector 3

        float xRot = Input.GetAxisRaw("Mouse Y");
        float camerarotationX = xRot * mouseSensitivityY;

        motor.RotateUpDawn(camerarotationX);
    }
    
}