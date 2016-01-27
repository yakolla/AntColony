using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomSpawningPool : SpawningPool<Room> {

	Room	m_prepare = null;
	RectTransform	m_topPannel;

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

	void Awake()
	{
		m_topPannel = GameObject.Find("HudGUI/Canvas/TopPanel").GetComponent<RectTransform>();
	}

	void Update()
	{
		Helper.CheckTouchDraging((touchedCount, touchPos, touchPhase)=>{
			if (touchedCount == 0)
				return;

			if (touchPos.y > Screen.height-m_topPannel.rect.height)
				return;

			if (m_prepare == null)
				return;
				
			m_prepare.transform.position = Camera.main.ScreenToWorldPoint(touchPos);
			Vector3 pos = m_prepare.transform.position;
			pos.z = 0;
			m_prepare.transform.position = pos;
				

		});
	}

}
