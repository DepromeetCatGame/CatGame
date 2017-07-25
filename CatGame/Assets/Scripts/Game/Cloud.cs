using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;

    public void StartCloud(float speed)
    {
        this.speed = speed;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (this.transform.position.x > -5000)
        {
            transform.localPosition += Vector3.left * speed;
            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);
    }
}