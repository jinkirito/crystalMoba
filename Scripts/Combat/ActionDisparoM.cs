using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDisparoM : Action
{
	private int speed;

	// ================================
	void Start()
	{
		this.speed = 8;
		this.timeOut = 2;
	}
	// ================================
	void OnTriggerEnter(Collider other)
	{
		GameObject otherplayer = other.gameObject;

		if (otherplayer != this.skill.gameObject)
		{
			CombatSystem otherCS = otherplayer.GetComponent<CombatSystem>();
			CombatSystem myCS = this.skill.GetCombatSystem();

			if (otherCS.GetTeam() != myCS.GetTeam())
			{
				this.skill.Return(other.gameObject);
				this.photonview.RPC("AutoDestroy", PhotonTargets.All, null);
			}
		}
	}
	// ===================================
	protected override void Tick()
	{
		this.transform.position += this.transform.forward * this.speed * Time.deltaTime;
	}
}