using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//En esta clase Actionpocion solo indica el tiempo que tarda en lanzarze 
public class ActionPocion : Action
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

