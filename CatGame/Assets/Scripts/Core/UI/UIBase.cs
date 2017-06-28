using UnityEngine;

namespace Core
{
	//UI의 베이스가 되는 클래스
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class UIBase : MonoBehaviour
	{
		public Canvas Canvas { get; private set; }

		protected CanvasGroup canvasGroup;

		protected virtual void Awake()
		{
			Canvas = GetComponent<Canvas>();
			Canvas.renderMode = RenderMode.ScreenSpaceCamera;
			Canvas.worldCamera = UIManager.Instance.Camera;

			canvasGroup = GetComponent<CanvasGroup>();
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void OnDisable()
		{
		}

		public void SetVisible(bool visible)
		{
			canvasGroup.alpha = visible ? 1 : 0;
		}

		public void SetInteractable(bool interactable)
		{
			canvasGroup.interactable = interactable;
		}

		public void SetBlocksRaycats(bool blcoksRaycats)
		{
			canvasGroup.blocksRaycasts = blcoksRaycats;
		}
	}
}