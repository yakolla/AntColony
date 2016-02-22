using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomSpawningPool : SpawningPool<Room> {

	Room	m_prepare = null;
	RectTransform	m_topPannel;

	[SerializeField]
	bool	m_auto = false;

	[SerializeField]
	bool	m_ai = false;

	List<Point>	m_spots = new List<Point>();

	const int GAP_SIZE = 10;

	override public void OnClickSpawn(string prefName)
	{
		if (m_prepare != null)
		{
			StartBuilding(m_prepare, m_prepare.transform.position);
			m_prepare = null;
			return;
		}

		m_prepare = Spawn(prefName);
	}

	override public void StartBuilding(Room room, Vector3 pos)
	{
		base.StartBuilding(room, pos);

		if (room.Type == SpawnObjType.RoomQueen)
		{
			Point roomPt = Point.ToPoint(pos);

			Point startPt;
			startPt.x = roomPt.x;
			startPt.y = 1;
			Helper.GetColony(Colony).StartPoint = startPt;

			int[] angs = {60, 120, 240, 300};
			Rect area = Helper.GetColony(Colony).Area;
			for (int y = GAP_SIZE-roomPt.y; y < area.height; y+=GAP_SIZE*3)
			{
				for (int x = GAP_SIZE-roomPt.x; x < area.width; x+=GAP_SIZE*3)
				{
					for (int i = 0; i < angs.Length; ++i)
					{
						Point pt = new Point();
						pt.x = (int)(area.x+x+Mathf.Cos(Random.Range(angs[i]-30, angs[i]+30)*Mathf.Deg2Rad)*Random.Range(0, GAP_SIZE));
						pt.y = (int)(area.y+y+Mathf.Sin(Random.Range(angs[i]-30, angs[i]+30)*Mathf.Deg2Rad)*Random.Range(0, GAP_SIZE));
						//pt.x = (int)(area.x+x);
						//pt.y = (int)(area.y+y);

						if (pt.x < area.x || (area.width+area.x) <= pt.x || true == Helper.GetBackground().UnableTo(pt.x, pt.y))
							continue;
						m_spots.Add(pt);
						Debug.Log("Colony:"+ Colony + " pt:" + pt);
					}
					
				}
			}

			Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntQueen).PushCommand(new AICommand(AICommandType.GEN_ROOM, room.UID));
		}
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
			if (m_ai == true)
			{
				Rect area = Helper.GetColony(Colony).Area;
				Room room = Spawn(SpawnObjType.RoomQueen.ToString());
				Point pt;
				pt.x = (int)(area.x+area.width/2);
				pt.y = (int)(area.y+area.height/2);
				StartBuilding(room, Point.ToVector(pt));
			}
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

				int antWorkerRoomCount = 0;
				if (SpawnKeys.ContainsKey(SpawnObjType.RoomAntWorker))
					antWorkerRoomCount = SpawnKeys[SpawnObjType.RoomAntWorker].Count;

				if (eggRoomCount < 1)
				{
					if (m_spots.Count == 0)
						break;

					int randRoom = Random.Range(0, m_spots.Count);
					
					Room room = Spawn(SpawnObjType.RoomEgg.ToString());
					StartBuilding(room, Point.ToVector(m_spots[randRoom]));
					m_spots.RemoveAt(randRoom);
				}
				else if (foodRoomCount <= Helper.GetColony(Colony).AntSpawningPool.SpawnKeys[SpawnObjType.AntWorker].Count/5)
				{
					if (m_spots.Count == 0)
						break;

					int randRoom = Random.Range(0, m_spots.Count);
					Room room = Spawn(SpawnObjType.RoomFood.ToString());

					StartBuilding(room, Point.ToVector(m_spots[randRoom]));
					m_spots.RemoveAt(randRoom);
				}
				else if (antWorkerRoomCount <= Helper.GetColony(Colony).AntSpawningPool.SpawnKeys[SpawnObjType.AntWorker].Count/10)
				{
					if (m_spots.Count == 0)
						break;

					int randRoom = Random.Range(0, m_spots.Count);
					Room room = Spawn(SpawnObjType.RoomAntWorker.ToString());

					StartBuilding(room, Point.ToVector(m_spots[randRoom]));
					m_spots.RemoveAt(randRoom);
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
