using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NaturalEnemy : Ant {

	override public void StartBuilding()
	{
		Point st = Point.ToPoint(transform.position);
		Helper.GetBackground().SetPixel(st.x, st.y, Helper.NATURAL_ENEMY_TILE);	
	}

	void OnTriggerEnter2D(Collider2D other) {
		Ant ant = other.gameObject.GetComponent<Ant>();
		if (ant != null)
		{
			--HP;
			Helper.GetAntSpawningPool().Kill(ant);

			if (0 == HP)
				Helper.GetNaturalEnemySpawningPool().Kill(this);
			
		}
	}
}
