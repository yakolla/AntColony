using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodSpawningPool : SpawningPool<Food> {


	override public void OnClickSpawn(int index)
	{
		Food food = Spawn(index);
		StartBuilding(food);
	}

	override public void StartBuilding(Food spawned)
	{
		base.StartBuilding(spawned);

		AICommand cmd = new AICommand(AICommandType.FOOD, false);
		cmd.Priority = 1;
		cmd.Target = spawned;
		Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.WorkerAnt).PushCommand(cmd);
	}

}
