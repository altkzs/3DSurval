using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public PlayerController Controller;
	public PlayerCondition condition;

	private void Awake()
	{
		CharacterManager.Instance.player = this;
		Controller = GetComponent<PlayerController>();
		condition = GetComponent<PlayerCondition>();
	}
}
