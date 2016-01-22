using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AntWorker : Ant {

	CarryHolder	m_carryHolder = new CarryHolder();

	// Use this for initialization
	void Start () {
		m_carryHolder.Init(transform.Find("CarryHolder").GetComponent<SpriteRenderer>());
	}


	
	// Update is called once per frame
	void Update () 
	{
	}

	override public void OnKill ()
	{

	}

	public void PutSomething(Texture2D tex)
	{
		m_carryHolder.PutSomething(tex);
	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		if (target != null)
		{
			switch(target.Type)
			{
			case Helper.SpawnObjType.FoodRoom:
				RoomFood roomFood = target.GetComponent<RoomFood>();
				if (m_carryHolder.IsExists() == true)
					roomFood.PutSomething(m_carryHolder.Takeout());
				break;
			case Helper.SpawnObjType.Food:
				Food food = target.GetComponent<Food>();
				PutSomething(food.Slice());
				break;
			}
		}

		base.OnReachToGoal(target);
	}
}
