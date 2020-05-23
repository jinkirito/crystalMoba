using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Esta clase es igual que la clase fireball
 */
public class ThrowAxes : Skill
{
    private string prefrabActionName;
    private int Damage;
    public AudioSource audio;
    // =================================
    void Start()
    {
        this.Damage = -50;
        this.prefrabActionName = "axe";
        this.manaCost = 30;
        this.coolDownTime = 45;
        this.time = this.coolDownTime;
    }


    //=================================
    
    public override bool Execute()
    {
        if (this.canExecute == true)
        {
            GameObject ball = PhotonNetwork.Instantiate(this.prefrabActionName, this.firePoint.position, this.firePoint
             .rotation, 0);
            ball.GetComponent<ActionFireBall>().SetSkill(this);
            ball.GetComponent<Collider>().enabled = true;
            this.audio.Play(2);
            this.time = 0;
            this.canExecute = false;
            return true;
        }
        else
        {
            return false;
        }

    }

    //================================
    // Si la bala ha chocad con un nexo o jugador enemigo le restara la vida indicada que eneste caso dera 20 
    public override void Return(GameObject target)
    {
        PhotonView targetPV = target.GetComponent<PhotonView>();
        targetPV.RPC("ModifyHealth", PhotonTargets.All, this.Damage);
    }

}
