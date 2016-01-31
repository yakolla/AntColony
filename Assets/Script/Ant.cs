using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ant : SpawnBaseObj {

	protected AntNavigator	m_navigator;

	[SerializeField]
	int			m_team = 0;

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
		if (HP <= 0)
			Helper.GetAntSpawningPool().Kill(this);
		else if (m_hunger <= 0)
			Helper.GetAntSpawningPool().Kill(this);
	}

	protected Room SelectRandomRoom(bool digy)
	{
		int roomTypeCount = Helper.GetRoomSpawningPool().Types.Count;
		if (roomTypeCount == 0)
			return null;

		Helper.SpawnObjType randType = Helper.GetRoomSpawningPool().Types[Random.Range(0, roomTypeCount)];
		return SelectRandomRoom(randType, digy);
	
	}

	protected Room SelectRandomRoom(Helper.SpawnObjType type, bool digy)
	{
		if (false == Helper.GetRoomSpawningPool().Types.Contains(type))
			return null;

		int roomCount = Helper.GetRoomSpawningPool().SpawnKeys[type].Count;
		if (roomCount == 0)
			return null;

		string randKey = Helper.GetRoomSpawningPool().SpawnKeys[type][Random.Range(0, roomCount)];

		Room room = Helper.GetRoomSpawningPool().GetSpawnedObject(randKey).GetComponent<Room>();
		if (digy == false && room.HasPath == false)
			return null;

		return room;
	}

	virtual public void DoDefaultAI()
	{

		if (Random.Range(0, 2) == 0)			
		{
			m_navigator.GoTo(Helper.GetBackground().GetRandomGround(), false);
		}
		else
		{
			bool digy = true;
			SpawnBaseObj target = SelectRandomRoom(digy = m_navigator.Digy);
			m_navigator.GoTo(target, digy);
		}

	}

	public void ContinueNextAICommand()
	{
		bool digy = true;
		SpawnBaseObj target = null;

		if (m_hunger < 60)
		{
			Room room = SelectRandomRoom(Helper.SpawnObjType.FoodRoom, false);
			if (room != null)
				m_cmd = new AICommand(AICommandType.EAT_FOOD, room.UID );
			else
				m_cmd = null;
		}
		else
		{
			m_cmd = Helper.GetBackground().AICommandQueue(m_type).PopCommand();
		}

		if (m_cmd != null)
		{
			switch(m_cmd.CommandType)
			{
			case AICommandType.GEN_EGG:
				target = Helper.GetRoomSpawningPool().GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_FOOD:
				target = Helper.GetFoodSpawningPool().GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.EAT_FOOD:
				target = Helper.GetRoomSpawningPool().GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_NATURAL_ENEMY:
				target = Helper.GetNaturalEnemySpawningPool().GetSpawnedObject(m_cmd.UID);
				digy = false;
				break;
			case AICommandType.GEN_ROOM:
				target = Helper.GetRoomSpawningPool().GetSpawnedObject(m_cmd.UID);
				digy = true;
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
			Helper.SpawnObjType type = target.Type;
			
			switch(type)
			{
			case Helper.SpawnObjType.FoodRoom:
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

	public int Team
	{
		get {return m_team;}
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
		if (ant != null && ant.Team != Team)
		{
			Attack (ant);
		}
	}
}
