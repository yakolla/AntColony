using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodPeace : Peace 
{
	Buff	m_buff;

	public FoodPeace(Buff	buff)
	{
		m_buff = buff;
	}

	public Buff Buff
	{
		get {return m_buff;}
	}
}
