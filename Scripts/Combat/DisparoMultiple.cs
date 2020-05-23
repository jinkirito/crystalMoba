using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoMultiple : Skill
{
	private int damage;
	private string prefabActionName;

	// ===============================
	void Start()
	{
		this.prefabActionName = "Bala";
		this.damage = -10;
		this.manaCost = 20;
		this.coolDownTime = 1.5f;
		this.time = this.coolDownTime;
	}
	// ===============================
	public override bool Execute()
	{
		// el disparo hace un arco rotando el Firepoint del personaje desde -15 hast a 20 y una vez rotado el fireponit 
		// se intancia el prefab en varios 
		if (this.canExecute)
		{
			Quaternion tmpRotation = this.firePoint.rotation;

			for (float angle = -15; angle <= 20; angle += 3)
			{
				this.firePoint.Rotate(this.firePoint.up * angle);

				GameObject ball = PhotonNetwork.Instantiate(this.prefabActionName, this.firePoint.position,
									this.firePoint.rotation, 0);

				ball.GetComponent<ActionDisparar>().SetSkill(this);
				ball.GetComponent<Collider>().enabled = true;

				this.time = 0;
				this.canExecute = false;
			}
			this.firePoint.rotation = tmpRotation;
			return true;
		}
		else
		{
			return false;
		}
	}
	// ===============================
	public override void Return(GameObject target)
	{
		PhotonView targetPV = target.GetComponent<PhotonView>();
		targetPV.RPC("ModifyHealth", PhotonTargets.All, this.damage);
	}
}