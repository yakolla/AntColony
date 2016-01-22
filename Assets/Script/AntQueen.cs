using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntQueen : Ant {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () 
	{
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		AICommand cmd = Helper.GetBackground().AICommandQueue(m_type).PopCommand();
		if (cmd != null)
		{
			GetComponent<AntNavigator>().GoTo(cmd.Target, cmd.Digy);
		}
	}

	override public void OnKill ()
	{

	}

}
