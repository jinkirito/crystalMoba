using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSwordAtack : Action
{
    private int speed;

    private void Start()
    {
        this.speed = 1;
        this.timeOut = 2;
    }

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
