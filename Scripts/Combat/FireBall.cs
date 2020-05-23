using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Esta es la clase Fireball que lo que hace es tener un prefab llamado Fireball que es un objeto y un daño especifica del ataque en caso de que hacierte*/
public class FireBall : Skill
{
    private string prefrabActionName;
    private int Damage;
    public AudioSource audio;
    // =================================
    void Start()
    {
        this.Damage = -20;
        this.prefrabActionName = "Fireball";
        this.manaCost = 2;
        this.coolDownTime = 1;
        this.time = this.coolDownTime;
    }


    //=================================
    /* Esta funcion indica que si el personaje cumple los requisitos de mana y tiempo de espera de habilidad podra lanzarla si la lanza la funcion lo que hara es coger el punto de fuego de nuestro presonaje
     (firepoint) y lanzar por ahi el prefab bala si la bala choca con otro objeto que tenga un colaider desaparecera */
    public override bool Execute()
    {
        if(this.canExecute == true)
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
