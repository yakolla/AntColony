using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomQueen : Room {

	override public void StartBuilding()
	{
		base.StartBuilding();

		Ant ant = Helper.GetColony(Colony).AntSpawningPool.Spawn(SpawnObjType.AntQueen.ToString());
		Helper.GetColony(Colony).AntSpawningPool.StartBuilding(ant, Helper.GetColony(Colony).GetRandomGround());

	}

}
