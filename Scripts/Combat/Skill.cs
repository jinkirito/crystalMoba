using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * La case Skill es una clase abstracta que es la que utilizara cada hablilidad.
 */

public abstract class Skill : MonoBehaviour
{

    
    public Transform firePoint;
    protected PhotonView playerPv;
    protected int manaCost;
    protected float coolDownTime;
    protected float time;
    protected bool canExecute;
    protected CombatSystem playerCS;
    public Sprite skillSprite;

    [HideInInspector]
    public Slider coolDownSlider;

    public void SetCombatSystem(CombatSystem _playerCs)
    {
        this.playerCS = _playerCs;
        this.coolDownSlider.value = 0;
    }

    public CombatSystem GetCombatSystem()
    {
        return this.playerCS;
    }

   public void SetPhotonView(PhotonView _pv)
    {
        this.playerPv = _pv;
    }

    public int GetManaCost()
    {
        return this.manaCost;
    }

    private void Update()
    {
        if (!this.canExecute)
        {
            this.time += Time.deltaTime;
            // hace que la barra de habilidad se vacie mostrando graficamente el tiempo que nos queda para usar la habilidada
            this.coolDownSlider.value =this.coolDownSlider.maxValue - ((this.coolDownSlider.maxValue * this.time) / this.coolDownTime);
            if(this.time >= this.coolDownTime)
            {
                this.canExecute = true;
            }
        }
    }

    //==============================================
    public abstract bool Execute();
    //si la accion golpea a ub objetivo pues avisa a la clase skill
    public abstract void Return(GameObject target);
    

    
}
