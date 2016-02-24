using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



public class Room : SpawnBaseObj {

	[System.Serializable]
	public class RoomSerialData
	{
		public Point 	m_roomSize;
		
		public bool		m_hasPath = false;
		public int		m_progress = 0;
	}
	[SerializeField]
	RoomSerialData	m_roomSerialData;

	CarryHolder	m_carryHolder = new CarryHolder();

	SpriteRenderer	m_render;

	void Awake()
	{
		m_carryHolder.Init(this, transform.Find("CarryHolder").GetComponent<SpriteRenderer>(), m_roomSerialData.m_roomSize.y, m_roomSerialData.m_roomSize.x);
		m_render = transform.Find("Body").GetComponent<SpriteRenderer>();
	}

	override public void StartBuilding()
	{
		HP = MaxHP;
	}

	public void Update()
	{
		m_carryHolder.Update();
	}

	public CarryHolder CarryHolder
	{
		get { return m_carryHolder; }
	}

	public bool HasPath
	{
		get {return m_roomSerialData.m_hasPath;}
		set {
			m_roomSerialData.m_hasPath = value;
			if (m_roomSerialData.m_hasPath == true)
			{
				Progress+=10;
			}
		}
	}

	public int Progress
	{
		get {return m_roomSerialData.m_progress;}
		set {
			m_roomSerialData.m_progress = value;
			m_roomSerialData.m_progress = Mathf.Min(100, m_roomSerialData.m_progress);
			Color color = m_render.color;
			color.a = m_roomSerialData.m_progress/100f;
			m_render.color = color;
		}
	}

	override public void Serialize(StreamWriter writer)
	{
		base.Serialize(writer);
		writer.WriteLine(JsonConvert.SerializeObject(m_roomSerialData));
	}

	override public void Deserialize(StreamReader reader)
	{
		base.Deserialize(reader);
		m_roomSerialData = JsonConvert.DeserializeObject<RoomSerialData>(reader.ReadLine());
	}
}
