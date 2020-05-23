using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//En esta clase ActionHeal solo indica el tiempo que tarda en lanzarze 

public class ActionHeal : Action
{

	// ==================================
	void Start()
	{
		this.timeOut = 2;
	}

	// ==================================
	protected override void Tick()
	{

	}
}
