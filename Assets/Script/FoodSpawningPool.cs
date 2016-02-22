using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodSpawningPool : SpawningPool<Food> {


	override public void OnClickSpawn(string prefName)
	{
		StartBuilding(Spawn(prefName), Helper.GetColony(Colony).GetRandomGround());
	}
}
