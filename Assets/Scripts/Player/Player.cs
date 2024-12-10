using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public PlayerController Controller;
	public PlayerCondition condition;

	public ItemData itemData;
	public Action addItem;
	public Transform dropPosition;

	private void Awake()
	{
		CharacterManager.Instance.player = this;
		Controller = GetComponent<PlayerController>();
		condition = GetComponent<PlayerCondition>();
	}
}
