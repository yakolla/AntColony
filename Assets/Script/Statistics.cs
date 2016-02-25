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

		Point count = Helper.GetColony(Helper.MY_COLONY).RoomSpawningPool.GetCapacityCount(SpawnObjType.RoomFood);
		
		m_foodsText.text = count.x + "/" + count.y;

	}


}
