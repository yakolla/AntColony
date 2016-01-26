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
		if (Helper.GetNaturalEnemySpawningPool().SpawnKeys[Helper.SpawnObjType.NaturalEnemy].Count == 0)
			return;

		string uid = Helper.GetNaturalEnemySpawningPool().SpawnKeys[Helper.SpawnObjType.NaturalEnemy][0];
		AICommand cmd = new AICommand(AICommandType.GEN_NATURAL_ENEMY, uid);
		Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.NaturalEnemy).PushCommand(cmd);
	}
}
