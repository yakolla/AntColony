using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Food : SpawnBaseObj {

	ModifiedTexture2D	m_modifiedTexture = new ModifiedTexture2D();

	void Awake()
	{

		SpriteRenderer renderer = transform.Find("Body").GetComponent<SpriteRenderer>();
		renderer.sprite = Sprite.Create(m_modifiedTexture.Init(renderer.sprite.texture), renderer.sprite.rect, new Vector2(0.5f, 0.5f));
		MaxHP = (m_modifiedTexture.Width*m_modifiedTexture.Height)/(Helper.ONE_PEACE_SIZE*Helper.ONE_PEACE_SIZE);

		HP = MaxHP;

	}

	void Update()
	{
		m_modifiedTexture.Update();
	}

	IEnumerator PeriodicSignal()
	{
		while(HP > 0)
		{
			AICommand cmd = new AICommand(AICommandType.GEN_FOOD, UID);
			Helper.GetColony(Colony).AICommandQueue(SpawnObjType.AntWorker).PushCommand(cmd);
			yield return new WaitForSeconds(0.5f);
		}
	}

	override public void StartBuilding()
	{
		StartCoroutine(PeriodicSignal());
	}

	public Peace	Slice()
	{
		Texture2D tex = null;
		int index = (MaxHP-HP);
		--HP;

		tex = m_modifiedTexture.SliceByIndex(index, m_modifiedTexture.Width/Helper.ONE_PEACE_SIZE, Helper.ONE_PEACE_SIZE, Helper.ONE_PEACE_SIZE);
		FoodPeace peace = new FoodPeace();
		peace.Start(tex);
		if (0 == HP)
			Helper.GetColony(Colony).FoodSpawningPool.Kill(this);

		return peace;
	}
}
