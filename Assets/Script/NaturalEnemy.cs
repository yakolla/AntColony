using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NaturalEnemy : Ant {

	Colony[]	m_colonys;

	override public void StartBuilding()
	{
		base.StartBuilding();

		m_colonys = GameObject.Find("Colonys").GetComponentsInChildren<Colony>();
	}

	new void Update()
	{
		if (HP <= 0)
			Helper.GetColony(Colony).NaturalEnemySpawningPool.Kill(this);
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		base.OnReachToGoal(target);

		m_navigator.GoTo(m_colonys[Random.Range(0, m_colonys.Length)].SelectRandomRoom(false), false);
	}

}
