using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * La clase PlayerControl es la que se encargar deel movimiento del personaje de las animaciones de movimiento tambien 
 */

public class PlayerControl : MonoBehaviour
{
    public Vector3 lookDirection;
    private Vector3 input;
    private float speed;
    private Rigidbody rigBody;
    private Vector3 tmpEulerRot;
    public float x, y;
    private Animator anim;
    private PhotonView photonView;

    void Start()
    {
        this.input = Vector3.zero;
        this.speed = 7;

        this.rigBody = GetComponent<Rigidbody>();
        this.anim = GetComponent<Animator>();
        this.photonView = GetComponent<PhotonView>();
        this.lookDirection = Vector3.zero;
        this.tmpEulerRot = Vector3.zero;

    }

    // ==================================
    void Update()
    {
        this.x = Input.GetAxis("Horizontal");
        this.y = Input.GetAxis("Vertical");
        this.input.Set(
            this.x,
            0,
            this.y
            );

        this.tmpEulerRot.y = Quaternion.LookRotation(this.lookDirection).eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(this.tmpEulerRot);
        if (photonView.isMine) { 

        this.anim.SetFloat("VelX", this.x);
        this.anim.SetFloat("VelY", this.y);
    }

    }
    //=================================================
    void FixedUpdate()
    {
        if(this.input != Vector3.zero)
        {
            this.rigBody.velocity = this.input * this.speed;
        }
    }
}
