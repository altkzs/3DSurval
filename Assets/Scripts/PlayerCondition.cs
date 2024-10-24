using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
	public UICondition uICondition;
	Condition health { get { return uICondition.health; } }
	Condition hunger { get { return uICondition.hunger; } }
	Condition stamina { get { return uICondition.stamina; } }

	public float noHungerHealthDecay;
	void Update()
	{
		hunger.Subtract(hunger.passiveValue * Time.deltaTime);
		stamina.Add(stamina.passiveValue * Time.deltaTime);

		if (hunger.curValue == 0f)
		{
			health.Subtract(noHungerHealthDecay * Time.deltaTime);
		}

		if (health.curValue == 0f)
		{
			Die();
		}
	}

	public void Heal(float amout)
	{
		health.Add(amout);
	}
	public void Eat(float amout)
	{
		hunger.Add(amout);
	}
	public void Die()
	{
		Debug.Log("죽었다!");
	}
}
