using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomSpawningPool : SpawningPool<Room> {

	Room	m_prepare = null;
	RectTransform	m_topPannel;

	[SerializeField]
	bool	m_auto = false;

	override public void OnClickSpawn(Room obj)
	{
		if (m_prepare != null)
		{
			StartBuilding(m_prepare);
			m_prepare = null;
			return;
		}

		m_prepare = Spawn((int)obj.Type);
	}

	override public void StartBuilding(Room room)
	{
		base.StartBuilding(room);

		if (room.Type == SpawnObjType.RoomQueen)
			Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntQueen).PushCommand(new AICommand(AICommandType.GEN_ROOM, room.UID));
		else		
			Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntWorker).PushCommand(new AICommand(AICommandType.GEN_ROOM, room.UID));
	}

	void Awake()
	{
		m_topPannel = GameObject.Find("HudGUI/Canvas/TopPanel").GetComponent<RectTransform>();
	}

	void Start()
	{
		if (m_auto == true)
		{
			StartCoroutine(LoopAutoSpawnRoom());
		}
	}

	IEnumerator LoopAutoSpawnRoom()
	{
		while(true)
		{
			if (Helper.GetColony(Colony).AntSpawningPool.SpawnKeys.ContainsKey(SpawnObjType.AntWorker))
			{
				int foodRoomCount = 0;
				if (SpawnKeys.ContainsKey(SpawnObjType.RoomFood))
					foodRoomCount = SpawnKeys[SpawnObjType.RoomFood].Count;

				if (foodRoomCount <= Helper.GetColony(Colony).AntSpawningPool.SpawnKeys[SpawnObjType.AntWorker].Count/5)
				{
					string queenRoomUID = SpawnKeys[SpawnObjType.RoomQueen][0];
					Room queenRoom = GetSpawnedObject(queenRoomUID);
					float radian = Random.Range(0, Mathf.PI*2);

					Room room = Spawn((int)SpawnObjType.RoomFood);
					Point pt = Point.ToPoint(new Vector3(queenRoom.transform.position.x + Mathf.Cos(radian) * 3, queenRoom.transform.position.y + Mathf.Sin(radian) * 3, 0));

					room.transform.position = Point.ToVector(pt);
					StartBuilding(room);

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
