using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomSpawningPool : SpawningPool<Room> {

	Room	m_prepare = null;
	RectTransform	m_topPannel;

	[SerializeField]
	bool	m_auto = false;

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
		if (m_auto == true)
		{
			StartCoroutine(LoopAutoSpawnRoom());
		}
	}

	IEnumerator LoopAutoSpawnRoom()
	{
		while(true)
		{
			if (Helper.GetAntSpawningPool().SpawnKeys.ContainsKey(Helper.SpawnObjType.WorkerAnt))
			{
				int foodRoomCount = 0;
				if (Helper.GetRoomSpawningPool().SpawnKeys.ContainsKey(Helper.SpawnObjType.FoodRoom))
					foodRoomCount = Helper.GetRoomSpawningPool().SpawnKeys[Helper.SpawnObjType.FoodRoom].Count;

				if (foodRoomCount <= Helper.GetAntSpawningPool().SpawnKeys[Helper.SpawnObjType.WorkerAnt].Count/5)
				{
					string queenRoomUID = Helper.GetRoomSpawningPool().SpawnKeys[Helper.SpawnObjType.QueenRoom][0];
					Room queenRoom = Helper.GetRoomSpawningPool().GetSpawnedObject(queenRoomUID);
					float radian = Random.Range(0, Mathf.PI*2);

					Room room = Helper.GetRoomSpawningPool().Spawn(1);
					Point pt = Point.ToPoint(new Vector3(queenRoom.transform.position.x + Mathf.Cos(radian) * 3, queenRoom.transform.position.y + Mathf.Sin(radian) * 3, 0));

					room.transform.position = Point.ToVector(pt);
					Helper.GetRoomSpawningPool().StartBuilding(room);

				}
			}
			yield return new WaitForSeconds(5f);
		}

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
