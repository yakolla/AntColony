using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntSoldier : Ant {


	override public void OnKill ()
	{

	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{		
		base.OnReachToGoal(target);

		ContinueNextAICommand();

	}
}
