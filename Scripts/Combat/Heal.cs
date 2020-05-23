using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    // Esta clase es la habilidad cura lo que hace es que modifica la vida al jugador q la utiliza si tiene la cantidad de mana necesaria para ejecutarse 
    // y no se puede ejecutar hasta que pase el tiempo de espera hasta volverse a ejecutar 
    private string prefrabActionName;
    private int amount;
    public AudioSource audio;
    // =================================
    void Start()
    {
        this.amount = 50;
        this.prefrabActionName = "Heal";
        this.manaCost = 50;
        this.coolDownTime = 20;
        this.time = this.coolDownTime;
    }


    //=================================
    // en esta funcion de ejecutar la habilidad primero comprueba si tiene los requisitos necesarios para poder ejecutarla si los tiene 
    // ejecuta la accion de modificar vida enseña el prefab en pantalla que son unas particulas para que todos los jugadores en red lo vea
    // y pone el tiempo de la habilidad a 0 y hasta que no pase 20 segundos no podra volver a activarlo siempre que cumpla los requisitos del coste
    // de mana.
    public override bool Execute()
    {
        if (this.canExecute == true)
        {
            this.playerPv.RPC("ModifyHealth", PhotonTargets.All, this.amount);
            PhotonNetwork.Instantiate(this.prefrabActionName, this.transform.position, Quaternion.LookRotation(Vector3.up), 0);
            this.audio.Play(2);

            this.time = 0;
            this.canExecute = false;
            return true;
        } else
        {
            return false;
        }
       
    }

    //================================
    public override void Return(GameObject target)
    {
        
    }
}
