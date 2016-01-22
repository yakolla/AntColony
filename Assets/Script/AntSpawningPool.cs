using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntSpawningPool : SpawningPool<Ant> {


	override public void OnClickSpawn(int index)
	{
		StartBuilding(Spawn(index));
	}

	public void OnClickAttack()
	{
		if (Helper.GetNaturalEnemySpawningPool().SpawnObjects.Count == 0)
			return;

		NaturalEnemySpawningPool.PushCommand(Helper.GetNaturalEnemySpawningPool().SpawnObjects[Helper.GetNaturalEnemySpawningPool().SpawnKeys[0]]);
	}
}
