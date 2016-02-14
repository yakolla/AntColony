using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntSpawningPool : SpawningPool<Ant> {




	override public void OnClickSpawn(Ant obj)
	{
		StartBuilding(Spawn((int)obj.Type), Helper.GetColony(Colony).GetRandomGround());
	}

	public void OnClickAttack()
	{
		string uid = Helper.GetColony(1).RoomSpawningPool.SpawnKeys[Helper.GetColony(1).RoomSpawningPool.RandomSpawnType(SpawnObjType.RoomQueen)][0];
		AICommand cmd = new AICommand(AICommandType.ATTACK, uid);
		Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntSoldier).PushCommand(cmd);
	}


}
