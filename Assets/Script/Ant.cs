using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ant : SpawnBaseObj {

	protected AntNavigator	m_navigator;

	public void Awake()
	{
		m_navigator = GetComponent<AntNavigator>();
	}

	protected Room SelectRandomRoom(bool digy)
	{
		int roomTypeCount = Helper.GetRoomSpawningPool().Types.Count;
		if (roomTypeCount == 0)
			return null;

		Helper.SpawnObjType randType = Helper.GetRoomSpawningPool().Types[Random.Range(0, roomTypeCount)];
		return SelectRandomRoom(randType, digy);
	
	}

	protected Room SelectRandomRoom(Helper.SpawnObjType type, bool digy)
	{
		if (false == Helper.GetRoomSpawningPool().Types.Contains(type))
			return null;

		int roomCount = Helper.GetRoomSpawningPool().SpawnKeys[type].Count;
		if (roomCount == 0)
			return null;

		string randKey = Helper.GetRoomSpawningPool().SpawnKeys[type][Random.Range(0, roomCount)];

		Room room = Helper.GetRoomSpawningPool().SpawnObject(randKey).GetComponent<Room>();
		if (digy == false && room.HasPath == false)
			return null;

		return room;
	}

	public void ContinueNextAICommand()
	{
		bool digy = true;
		SpawnBaseObj target = null;

		AICommand cmd = Helper.GetBackground().AICommandQueue(m_type).PopCommand();
		if (cmd != null)
		{
			switch(cmd.CommandType)
			{
			case AICommandType.GEN_EGG:
				target = Helper.GetRoomSpawningPool().SpawnObject(cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_FOOD:
				target = Helper.GetFoodSpawningPool().SpawnObject(cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_NATURAL_ENEMY:
				target = Helper.GetNaturalEnemySpawningPool().SpawnObject(cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_ROOM:
				target = Helper.GetRoomSpawningPool().SpawnObject(cmd.UID);
				digy = true;
				break;
			}
		}
		else
		{
			target = SelectRandomRoom(digy=m_navigator.Digy);
		}
		
		m_navigator.GoTo(target, digy);
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		if (target != null && target.GetComponent<Room>() != null)
			target.GetComponent<Room>().HasPath = true;
	}


}
