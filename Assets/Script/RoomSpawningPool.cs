using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomSpawningPool : SpawningPool<Room> {

	Room	m_prepare = null;

	override public void OnClickSpawn(int index)
	{
		if (m_prepare != null)
		{
			StartBuilding(m_prepare);
			m_prepare = null;
			return;
		}

		m_prepare = Spawn(index);
	}

	override public void StartBuilding(Room room)
	{
		base.StartBuilding(room);

		if (room.Type == Helper.SpawnObjType.QueenRoom)
			Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.QueenAnt).PushCommand(new AICommand(AICommandType.GEN_ROOM, room.UID));
		else		
			Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.WorkerAnt).PushCommand(new AICommand(AICommandType.GEN_ROOM, room.UID));
	}

	void Start()
	{

	}

	void Update()
	{
		Helper.CheckTouched((touchedCount, touchPos)=>{
			for (int i = 0; i < touchedCount; ++i) 
			{
				if (m_prepare == null)
					break;
				
				m_prepare.transform.position = Camera.main.ScreenToWorldPoint(touchPos[i]);
				Vector3 pos = m_prepare.transform.position;
				pos.z = 0;
				m_prepare.transform.position = pos;
				break;
				
			}
		});
	}

}
