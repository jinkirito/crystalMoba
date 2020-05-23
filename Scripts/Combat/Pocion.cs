using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : Skill
{
    // Esta clase es la habilidad pocion lo que hace es que modifica la vida al jugador que la utiliza si pasa el tiempo necesario
    // y no se puede ejecutar hasta que pase el tiempo de espera hasta volverse a ejecutar 
    public AudioSource audio;
    private int amount;
   
    // =================================
    void Start()
    {
        this.amount = 50;
        this.manaCost = 0;
        this.coolDownTime = 100;
        this.time = this.coolDownTime;
    }


    //=================================
    // en esta funcion de ejecutar la habilidad primero comprueba si tiene los requisitos necesarios para poder ejecutarla si los tiene 
    // ejecuta la accion de modificar vida enseña se escucha el audio de beber una pocion 
    // y pone el tiempo de la habilidad a 0 y hasta que no pase 20 segundos no podra volver a activarlo siempre que cumpla los requisitos del coste
    // de mana.
    public override bool Execute()
    {
        


        if (this.canExecute == true)
        {
            this.playerPv.RPC("ModifyHealth", PhotonTargets.All, this.amount);
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

    }
}