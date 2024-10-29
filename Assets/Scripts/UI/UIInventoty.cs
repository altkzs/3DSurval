using TMPro;
using UnityEngine;

public class UIInventoty : MonoBehaviour
{
	public ItemSlot[] slots;
	public GameObject inventotyWindow;
	public Transform slotPanel;

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


	void Start()
	{
		controller = CharacterManager.Instance.player.Controller;
		condition = CharacterManager.Instance.player.condition;

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

	}


	ItemSlot GetItemStack(ItemData data)
	{
		return null;
	}
	ItemSlot GetEmptySlot()
	{
		return null;
	}
	void ThrowItem(ItemData data)
	{

	}
	public void Set()
	{

	}
	public void Clear()
	{

	}
}
