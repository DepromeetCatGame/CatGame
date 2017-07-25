using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Singleton(DontDestroyOnLoad = false, CreateInstance = false)]
public class MapController : SingletonBehaviour<MapController>
{
    [SerializeField]
    private Transform background1Parent;
    [SerializeField]
    private Transform background2Parent;
    [SerializeField]
    private Transform mount1Parent;
    [SerializeField]
    private Transform mount2Parent;
    [SerializeField]
    private Transform cloud1Parent;
    [SerializeField]
    private Transform cloud2Parent;
    [SerializeField]
    private Transform blockParent;

    [SerializeField]
    private Transform moonParent;

    [SerializeField]
    private GameObject background1Prefab;
    [SerializeField]
    private GameObject background2Prefab;
    [SerializeField]
    private GameObject mount1Prefab;
    [SerializeField]
    private GameObject mount2Prefab;
    [SerializeField]
    private GameObject moonPrefab;
    [SerializeField]
    private Cloud mapCloudPrefab;
    [SerializeField]
    private GameObject blockPrefab;

    private GameObject moon;
    private List<GameObject> background2 = new List<GameObject>();
    private List<GameObject> mount1 = new List<GameObject>();
    private List<GameObject> mount2 = new List<GameObject>();
    private List<GameObject> block = new List<GameObject>();

    [SerializeField]
    private float background2Speed;
    [SerializeField]
    private float mount1Speed;
    [SerializeField]
    private float mount2Speed;
    [SerializeField]
    private float blockSpeed;

    [SerializeField]
    private float moonHeight;

    private float background2Width;
    private float mount1Width;
    private float mount2Width;
    private float blockWidth;

    protected override void Awake()
    {
        CreateMap();
        StartMap();
    }

    public void CreateMap()
    {
        Instantiate(background1Prefab, background1Parent);
        moon = Instantiate(moonPrefab, moonParent);
        moon.transform.position = new Vector3(2000, moonHeight, 0);

        background2Width = ImageSizeGetter.GetWidth(background2Prefab);
        mount1Width = ImageSizeGetter.GetWidth(mount1Prefab);
        mount2Width = ImageSizeGetter.GetWidth(mount2Prefab);
        blockWidth = ImageSizeGetter.GetWidth(blockPrefab);

        //일단 대충
        for (int i = -2; i < 8; i++)
        {
            var background2Obj = Instantiate(background2Prefab, background2Parent);
            background2Obj.transform.localPosition = new Vector3(background2Width * i, 0, 0);
            background2.Add(background2Obj);

            var mount1Obj = Instantiate(mount1Prefab, mount1Parent);
            mount1Obj.transform.localPosition = new Vector3(mount1Width * i, 0, 0);
            mount1.Add(mount1Obj);

            var mount2Obj = Instantiate(mount2Prefab, mount2Parent);
            mount2Obj.transform.localPosition = new Vector3(mount2Width * i, 0, 0);
            mount2.Add(mount2Obj);

            var blockObj = Instantiate(blockPrefab, blockParent);
            blockObj.transform.localPosition = new Vector3(blockWidth * i, 0, 0);
            block.Add(blockObj);
        }
    }

    public void StartMap()
    {
        StartCoroutine(UpdateMap());
    }

    private void UpdateMapItems(List<GameObject> items, float speed, float width)
    {
        foreach (var item in items)
        {
            item.transform.localPosition += Vector3.left * speed;
            if (item.transform.localPosition.x < -4003 - width)
                item.transform.localPosition = new Vector3(items.Max(i => i.transform.localPosition.x) + (width / 2), item.transform.localPosition.y, item.transform.localPosition.z);
        }
    }

    private IEnumerator UpdateMap()
    {
        float cloud1Duration = Random.Range(1, 5);
        float cloud2Duration = Random.Range(1, 5);

        while (true)
        {
            UpdateMapItems(background2, background2Speed, background2Width);
            UpdateMapItems(mount1, mount1Speed, mount1Width);
            UpdateMapItems(mount2, mount2Speed, mount2Width);
            UpdateMapItems(block, blockSpeed, blockWidth);

            moon.transform.position += Vector3.left * 0.1f;

            var deltaTime = Time.deltaTime;
            cloud1Duration -= deltaTime;
            cloud2Duration -= deltaTime;

            if (cloud1Duration < 0)
            {
                var cloud1 = Instantiate(mapCloudPrefab, cloud1Parent);
                cloud1.transform.localPosition = new Vector3(5000, Random.Range(-1000, 100), 0);
                cloud1.StartCloud(Random.Range(20, 60));
                cloud1Duration = Random.Range(2, 6);
            }

            if (cloud2Duration < 0)
            {
                var cloud2 = Instantiate(mapCloudPrefab, cloud2Parent);
                cloud2.transform.localPosition = new Vector3(5000, Random.Range(-1000, 100), 0);
                cloud2.StartCloud(Random.Range(20, 60));
                cloud2Duration = Random.Range(2, 6);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}