using TMPro;
using UnityEngine;

public class UIInventoty : MonoBehaviour
{
	public ItemSlot[] slots;
	public GameObject inventotyWindow;
	public Transform slotPanel;
	public Transform dropPosition;

	[Header("Select Item")]
	public TextMeshProUGUI selectedItemName;
	public TextMeshProUGUI selectedItemDescription;
	public TextMeshProUGUI selectedStatName;
	public TextMeshProUGUI selectedStatValue;

	public GameObject useButton;
	public GameObject equipButton;
	public GameObject unEquipButton;
	public GameObject dropButton;

	private PlayerController controller;
	private PlayerCondition condition;

	ItemData selectedItem;
	int selectedItemIndex = 0;

	void Start()
	{
		controller = CharacterManager.Instance.player.Controller;
		condition = CharacterManager.Instance.player.condition;
		dropPosition = CharacterManager.Instance.player.dropPosition;

		controller.inventory += Toggle;
		CharacterManager.Instance.player.addItem += AddItem;

		inventotyWindow.SetActive(false);
		slots = new ItemSlot[slotPanel.childCount];

		for (int i = 0; i < slots.Length; i++)
		{
			slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
			slots[i].index = i;
			slots[i].inventoty = this;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
	void ClearSelectedItemWindow()
	{
		selectedItemName.text = string.Empty;
		selectedItemDescription.text = string.Empty;
		selectedStatName.text = string.Empty;
		selectedStatValue.text = string.Empty;

		useButton.SetActive(false);
		equipButton.SetActive(false);
		unEquipButton.SetActive(false);
		dropButton.SetActive(false);
	}

	public void Toggle()
	{
		if (IsOpen())
		{
			inventotyWindow.SetActive(false);
		}
		else
		{
			inventotyWindow.SetActive(true);
		}
	}
	public bool IsOpen()
	{
		return inventotyWindow.activeInHierarchy;
	}

	void AddItem()
	{
		ItemData data = CharacterManager.Instance.player.itemData;
		//아이템이 중복 가능한지 canStack
		if (data.canStack)
		{
			ItemSlot slot = GetItemStack(data);
			if (slot != null)
			{
				slot.quantity++;
				CharacterManager.Instance.player.itemData = null;
				return;
			}
		}

		//비어있는 슬롯 가져온다
		ItemSlot emptySlot = GetEmptySlot();

		//있다면
		if (emptySlot != null)
		{
			emptySlot.item = data;
			emptySlot.quantity = 1;
			UpdateUI();
			CharacterManager.Instance.player.itemData = null;
			return;
		}

		//없다면
		ThrowItem(data);

		CharacterManager.Instance.player.itemData = null;
	}
	void UpdateUI()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].item != null)
			{
				slots[i].Set();
			}
			else
			{
				slots[i].Clear();
			}
		}
	}

	ItemSlot GetItemStack(ItemData data)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
			{
				return slots[i];
			}
		}
		return null;
	}
	ItemSlot GetEmptySlot()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].item == null)
			{
				return slots[i];
			}
		}
		return null;
	}
	void ThrowItem(ItemData data)
	{
		Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
	}
	public void SelectItem(int index)
	{
		if (slots[index].item == null) return;
		selectedItem = slots[index].item;
		selectedItemIndex = index;

		selectedItemName.text = selectedItem.displayName;
		selectedItemDescription.text = selectedItem.description;
		selectedStatName.text = string.Empty;
		selectedStatValue.text = string.Empty;
		for (int i = 0; i < selectedItem.consumables.Length; i++)
		{
			selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
			selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
		}

		useButton.SetActive(selectedItem.type == ItemType.Consumable);
		equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
		unEquipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
		dropButton.SetActive(true);
	}
	public void OnUseButton()
	{
		if (selectedItem.type == ItemType.Consumable)
		{
			for (int i = 0; i < selectedItem.consumables.Length; i++)
			{
				switch (selectedItem.consumables[i].type)
				{
					case ConsumableType.Health:
						condition.Heal(selectedItem.consumables[i].value);
						break;
					case ConsumableType.Huger:
						condition.Eat(selectedItem.consumables[i].value);
						break;
				}
			}
			RemoveSelctedItem();
		}
	}
	public void OnDropButton()
	{
		ThrowItem(selectedItem);
		RemoveSelctedItem();
	}
	void RemoveSelctedItem()
	{
		slots[selectedItemIndex].quantity--;
		if (slots[selectedItemIndex].quantity <= 0)
		{
			selectedItem = null;
			slots[selectedItemIndex].item = null;
			selectedItemIndex = -1;
			ClearSelectedItemWindow();
		}
		UpdateUI();
	}
}
