using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawningPool<T>  : MonoBehaviour where T : SpawnBaseObj  {

	Dictionary<Helper.SpawnObjType, List<string> >	m_keys = new Dictionary<Helper.SpawnObjType, List<string> >();
	List<Helper.SpawnObjType>		m_types = new List<Helper.SpawnObjType>();
	Dictionary<string, T>	m_objs = new Dictionary<string, T>();

	[SerializeField]
	GameObject []	m_prefObjs;

	virtual public void OnClickSpawn(int index){}

	public T Spawn(int index)
	{		
		GameObject obj = Instantiate(m_prefObjs[index]) as GameObject;
		obj.name = m_prefObjs[index].name;
		obj.GetComponent<T>().UID = System.Guid.NewGuid().ToString();
		return obj.GetComponent<T>();
	}

	virtual public void StartBuilding(T spawned)
	{
		if (false == m_keys.ContainsKey(spawned.Type))
		{
			m_types.Add(spawned.Type);			
			m_keys.Add(spawned.Type, new List<string>());
		}

		m_objs.Add(spawned.UID, spawned);
		m_keys[spawned.Type].Add(spawned.UID);
		spawned.transform.parent = transform;
		spawned.StartBuilding();
	}

	public T SpawnObject(string uid)
	{
		if (m_objs.ContainsKey(uid))
			return m_objs[uid];

		return null;
	}

	public void Kill(T spawned)
	{
		spawned.OnKill();
		SpawnKeys[spawned.Type].Remove(spawned.UID);
		m_objs.Remove(spawned.UID);
		GameObject.DestroyObject(spawned.gameObject);
	}

	public Dictionary<Helper.SpawnObjType, List<string> > SpawnKeys
	{
		get {return m_keys;}
	}

	public List<Helper.SpawnObjType> Types
	{
		get {return m_types;}
	}
}
