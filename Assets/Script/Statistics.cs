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
		if (Helper.GetColony(Helper.MY_COLONY).AntSpawningPool.SpawnKeys.ContainsKey(SpawnObjType.AntWorker))
			m_antWorkersText.text = Helper.GetColony(Helper.MY_COLONY).AntSpawningPool.SpawnKeys[SpawnObjType.AntWorker].Count.ToString();
		else
			m_antWorkersText.text = "0";

		if (Helper.GetColony(Helper.MY_COLONY).AntSpawningPool.SpawnKeys.ContainsKey(SpawnObjType.AntSoldier))
			m_antSoldiersText.text = Helper.GetColony(Helper.MY_COLONY).AntSpawningPool.SpawnKeys[SpawnObjType.AntSoldier].Count.ToString();
		else
			m_antSoldiersText.text = "0";

		if (Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.SpawnKeys.ContainsKey(SpawnObjType.RoomFood))
		{
			int count = 0;
			foreach(string uid in Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.SpawnKeys[SpawnObjType.RoomFood])
			{
				count += Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.GetSpawnedObject(uid).CarryHolder.CarryCount;
			}

			m_foodsText.text = count.ToString();
		}
		else
			m_foodsText.text = "0";

	}


}
