using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Room : SpawnBaseObj {

	protected bool	m_doneBuilding = false;

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
		for(int y = 0; y < 4; ++y)
		{
			for(int x = 0; x < 4; ++x)
			{
				Helper.GetBackground().SetPixel(st.x+x, st.y+y, Helper.ROOM_TILE);
			}
		}
		Helper.GetBackground().SetPixel(st.x, st.y, Helper.OPEN_TILE);
		HP = MaxHP;
		m_doneBuilding = true;
	}

}
