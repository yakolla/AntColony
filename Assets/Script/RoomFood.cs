using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomFood : Room {

	CarryHolder	m_carryHolder = new CarryHolder();

	void Start () {
		m_carryHolder.Init(transform.Find("CarryHolder").GetComponent<SpriteRenderer>());
	}

	public void PutSomething(Texture2D tex)
	{
		m_carryHolder.PutSomething(tex);
	}
}
