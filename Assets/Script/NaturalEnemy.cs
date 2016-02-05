using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NaturalEnemy : Ant {

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
		Helper.GetBackground().SetPixel(st.x, st.y, Helper.NATURAL_ENEMY_TILE);	
	}

	new void Update()
	{
		if (HP <= 0)
			Helper.GetColony(Colony).NaturalEnemySpawningPool.Kill(this);
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		base.OnReachToGoal(target);

		m_navigator.GoTo(SelectRandomRoom(false), false);
	}

}
