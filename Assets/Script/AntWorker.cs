using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntWorker : Ant {

	override public void OnReachToGoal(SpawnBaseObj target)
	{		
		base.OnReachToGoal(target);

		if (target != null)
		{
			SpawnObjType type = target.Type;


			switch(type)
			{
			case SpawnObjType.RoomFood:
				RoomFood roomFood = target.GetComponent<RoomFood>();
				if (m_carryHolder.CarryCount > 0)
					roomFood.CarryHolder.PutOn(m_carryHolder.Takeout());
				target = null;
				break;
			case SpawnObjType.RoomEgg:
				RoomEgg roomEgg = target.GetComponent<RoomEgg>();
				if (m_carryHolder.CarryCount > 0)
					roomEgg.CarryHolder.PutOn(m_carryHolder.Takeout());
				target = null;
				break;
			case SpawnObjType.Food:
				Food food = target.GetComponent<Food>();
				target = Helper.GetColony(Colony).SelectRandomRoom(SpawnObjType.RoomFood, false);
				if (target != null)
				{
					FoodPeace foodPeace = (FoodPeace)food.Slice();
					m_carryHolder.PutOn(foodPeace);
					StartCoroutine(LoopForBuffSpeed(AICommandType.EAT_FOOD.ToString(), foodPeace.Buff));
				}
				break;
			case SpawnObjType.RoomQueen:
				RoomQueen roomQueen = target.GetComponent<RoomQueen>();
				if (roomQueen.CarryHolder.CarryCount > 0)
				{
					target = Helper.GetColony(Colony).SelectRandomRoom(SpawnObjType.RoomEgg, false);
					if (target != null)
						m_carryHolder.PutOn(roomQueen.CarryHolder.Takeout());

				}
				else
				{
					target = null;
				}
				break;
			default:
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
		
		Work ();
	}
}
