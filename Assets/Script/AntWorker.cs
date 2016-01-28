using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntWorker : Ant {

	CarryHolder	m_carryHolder = new CarryHolder();

	// Use this for initialization
	public void Awake () {
		base.Awake();
		m_carryHolder.Init(this, transform.Find("CarryHolder").GetComponent<SpriteRenderer>(), 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update();
	}

	override public void OnKill ()
	{

	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{		
		base.OnReachToGoal(target);

		if (target != null)
		{
			Helper.SpawnObjType type = target.Type;


			switch(type)
			{
			case Helper.SpawnObjType.FoodRoom:
				RoomFood roomFood = target.GetComponent<RoomFood>();
				if (m_carryHolder.CarryCount > 0)
					roomFood.CarryHolder.PutOn(m_carryHolder.Takeout());
				target = null;
				break;
			case Helper.SpawnObjType.EggRoom:
				RoomEgg roomEgg = target.GetComponent<RoomEgg>();
				if (m_carryHolder.CarryCount > 0)
					roomEgg.CarryHolder.PutOn(m_carryHolder.Takeout());
				target = null;
				break;
			case Helper.SpawnObjType.Food:
				Food food = target.GetComponent<Food>();
				target = SelectRandomRoom(Helper.SpawnObjType.FoodRoom, false);
				if (target != null)
					m_carryHolder.PutOn(food.Slice());
				break;
			case Helper.SpawnObjType.QueenRoom:
				RoomQueen roomQueen = target.GetComponent<RoomQueen>();
				if (roomQueen.CarryHolder.CarryCount > 0)
				{
					target = SelectRandomRoom(Helper.SpawnObjType.EggRoom, false);
					if (target != null)
						m_carryHolder.PutOn(roomQueen.CarryHolder.Takeout());

				}
				else
				{
					target = null;
				}
				break;
			}
		}
		
		if (target == null)
			ContinueNextAICommand();
		else
			m_navigator.GoTo(target, false);
	}
}
