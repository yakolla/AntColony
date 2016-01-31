using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Statistics : MonoBehaviour {

	[SerializeField]
	Text	m_antWorkersText;

	[SerializeField]
	Text	m_antSoldiersText;

	[SerializeField]
	Text	m_foodsText;


	// Update is called once per frame
	void Update () {
		if (Helper.GetAntSpawningPool().SpawnKeys.ContainsKey(Helper.SpawnObjType.WorkerAnt))
			m_antWorkersText.text = Helper.GetAntSpawningPool().SpawnKeys[Helper.SpawnObjType.WorkerAnt].Count.ToString();
		else
			m_antWorkersText.text = "0";

		if (Helper.GetAntSpawningPool().SpawnKeys.ContainsKey(Helper.SpawnObjType.SolderAnt))
			m_antSoldiersText.text = Helper.GetAntSpawningPool().SpawnKeys[Helper.SpawnObjType.SolderAnt].Count.ToString();
		else
			m_antSoldiersText.text = "0";

		if (Helper.GetRoomSpawningPool().SpawnKeys.ContainsKey(Helper.SpawnObjType.FoodRoom))
		{
			int count = 0;
			foreach(string uid in Helper.GetRoomSpawningPool().SpawnKeys[Helper.SpawnObjType.FoodRoom])
			{
				count += Helper.GetRoomSpawningPool().GetSpawnedObject(uid).CarryHolder.CarryCount;
			}

			m_foodsText.text = count.ToString();
		}
		else
			m_foodsText.text = "0";

	}


}
