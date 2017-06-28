using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
	private static bool isApplicationQuitting = false;
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<T>();

			if (instance == null)
				instance = InstantiatePrefab();

			if (instance == null)
				instance = CreateInstance();

			return instance;
		}
	}

	private static T InstantiatePrefab()
	{
		var singletonAttribute = GetSingletonAttribute();
		if (singletonAttribute == null)
			return null;

		if (string.IsNullOrEmpty(singletonAttribute.PrefabPath))
			return null;

		T prefab = Resources.Load<T>(singletonAttribute.PrefabPath);
		if (prefab == null)
		{
			Debug.LogWarningFormat("{0} singleton prefab not found : {1}", typeof(T).Name, singletonAttribute.PrefabPath);
			return null;
		}

		if (isApplicationQuitting)
		{
			Debug.LogWarningFormat("{0} singleton prefab instantiating while quitting: {1}", typeof(T).Name, singletonAttribute.PrefabPath);
			return null;
		}

		return GameObject.Instantiate<T>(prefab);
	}

	private static T CreateInstance()
	{
		var singletonAttribute = GetSingletonAttribute();
		if (singletonAttribute == null)
			return null;

		if (!singletonAttribute.CreateInstance)
			return null;

		if (isApplicationQuitting)
		{
			Debug.LogWarningFormat("{0} singleton prefab creating while quitting", typeof(T).Name);
			return null;
		}

		return new GameObject(typeof(T).Name).AddComponent<T>();
	}

	private static SingletonAttribute GetSingletonAttribute()
	{
		var attributes = typeof(T).GetCustomAttributes(typeof(SingletonAttribute), true);

		if (attributes.Length > 0)
			return attributes[0] as SingletonAttribute;
		else
			return null;
	}

	protected virtual void Awake()
	{
		if (instance == null)
			instance = (T)this;

		if (this != instance)
		{
			Debug.LogWarningFormat("Destroying duplicated singleton instance {0}", this);
			Destroy(this);
		}
		else
		{
			var singletonAttribute = GetSingletonAttribute();
			if (singletonAttribute != null && singletonAttribute.DontDestroyOnLoad)
				GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
		if (this == instance)
			instance = null;
	}

	protected virtual void OnApplicationQuit()
	{
		isApplicationQuitting = true;
	}
}