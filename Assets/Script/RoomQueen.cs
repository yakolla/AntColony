using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomQueen : Room {

	override public void StartBuilding()
	{
		base.StartBuilding();

		Ant ant = Helper.GetAntSpawningPool().Spawn((int)Helper.SpawnObjType.QueenAnt);
		Helper.GetAntSpawningPool().StartBuilding(ant);

	}

}
