using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodSpawningPool : SpawningPool<Food> {


	override public void OnClickSpawn(int index)
	{
		StartBuilding(Spawn(index));
	}


}
