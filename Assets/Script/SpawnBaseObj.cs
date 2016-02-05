using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class SpawnBaseObj : MonoBehaviour {

	[System.Serializable]
	public class SpawnBaseObjSerialData
	{
		public class Vector3SerialData
		{
			public float x;
			public float y;
			public float z;

			public Vector3SerialData(Vector3 value)
			{
				x = value.x;
				y = value.y;
				z = value.z;
			}
			
			public static implicit operator Vector3(Vector3SerialData d)  
			{
				return new Vector3(d.x, d.y, d.z);
			}

			public static implicit operator Vector3SerialData(Vector3 value) 
			{
				return new Vector3SerialData(value);
			}
		}

		public string m_uid;
		public int		m_maxHP = 4;
		public int		m_hp = 0;
		public Vector3SerialData	m_pos = Vector3.zero;
		public SpawnObjType m_type;
		public int		m_colony = 0;
	}

	[SerializeField]
	SpawnBaseObjSerialData	m_spawnBaseSerialData;

	public string UID
	{
		get { return m_spawnBaseSerialData.m_uid; }
		set { m_spawnBaseSerialData.m_uid = value; }
	}

	public int MaxHP
	{
		get {return m_spawnBaseSerialData.m_maxHP;}
		set {m_spawnBaseSerialData.m_maxHP = value;}
	}

	public int HP
	{
		get {return m_spawnBaseSerialData.m_hp;}
		set {m_spawnBaseSerialData.m_hp = value;}
	}

	public SpawnObjType Type
	{
		get {return m_spawnBaseSerialData.m_type;}
	}

	public int Colony
	{
		get {return m_spawnBaseSerialData.m_colony;}
		set {m_spawnBaseSerialData.m_colony = value;}
	}

	virtual public void Serialize(StreamWriter writer)
	{
		m_spawnBaseSerialData.m_pos = transform.position;
		writer.WriteLine(JsonConvert.SerializeObject(m_spawnBaseSerialData));
	}
	
	virtual public void Deserialize(StreamReader reader)
	{
		m_spawnBaseSerialData = JsonConvert.DeserializeObject<SpawnBaseObjSerialData>(reader.ReadLine());
		transform.position = m_spawnBaseSerialData.m_pos;
	}
	virtual public void StartBuilding(){}
	virtual public void OnKill(){}
	virtual public void OnReachToGoal(SpawnBaseObj target){}
}
