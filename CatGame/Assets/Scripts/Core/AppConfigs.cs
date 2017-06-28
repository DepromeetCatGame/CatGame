using System.Collections;
using UnityEngine;

namespace Core
{
	[Singleton(CreateInstance = true, DontDestroyOnLoad = true, PrefabPath = "AppConfigs")]
	public class AppConfigs : SingletonBehaviour<AppConfigs>
	{
		[SerializeField]
		private int resoultionWidth = 1080;

		[SerializeField]
		private int resoultionHeight = 1920;

		[SerializeField]
		private int frameRate = 60;

		public int CalculatedScreenWidth { get; private set; }
		public int CalculatedScreenHeigth { get; private set; }

		public Coroutine Configure()
		{
			SetFrameRate();

			return StartCoroutine(DoSetResolution(resoultionWidth, resoultionHeight));
		}

		private void SetFrameRate()
		{
			Application.targetFrameRate = frameRate;
		}

		private IEnumerator DoSetResolution(int width, int height)
		{
			UIManager.Instance.Camera.enabled = false;

			SetResolution(width, height);

			//해상도 조절시 화면 깜빡이는 시간 대기
			yield return new WaitForSeconds(0.1f);

			UIManager.Instance.Camera.enabled = true;
		}

		private void SetResolution(int width, int height)
		{
			var screenWidth = Screen.width;
			var screenHeight = Screen.height;

			var scale = 1.0f;

			//비율 맞춤
			screenWidth = screenHeight * width / height;
			//크기 맞춤
			scale = Mathf.Max(1.0f, (float)screenHeight / height);

			CalculatedScreenWidth = (int)(screenWidth * scale);
			CalculatedScreenHeigth = (int)(screenHeight * scale);
		}
	}
}