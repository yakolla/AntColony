using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EggPeace : CarryAble {

	float	m_hatchOutTime = 1f*5;

	bool	m_startedBuilding = false;

	override public void Start(Texture2D tex)
	{
		base.Start(tex);
		m_startedBuilding = true;
		m_hatchOutTime += Time.time;
	}

	override public void Update()
	{
		if (m_startedBuilding == false)
			return;

		if (m_hatchOutTime < Time.time)
		{
			OnKill();
		}
	}

	override public void OnKill()
	{

		Ant ant = Helper.GetColony(m_colony).AntSpawningPool.Spawn(Helper.GetColony(m_colony).AntSpawningPool.RandomSpawnType(SpawnObjType.AntWorker));
		if (ant != null)
		{
			Helper.GetColony(m_colony).AntSpawningPool.StartBuilding(ant, m_holder.Onwer.transform.position);
		}

		base.OnKill();
	}

}
