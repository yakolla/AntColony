using UnityEngine;
using System.Collections;

public class NaturalEnemySpawningPool : SpawningPool<NaturalEnemy> {

	override public void OnClickSpawn(int index)
	{
		NaturalEnemy spawned = Spawn(index);
		StartBuilding(spawned);
	}

	public static void PushCommand(NaturalEnemy spawned)
	{
		AICommand cmd = new AICommand(AICommandType.NATURAL_ENEMY, false);
		cmd.Priority = 1;
		cmd.Target = spawned;
		Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.NaturalEnemy).PushCommand(cmd);
	}
}
