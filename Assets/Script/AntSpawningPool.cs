using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntSpawningPool : SpawningPool<Ant> {




	override public void OnClickSpawn(string prefName)
	{
		StartBuilding(Spawn(prefName), Helper.GetColony(Colony).GetRandomGround());
	}

	public void OnClickAttack()
	{
		string uid = Helper.GetColony(1).RoomSpawningPool.UIDs(Helper.GetColony(1).RoomSpawningPool.RandomSpawnType(SpawnObjType.RoomQueen))[0];
		AICommand cmd = new AICommand(AICommandType.ATTACK, uid);
		Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntWorker).PushCommand(cmd);
		Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntSoldier).PushCommand(cmd);
	}


}
