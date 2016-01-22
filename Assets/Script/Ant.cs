using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ant : SpawnBaseObj {




	protected Room SelectRandomRoom()
	{
		int roomCount = Helper.GetRoomSpawningPool().SpawnObjects.Count;
		if (roomCount == 0)
			return null;

		return Helper.GetRoomSpawningPool().SpawnObjects[Helper.GetRoomSpawningPool().SpawnKeys[Random.Range(0, roomCount)]].GetComponent<Room>();
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		bool digy = true;
		
		AICommand cmd = Helper.GetBackground().AICommandQueue(m_type).PopCommand();
		if (cmd == null)
		{
			target = SelectRandomRoom();
		}
		else
		{
			if (target != null && target.Type == Helper.SpawnObjType.Food)
			{
				target = SelectRandomRoom();
				digy = false;
			}
			else
			{
				target = cmd.Target;
				digy = cmd.Digy;
			}

		}

		GetComponent<AntNavigator>().GoTo(target, digy);
	}


}
