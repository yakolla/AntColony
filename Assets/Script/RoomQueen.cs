using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomQueen : Room {

	[SerializeField]
	float	m_eggPerSec = 1f;

	float	m_elapsedTime = 0;

	override public void StartBuilding()
	{
		base.StartBuilding();

		Ant ant = Helper.GetAntSpawningPool().Spawn((int)Helper.SpawnObjType.QueenAnt);
		Helper.GetAntSpawningPool().StartBuilding(ant);

		AICommand cmd = new AICommand(AICommandType.ROOM, true);
		cmd.Target = this;
		cmd.Priority = 1;
		Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.QueenAnt).PushCommand(cmd);

	}

	void Update()
	{
		if (m_doneBuilding == false)
			return;

		m_elapsedTime += Time.deltaTime;
		while (m_elapsedTime >= 1f)
		{
			for( int i = 0; i < m_eggPerSec; ++i)
			{

			}

			m_elapsedTime = m_elapsedTime-1f;
		}


	}
}
