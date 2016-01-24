using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntQueen : Ant {

	[SerializeField]
	float		m_genEggTime = 1f;

	bool		m_eggable = false;

	float		m_elapsedTime = 0f;

	RoomQueen	m_roomQueen = null;

	[SerializeField]
	Sprite 		m_eggSprite;


	// Update is called once per frame
	void Update () 
	{
		if (m_roomQueen == null)
			return;

		m_elapsedTime += Time.deltaTime;

		if (m_elapsedTime > m_genEggTime)
		{
			m_elapsedTime -= m_genEggTime;

			EggPeace egg = new EggPeace();
			egg.Start(m_eggSprite.texture);

			AICommand cmd = new AICommand(AICommandType.GEN_EGG, m_roomQueen.UID);
			Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.WorkerAnt).PushCommand(cmd);

			m_roomQueen.CarryHolder.PutOn(egg);
		}
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		base.OnReachToGoal(target);

		if (target != null && target.Type == Helper.SpawnObjType.QueenRoom)
		{
			m_eggable = true;
			m_roomQueen = target as RoomQueen;
		}

		if (m_eggable == false && target == null)
			ContinueNextAICommand();
	}

	override public void OnKill ()
	{

	}

}
