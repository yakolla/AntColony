using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Statistics : MonoBehaviour {

	[SerializeField]
	Text	m_antWorkersText = null;

	[SerializeField]
	Text	m_antSoldiersText = null;

	[SerializeField]
	Text	m_foodsText = null;


	// Update is called once per frame
	void Update () {
		m_antWorkersText.text = Helper.GetColony(Helper.MY_COLONY).AntSpawningPool.GetCount(SpawnObjType.AntWorker).ToString() + "/" + Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.GetCount(SpawnObjType.RoomAntWorker)*Helper.CAPACITY_ROOM;

		m_antSoldiersText.text = Helper.GetColony(Helper.MY_COLONY).AntSpawningPool.GetCount(SpawnObjType.AntSoldier).ToString() + "/" + Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.GetCount(SpawnObjType.RoomAntSoldier)*Helper.CAPACITY_ROOM;


		List<string> uids = Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.UIDs(SpawnObjType.RoomFood);
		if (uids != null)
		{
			int maxCarryCount = 0;
			int count = 0;
			foreach(string uid in uids)
			{
				Room room = Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.GetSpawnedObject(uid);
				if (room.HasPath == false)
					continue;

				count += room.CarryHolder.CarryCount;
				maxCarryCount += room.CarryHolder.MaxCarryCount;
			}

			m_foodsText.text = count.ToString() + "/" + maxCarryCount;
		}
		else
		{
			m_foodsText.text = "0/0";
		}
	}


}
