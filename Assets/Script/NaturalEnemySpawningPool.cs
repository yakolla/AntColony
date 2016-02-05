using UnityEngine;
using System.Collections;

public class NaturalEnemySpawningPool : SpawningPool<NaturalEnemy> {

	override public void OnClickSpawn(NaturalEnemy obj)
	{
		StartBuilding(Spawn((int)SpawnObjType.NaturalEnemy));
	}

}
