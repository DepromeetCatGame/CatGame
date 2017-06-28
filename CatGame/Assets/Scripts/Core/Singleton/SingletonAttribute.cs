public class SingletonAttribute : System.Attribute
{
	private string prefabPath;
	private bool createInstance;
	private bool dontDestroyOnLoad;

	public string PrefabPath
	{
		get { return prefabPath; }
		set { prefabPath = value; }
	}

	public bool CreateInstance
	{
		get { return createInstance; }
		set { createInstance = value; }
	}

	public bool DontDestroyOnLoad
	{
		get { return dontDestroyOnLoad; }
		set { dontDestroyOnLoad = value; }
	}
}