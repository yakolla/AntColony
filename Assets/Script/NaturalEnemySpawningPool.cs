using UnityEngine;
using System.Collections;

public class NaturalEnemySpawningPool : SpawningPool<NaturalEnemy> {

	override public void OnClickSpawn(int index)
	{
		StartBuilding(Spawn(index));
	}

}
