using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AICommandType
{
	GEN_NATURAL_ENEMY,
	GEN_ROOM,
	GEN_FOOD,
	GEN_EGG,
	EAT_FOOD,
}

public class AICommand {

	AICommandType	m_type;
	int		m_priority = 1;
	string	m_uid;

	public AICommand(AICommandType type, string uid)
	{
		m_type = type;
		m_priority = (int)type;
		m_uid = uid;
	}

	public AICommandType CommandType
	{
		get {return m_type;}
	}



	public int Priority
	{
		get { return m_priority; }
	}

	public string UID
	{
		get {return m_uid;}
	}
}
