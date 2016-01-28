using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntQueen : Ant {

	[SerializeField]
	float		m_genEggTime = 1f;

	bool		m_eggable = false;

	float		m_eggElapsedTime = 0f;

	RoomQueen	m_roomQueen = null;

	[SerializeField]
	Sprite 		m_eggSprite;


	// Update is called once per frame
	void Update () 
	{
		base.Update();

		if (m_roomQueen == null)
			return;

		m_eggElapsedTime += Time.deltaTime;

		if (m_eggElapsedTime > m_genEggTime)
		{
			m_eggElapsedTime -= m_genEggTime;

			EggPeace egg = new EggPeace();
			egg.Start(m_eggSprite.texture);

			AICommand cmd = new AICommand(AICommandType.GEN_EGG, m_roomQueen.UID);
			Helper.GetBackground().AICommandQueue(Helper.SpawnObjType.WorkerAnt).PushCommand(cmd);

			m_roomQueen.CarryHolder.PutOn(egg);
		}
	}

	override public void DoDefaultAI()
	{
		bool gotoRoomQueen = true;
		if (m_roomQueen != null)
		{
			if (0 == Vector3.Distance(m_roomQueen.transform.position, transform.position))
			{
				gotoRoomQueen = false;
			}
		}

		if (gotoRoomQueen == true)
		{
			SpawnBaseObj target = SelectRandomRoom( Helper.SpawnObjType.QueenRoom, false );
			m_navigator.GoTo(target, false);
		}
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		base.OnReachToGoal(target);

		if (target != null)
		{
			Helper.SpawnObjType type = target.Type;
			
			switch(type)
			{
			
			case Helper.SpawnObjType.QueenRoom:
				m_roomQueen = target as RoomQueen;
				break;
			}
		}
		
		ContinueNextAICommand();

	}

	override public void OnKill ()
	{

	}

}
