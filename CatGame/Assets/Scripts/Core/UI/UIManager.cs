using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
	[Singleton(CreateInstance = true, DontDestroyOnLoad = true, PrefabPath = "UIManager")]
	public class UIManager : SingletonBehaviour<UIManager>
	{
		public EventSystem EventSystem { get; private set; }
		public Camera Camera { get; private set; }

		//모달의 시작 Order
		private const int kOrderOfModal = 10;
		//모달간의 Spacing
		private const int kSpacingOfModal = 10;

		private const string kUIPrefabPath = "UI/Prefab/";

		private List<UIModal> modals = new List<UIModal>();

		protected override void Awake()
		{
			base.Awake();

			EventSystem = GetComponentInChildren<EventSystem>();
			Camera = GetComponentInChildren<Camera>();
		}

		public void Init()
		{
		}

		public void PushModal(UIModal modal)
		{
			modals.Add(modal);

			UpdateModalOrders();
		}

		public void PopModal(UIModal modal)
		{
			modals.Remove(modal);

			UpdateModalOrders();
		}

		private void UpdateModalOrders()
		{
			for (int i = 0; i < modals.Count; i++)
			{
				var modal = modals[i];
				modal.Canvas.sortingOrder = kOrderOfModal + (kSpacingOfModal * i);
			}
		}

		public void BlockEvent()
		{
			EventSystem.enabled = false;
		}

		public void UnblockEvent()
		{
			EventSystem.enabled = true;
		}

		//Type, PrepabPath
		private static Dictionary<Type, string> uiMap = new Dictionary<Type, string>();

		public T Open<T>() where T : UIBase
		{
			string path;
			if (uiMap.TryGetValue(typeof(T), out path) == false)
			{
				Debug.LogError("UI를 찾을 수 없습니다." + typeof(T).Name);
				return null;
			}

			var prefab = Resources.Load<T>(path);
			var ui = Instantiate<T>(prefab);

			return ui;
		}
	}
}