using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerControl player;
    public Vector3 offset;
    private Camera cam;
    private Ray ray;
    private Vector3 playerToMousePoint;
    private Vector3 mousePoint;

    //=========================================
    void Start()
    {
        this.cam = GetComponent<Camera>();
        this.mousePoint = Vector3.zero;
        this.mousePoint.y = 0;
    }

    //==========================================
    void Update()
    {
        if (this.player != null)
        {
            this.ray = this.cam.ScreenPointToRay(Input.mousePosition);

            //Calculate the place on the player plane, where de Ray hits
            this.mousePoint.x = ((this.player.transform.position.y - ray.origin.y) / ray.direction.y)
                * ray.direction.x + ray.origin.x;
            this.mousePoint.z = ((this.player.transform.position.y - ray.origin.y) / ray.direction.y)
                * ray.direction.z + ray.origin.z;
            this.mousePoint.y = this.player.transform.position.y;

            //Calculate de Vector to that point
            this.playerToMousePoint = this.player.transform.position - this.mousePoint;
            this.playerToMousePoint.y = this.player.transform.position.y;
            this.playerToMousePoint /= 4;


            // Set the player look direction
            this.player.lookDirection = -this.playerToMousePoint;

            this.transform.position = (this.player.transform.position - this.playerToMousePoint) + this.offset;
            this.transform.rotation = Quaternion.LookRotation(-this.offset);
        }
    }
}
