using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
	public float checkRate = 0.05f;
	private float lastCheckTime;
	public float maxCheckDistance;
	public LayerMask layerMask;

	public GameObject curInteractGameObject;
	private IInteractable curInteractable;

	[SerializeField] private TextMeshProUGUI PromptText;
	private Camera camera;
	void Start()
	{
		camera = Camera.main;
	}

	void Update()
	{
		if (Time.time - lastCheckTime > checkRate)
		{
			lastCheckTime = Time.time;
			Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
			{
				if (hit.collider.gameObject != curInteractGameObject)
				{
					curInteractGameObject = hit.collider.gameObject;
					curInteractable = hit.collider.GetComponent<IInteractable>();
					SetPromptText();
				}
			}
			else
			{
				curInteractGameObject = null;
				curInteractable = null;
				PromptText.gameObject.SetActive(false);
			}
		}
	}



	private void SetPromptText()
	{
		PromptText.gameObject.SetActive(true);
		PromptText.text = curInteractable.GetInteractPrompt();
	}

	public void OnInteractInput(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && curInteractable != null)
		{
			curInteractable.OnInteract();
			curInteractGameObject = null;
			curInteractable = null;
			PromptText.gameObject.SetActive(false);
		}
	}
}
