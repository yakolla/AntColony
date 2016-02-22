using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class Colony  : MonoBehaviour{
	
	RoomSpawningPool	m_roomSpawningPool;
	NaturalEnemySpawningPool	m_naturalEnemySpawningPool;
	AntSpawningPool	m_antSpawningPool;
	FoodSpawningPool	m_foodSpawningPool;

	AICommandQueue[]	m_antAICommandQueue = new AICommandQueue[(int)SpawnObjType.Count];

	[SerializeField]
	Rect		m_area;

	Point		m_startPoint;

	// Use this for initialization
	public void Awake () {
		m_roomSpawningPool = transform.Find("Rooms").GetComponent<RoomSpawningPool>();
		m_naturalEnemySpawningPool = transform.Find("Enemys").GetComponent<NaturalEnemySpawningPool>();
		m_antSpawningPool = transform.Find("Ants").GetComponent<AntSpawningPool>();
		m_foodSpawningPool = transform.Find("Foods").GetComponent<FoodSpawningPool>();

		for(int i = 0; i < (int)SpawnObjType.Count; ++i)
		{
			m_antAICommandQueue[i] = new AICommandQueue();
		}
	}

	public void Serialize()
	{
		MemoryStream stream = new MemoryStream();
		
		StreamWriter writer = new StreamWriter(stream);

		Room[] rooms = m_roomSpawningPool.GetComponentsInChildren<Room>();
		int roomCount = rooms.Length;

		writer.WriteLine(roomCount);
		for(int i = 0; i < roomCount; ++i)
		{
			writer.WriteLine(JsonConvert.SerializeObject(rooms[i].Type));
			rooms[i].Serialize(writer);
		}

		writer.Close();

		PlayerPrefs.SetString("data", System.Text.Encoding.UTF8.GetString(stream.ToArray()));
	}

	public void Deserialize()
	{
		m_roomSpawningPool.Clear();

		byte[] data = System.Text.Encoding.UTF8.GetBytes(PlayerPrefs.GetString("data"));
		MemoryStream stream = new MemoryStream(data);
		
		StreamReader reader = new StreamReader(stream);
		
		int roomCount = JsonConvert.DeserializeObject<int>(reader.ReadLine());
		for(int i = 0; i < roomCount; ++i)
		{
			SpawnObjType type = JsonConvert.DeserializeObject<SpawnObjType>(reader.ReadLine());
			Room room = m_roomSpawningPool.Spawn(type.ToString());
			room.Deserialize(reader);
			m_roomSpawningPool.StartBuilding(room, room.transform.position);
		}

		reader.Close();

	}

	public RoomSpawningPool RoomSpawningPool
	{
		get {return m_roomSpawningPool;}
	}

	public NaturalEnemySpawningPool NaturalEnemySpawningPool
	{
		get {return m_naturalEnemySpawningPool;}
	}

	public AntSpawningPool AntSpawningPool
	{
		get {return m_antSpawningPool;}
	}

	public FoodSpawningPool FoodSpawningPool
	{
		get {return m_foodSpawningPool;} 
	}

	public AICommandQueue AICommandQueue(SpawnObjType type)
	{
		return m_antAICommandQueue[(int)type];
	}

	public Vector3	GetRandomGround()
	{
		Point pt;
		pt.x = (int)Random.Range(m_area.x, m_area.x+m_area.width);
		pt.y = 0;
		return Point.ToVector(pt);
	}

	public Rect Area
	{
		get {return m_area;}
	}

	public Room SelectRandomRoom(bool digy)
	{
		int roomTypeCount = RoomSpawningPool.Types.Count;
		if (roomTypeCount == 0)
			return null;
		
		SpawnObjType randType = RoomSpawningPool.Types[Random.Range(0, roomTypeCount)];
		return SelectRandomRoom(randType, digy);
		
	}
	
	public Room SelectRandomRoom(SpawnObjType type, bool digy)
	{
		if (false == RoomSpawningPool.Types.Contains(type))
			return null;
		
		int roomCount = RoomSpawningPool.SpawnKeys[type].Count;
		if (roomCount == 0)
			return null;
		
		string randKey = RoomSpawningPool.SpawnKeys[type][Random.Range(0, roomCount)];
		
		Room room = RoomSpawningPool.GetSpawnedObject(randKey).GetComponent<Room>();
		if (digy == false && room.HasPath == false)
			return null;
		
		return room;
	}

	public Point StartPoint
	{
		get { return m_startPoint; }
		set { m_startPoint = value; }
	}
}
