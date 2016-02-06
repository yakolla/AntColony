using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodSpawningPool : SpawningPool<Food> {


	override public void OnClickSpawn(Food obj)
	{
		StartBuilding(Spawn((int)obj.Type), Helper.GetColony(Colony).GetRandomGround());
	}


}
