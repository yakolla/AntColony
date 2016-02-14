using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomSpawningPool : SpawningPool<Room> {

	Room	m_prepare = null;
	RectTransform	m_topPannel;

	[SerializeField]
	bool	m_auto = false;

	List<Vector3>	m_spots = new List<Vector3>();

	const int GAP_SIZE = 2;

	override public void OnClickSpawn(Room obj)
	{
		if (m_prepare != null)
		{
			StartBuilding(m_prepare, m_prepare.transform.position);
			m_prepare = null;
			return;
		}

		m_prepare = Spawn((int)obj.Type);
	}

	override public void StartBuilding(Room room, Vector3 pos)
	{
		base.StartBuilding(room, pos);

		Rect area = Helper.GetColony(Colony).Area;
		for (int y = 4; y < area.height-4; y+=GAP_SIZE*2)
		{
			for (int x = 4; x < area.width; x+=GAP_SIZE*2)
			{
				m_spots.Add(new Vector3(area.x+x, -area.y-y, 0));
			}
		}


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
				int eggRoomCount = 0;
				if (SpawnKeys.ContainsKey(SpawnObjType.RoomEgg))
					eggRoomCount = SpawnKeys[SpawnObjType.RoomEgg].Count;

				int foodRoomCount = 0;
				if (SpawnKeys.ContainsKey(SpawnObjType.RoomFood))
					foodRoomCount = SpawnKeys[SpawnObjType.RoomFood].Count;

				if (eggRoomCount < 1)
				{
					if (m_spots.Count == 0)
						break;

					float radian = Random.Range(0, Mathf.PI*2);
					
					Room room = Spawn((int)SpawnObjType.RoomEgg);
					Vector3 spot = m_spots[0];
					Point pt = Point.ToPoint(new Vector3(spot.x + Mathf.Cos(radian) * GAP_SIZE, spot.y + Mathf.Sin(radian) * GAP_SIZE, 0));
					StartBuilding(room, Point.ToVector(pt));
					m_spots.RemoveAt(0);
				}
				else if (foodRoomCount <= Helper.GetColony(Colony).AntSpawningPool.SpawnKeys[SpawnObjType.AntWorker].Count/5)
				{
					if (m_spots.Count == 0)
						break;

					float radian = Random.Range(0, Mathf.PI*2);

					Room room = Spawn((int)SpawnObjType.RoomFood);

					Vector3 spot = m_spots[0];
					Point pt = Point.ToPoint(new Vector3(spot.x + Mathf.Cos(radian) * GAP_SIZE, spot.y + Mathf.Sin(radian) * GAP_SIZE, 0));

					StartBuilding(room, Point.ToVector(pt));
					m_spots.RemoveAt(0);
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
