using UnityEngine;
using System.Collections;

public class NaturalEnemySpawningPool : SpawningPool<NaturalEnemy> {

	override public void OnClickSpawn(string prefName)
	{
		StartBuilding(Spawn(prefName), Helper.GetColony(Colony).GetRandomGround());
	}

}
