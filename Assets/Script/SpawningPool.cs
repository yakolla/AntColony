using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpawnRatio
{
	public SpawnObjType m_spawnType;
	public int m_ratio;
}

public class SpawningPool<T>  : MonoBehaviour where T : SpawnBaseObj  {

	Dictionary<SpawnObjType, List<string> >	m_keys = new Dictionary<SpawnObjType, List<string> >();
	List<SpawnObjType>		m_types = new List<SpawnObjType>();
	Dictionary<string, T>	m_objs = new Dictionary<string, T>();

	protected Dictionary<string, GameObject>	m_prefObjs = new Dictionary<string, GameObject>();

	[SerializeField]
	int	m_maxSpawnCount = 0;

	[SerializeField]
	int m_colony = 0;
	
	[SerializeField]
	SpawnRatio[]		m_spawnRatios = null;
	
	int			m_maxRatio;

	virtual public void OnClickSpawn(string prefName){}

	void Awake()
	{
		for(int i = 0 ; i < m_spawnRatios.Length; ++i)
		{
			int ratio = m_spawnRatios[i].m_ratio;
			m_spawnRatios[i].m_ratio += m_maxRatio;
			m_maxRatio += ratio;
		}
	}

	public T Spawn(string prefName)
	{	
		if (m_maxSpawnCount > 0 && m_maxSpawnCount <= m_objs.Count)
			return null;

		if (m_prefObjs.ContainsKey(prefName) == false)
			m_prefObjs.Add(prefName, Resources.Load("Pref/"+prefName) as GameObject);


		GameObject obj = Instantiate(m_prefObjs[prefName]) as GameObject;
		obj.name = m_prefObjs[prefName].name;
		T spawnObj = obj.GetComponent<T>();
		spawnObj.UID = System.Guid.NewGuid().ToString();
		spawnObj.Colony = Colony;
		return spawnObj;
	}

	virtual public void StartBuilding(T spawned, Vector3 pos)
	{
		if (spawned == null)
			return;

		if (false == m_keys.ContainsKey(spawned.Type))
		{
			m_types.Add(spawned.Type);			
			m_keys.Add(spawned.Type, new List<string>());
		}

		m_objs.Add(spawned.UID, spawned);
		m_keys[spawned.Type].Add(spawned.UID);
		spawned.transform.parent = transform;
		spawned.transform.position = pos;
		spawned.StartBuilding();
	}

	public T GetSpawnedObject(string uid)
	{
		if (m_objs.ContainsKey(uid))
			return m_objs[uid];

		return null;
	}

	public T PopSpawnedObject(string uid)
	{
		T spawned = null;
		if (m_objs.ContainsKey(uid))
		{
			spawned = m_objs[uid];
			m_objs.Remove(uid);
			m_keys[spawned.Type].Remove(uid);
		}
		
		return spawned;
	}

	public void Kill(T spawned)
	{
		spawned.OnKill();
		SpawnKeys[spawned.Type].Remove(spawned.UID);
		m_objs.Remove(spawned.UID);
		GameObject.DestroyObject(spawned.gameObject);
	}

	public void Clear()
	{
		List<string> uids = new List<string>(m_objs.Keys);
		foreach(string uid in uids)
		{
			Kill(GetSpawnedObject(uid));
		}

		m_keys.Clear();
		m_types.Clear();
		m_objs.Clear();
	}

	public SpawnObjType RandomSpawnType(SpawnObjType defaultSpawnType)
	{
		int random = Random.Range(1, m_maxRatio);
		for(int i = 0 ; i < m_spawnRatios.Length; ++i)
		{
			if (random < m_spawnRatios[i].m_ratio)
			{
				return m_spawnRatios[i].m_spawnType;
			}
		}
		
		return defaultSpawnType;
	}

	public Dictionary<SpawnObjType, List<string> > SpawnKeys
	{
		get {return m_keys;}
	}

	public List<SpawnObjType> Types
	{
		get {return m_types;}
	}

	public int Colony
	{
		get {return m_colony;}
	}

	public void MoveToColony(SpawningPool<T> pool)
	{
		List<string> uids = new List<string>(m_objs.Keys);
		foreach(string uid in uids)
		{
			T spawend = GetSpawnedObject(uid);
			if (spawend.Type == SpawnObjType.RoomQueen)
				continue;

			spawend = PopSpawnedObject(uid);
			spawend.Colony = pool.Colony;
			pool.StartBuilding(spawend, spawend.transform.position);
		}

		Clear();
	}

	public int MaxSpawnCount
	{
		set { m_maxSpawnCount = value; }
		get { return m_maxSpawnCount; }
	}
}
