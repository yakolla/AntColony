using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnBaseObj : MonoBehaviour {

	string m_uid;
	[SerializeField]
	int		m_maxHP = 4;
	int		m_hp = 0;

	[SerializeField]
	protected Helper.SpawnObjType m_type;

	public string UID
	{
		get { return m_uid; }
		set { m_uid = value; }
	}

	public int MaxHP
	{
		get {return m_maxHP;}
	}

	public int HP
	{
		get {return m_hp;}
		set {m_hp = value;}
	}

	public Helper.SpawnObjType Type
	{
		get {return m_type;}
	}

	virtual public void StartBuilding(){}
	virtual public void OnKill(){}
	virtual public void OnReachToGoal(SpawnBaseObj target){}
}
