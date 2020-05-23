using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * La clase SwordAtack es muy parecida a Fireball con la unica diferencia de que que el prefab que fanza tiene una distancia mas corta y es invisible por que se usa la animacion en este caso 
 */

public class SwordAtack : Skill
{
    private string prefrabActionName;
    private int Damage;
    public AudioSource audio;
    // =================================
    void Start()
    {
        this.Damage = -20;
        this.prefrabActionName = "SwordAtcak";
        this.manaCost = 2;
        this.coolDownTime = 1;
        this.time = this.coolDownTime;
    }


    //=================================
    public override bool Execute()
    {
        if (this.canExecute == true)
        {
            GameObject Sword = PhotonNetwork.Instantiate(this.prefrabActionName, this.firePoint.position, this.firePoint
             .rotation, 0);
            Sword.GetComponent<ActionSwordAtack>().SetSkill(this);
            Sword.GetComponent<Collider>().enabled = true;
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
    public override void Return(GameObject target)
    {
        PhotonView targetPV = target.GetComponent<PhotonView>();
        targetPV.RPC("ModifyHealth", PhotonTargets.All, this.Damage);
    }
}
