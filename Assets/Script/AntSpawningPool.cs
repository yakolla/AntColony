using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntSpawningPool : SpawningPool<Ant> {




	override public void OnClickSpawn(Ant obj)
	{
		StartBuilding(Spawn((int)obj.Type));
	}

	public void OnClickAttack()
	{
		if (Helper.GetColony(Colony).NaturalEnemySpawningPool.SpawnKeys[SpawnObjType.NaturalEnemy].Count == 0)
			return;

		string uid = Helper.GetColony(Colony).NaturalEnemySpawningPool.SpawnKeys[SpawnObjType.NaturalEnemy][0];
		AICommand cmd = new AICommand(AICommandType.GEN_NATURAL_ENEMY, uid);
		Helper.GetColony(Colony).AICommandQueue(SpawnObjType.NaturalEnemy).PushCommand(cmd);
	}


}
