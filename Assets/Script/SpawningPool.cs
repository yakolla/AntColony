using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawningPool<T>  : MonoBehaviour where T : SpawnBaseObj  {

	Dictionary<string, T>	m_objects = new Dictionary<string, T>();
	List<string>	m_keys = new List<string>();

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
		m_objects.Add(spawned.UID, spawned);
		m_keys.Add(spawned.UID);
		spawned.transform.parent = transform;
		spawned.StartBuilding();
	}

	public Dictionary<string, T> SpawnObjects
	{
		get {return m_objects;}
	}

	public void Kill(T spawned)
	{
		spawned.OnKill();
		SpawnKeys.Remove(spawned.UID);
		SpawnObjects.Remove(spawned.UID);
		GameObject.DestroyObject(spawned.gameObject);
	}

	public List<string> SpawnKeys
	{
		get {return m_keys;}
	}
}
