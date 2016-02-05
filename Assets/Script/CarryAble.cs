using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarryAble 
{
	protected Texture2D m_tex;
	protected CarryHolder m_holder;
	protected int	m_colony;

	virtual public  void Start(Texture2D tex)
	{
		m_tex = tex;
	}

	virtual public void Update()
	{

	}
	
	virtual public  void OnKill()
	{
		if (m_holder != null)
		{
			m_holder.Takeout();			
			m_holder = null;
		}

	}
	
	virtual public  void SetCarryHolder(CarryHolder holder)
	{
		m_holder = holder;
		if (holder != null)
			m_colony = holder.Onwer.Colony;
	}

	public Texture2D Img
	{
		get { return m_tex; }
	}
}
