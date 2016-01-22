using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AICommandType
{
	FOOD,
	ROOM,
	NATURAL_ENEMY,
}

public class AICommand {

	AICommandType	m_type;
	int		m_priority;
	Vector3	m_pos;
	SpawnBaseObj	m_target;
	bool m_digy = false;

	public AICommand(AICommandType type, bool digy)
	{
		m_type = type;
		m_digy = digy;
	}

	public AICommandType CommandType
	{
		get {return m_type;}
	}

	public Vector3	Position
	{
		get { return m_pos; }
	}

	public int Priority
	{
		get { return m_priority; }
		set { m_priority = value; }
	}

	public SpawnBaseObj Target
	{
		get {return m_target;}
		set {
			m_target = value;
			m_pos = Target.transform.position;
		}
	}

	public bool Digy
	{
		get {return m_digy;}
	}
}
