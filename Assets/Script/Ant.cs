using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ant : SpawnBaseObj {

	protected AntNavigator	m_navigator;

	[SerializeField]
	int			m_hunger = 10;
	[SerializeField]
	int			m_maxHunger = 10;

	Animator	m_animator;


	AICommand m_cmd = null;

	protected CarryHolder	m_carryHolder = new CarryHolder();

	public void Awake()
	{
		m_navigator = GetComponent<AntNavigator>();
		m_hunger = m_maxHunger;
		m_carryHolder.Init(this, transform.Find("CarryHolder").GetComponent<SpriteRenderer>(), 1, 1, 0);
		HP = MaxHP;
		m_animator = GetComponent<Animator>();
		StartCoroutine(LoopHenger());

	}

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
		m_navigator.GoTo(transform.position, m_navigator.Digy);
	}

	IEnumerator LoopHenger()
	{
		while (m_hunger > 0)
		{
			yield return new WaitForSeconds(1f);
			--m_hunger;
		}
	}

	public void Update()
	{
		if (HP <= 0 || m_hunger <= 0)
			Helper.GetColony(Colony).AntSpawningPool.Kill(this);
	}


	virtual public void DoDefaultAI()
	{

		if (Random.Range(0, 2) == 0)			
		{
			m_navigator.GoTo(Helper.GetColony(Colony).GetRandomGround(), false);
		}
		else
		{
			SpawnBaseObj target = Helper.GetColony(Colony).SelectRandomRoom(m_navigator.Digy);
			m_navigator.GoTo(target, m_navigator.Digy);
		}

	}

	public void ContinueNextAICommand()
	{

		SpawnBaseObj target = null;

		if (m_hunger < 60)
		{
			Room room = Helper.GetColony(Colony).SelectRandomRoom(SpawnObjType.RoomFood, false);
			if (room != null)
				m_cmd = new AICommand(AICommandType.EAT_FOOD, room.UID );
			else
				m_cmd = null;
		}
		else
		{
			m_cmd = Helper.GetColony(Colony).AICommandQueue(Type).PopCommand();
		}

		if (m_cmd != null)
		{
			bool digy = m_navigator.Digy;
			switch(m_cmd.CommandType)
			{
			case AICommandType.GEN_EGG:
				target = Helper.GetColony(Colony).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_FOOD:
				target = Helper.GetColony(Colony).FoodSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.EAT_FOOD:
				target = Helper.GetColony(Colony).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_NATURAL_ENEMY:
				target = Helper.GetColony(Colony).NaturalEnemySpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_ROOM:
				target = Helper.GetColony(Colony).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = true;
				break;
			case AICommandType.ATTACK:
				m_navigator.AddWayPoints(Helper.GetColony(Colony).GetRandomGround());
				target = Helper.GetColony(1).RoomSpawningPool.GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			}

			m_navigator.GoTo(target, digy);
		}
		else
		{
			DoDefaultAI();
		}
		

	}

	override public void OnReachToGoal(SpawnBaseObj target)
	{
		if (target != null && target.GetComponent<Room>() != null)
			target.GetComponent<Room>().HasPath = true;

		if (target != null && m_cmd != null)
		{
			SpawnObjType type = target.Type;
			
			switch(type)
			{
			case SpawnObjType.RoomFood:
				RoomFood roomFood = target.GetComponent<RoomFood>();
				if (m_cmd.CommandType == AICommandType.EAT_FOOD)
				{
					if (roomFood.CarryHolder.CarryCount > 0)
					{
						roomFood.CarryHolder.Takeout();
						m_hunger = Mathf.Min(m_hunger+m_maxHunger, m_maxHunger);
					}
				}
				break;
			
			}

		}
	}

	public void Attack(Ant victim)
	{
		victim.HP--;
		m_navigator.Stop();
		m_animator.SetTrigger("Attack");
	}

	public void OnAttackAniFinish()
	{
		m_navigator.RestartGo();

	}

	void OnTriggerEnter2D(Collider2D other) {
		Ant ant = other.gameObject.GetComponent<Ant>();
		if (ant != null && ant.Colony != Colony)
		{
			Attack (ant);
		}
	}
}
