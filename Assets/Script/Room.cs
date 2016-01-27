using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Room : SpawnBaseObj {

	[SerializeField]
	protected Point			m_roomSize;

	protected bool	m_hasPath = false;

	CarryHolder	m_carryHolder = new CarryHolder();

	void Awake()
	{
		m_carryHolder.Init(this, transform.Find("CarryHolder").GetComponent<SpriteRenderer>(), m_roomSize.y/Helper.ONE_PEACE_SIZE, m_roomSize.x/Helper.ONE_PEACE_SIZE, m_roomSize.x*m_roomSize.y/(Helper.ONE_PEACE_SIZE*Helper.ONE_PEACE_SIZE));
	}

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
		for(int y = 0; y < 4; ++y)
		{
			for(int x = 0; x < 4; ++x)
			{
				//Helper.GetBackground().SetPixel(st.x+x, st.y+y, Helper.ROOM_TILE);
			}
		}
		Helper.GetBackground().SetPixel(st.x, st.y, Helper.OPEN_TILE);
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
		get {return m_hasPath;}
		set {m_hasPath = value;}
	}
}
