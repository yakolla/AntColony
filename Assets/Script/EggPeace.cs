using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EggPeace : Peace {

	float	m_hatchOutTime = 1f*6;

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
		Ant ant = null;
		SpawnObjType randomType = Helper.GetColony(m_colony).AntSpawningPool.RandomSpawnType(SpawnObjType.AntWorker);
		switch(randomType)
		{
		case SpawnObjType.AntWorker:
			if (Helper.GetColony(m_colony).RoomSpawningPool.GetCount(SpawnObjType.RoomAntWorker) == 0)
			{
				if (Helper.GetColony(m_colony).AntSpawningPool.GetCount(randomType) < Helper.CAPACITY_ROOM )
					ant = Helper.GetColony(m_colony).AntSpawningPool.Spawn(randomType.ToString());
			}
			else if (Helper.GetColony(m_colony).AntSpawningPool.GetCount(randomType) < Helper.GetColony(m_colony).RoomSpawningPool.GetCount(SpawnObjType.RoomAntWorker)*Helper.CAPACITY_ROOM )
				ant = Helper.GetColony(m_colony).AntSpawningPool.Spawn(randomType.ToString());

			break;
		case SpawnObjType.AntSoldier:
			if (Helper.GetColony(m_colony).AntSpawningPool.GetCount(randomType) < Helper.GetColony(m_colony).RoomSpawningPool.GetCount(SpawnObjType.RoomAntSoldier)*Helper.CAPACITY_ROOM )
				ant = Helper.GetColony(m_colony).AntSpawningPool.Spawn(randomType.ToString());
			break;
		}

		if (ant != null)
		{
			Helper.GetColony(m_colony).AntSpawningPool.StartBuilding(ant, m_holder.Onwer.transform.position);
		}

		base.OnKill();
	}

}
