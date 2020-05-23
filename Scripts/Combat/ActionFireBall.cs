using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Esta clase llamada Action fireball lo que hace es comprobar si el prefab al chocar con otro colaider es de nuestro equipo o no */

public class ActionFireBall : Action
{
    private int speed;

    private void Start()
    {
        this.speed = 9;
        this.timeOut = 2;
    }

    /**
     * Como podemos ver en la funcion OnTriggerEnter esta funcion recibe un colaider si el colaide es distinto del mio pasa a la siguiente comprobacion 
     * que es que optenemos el colaider mio y el del otro objeto y cogemos su equipo, si el equipo es distindo del nuestro entonces podremos hacerle daño 
     * si no lo es la habilidad no le hara daño al jugador.
     */
    private void OnTriggerEnter(Collider other)
    {
        GameObject op = other.gameObject;
        if (op != this.skill.gameObject)
        {
            CombatSystem otherCS = op.GetComponent<CombatSystem>();
            CombatSystem myCS = this.skill.GetCombatSystem();

            if (otherCS.GetTeam() != myCS.GetTeam())
            {
                this.skill.Return(other.gameObject);
                this.photonview.RPC("AutoDestroy", PhotonTargets.All, null);
            }



        }

    }

    protected override void Tick()
    {
        this.transform.position += this.transform.forward * this.speed * Time.deltaTime;
    }
}