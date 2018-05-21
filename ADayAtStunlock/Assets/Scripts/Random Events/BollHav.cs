using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BollHav : MonoBehaviour
{
    private static BollHav myInstance;
    public static BollHav MyInstance
    {
        get { return myInstance; }
    }
    public GameObject bollPrefab;

    private void Awake()
    {
        if (myInstance == null)
            myInstance = this;
        else
            Destroy(this);
    }

    public void StartBollHav()
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                StartCoroutine(BollarSkapas(i, j));
    }

    private IEnumerator BollarSkapas(int i, int j)
    {
        yield return new WaitForSeconds(0.01f*(i+j));
        GameObject temp;
        temp = Instantiate(myInstance.bollPrefab, new Vector3(Mathf.Lerp(-19f, 19f, ((float)i / 50f)), Random.Range(75f, 150f), Mathf.Lerp(-15f, 19f, ((float)j / 50f))), Quaternion.identity);
        temp.AddComponent<Boll>();
    }

    public class Boll : MonoBehaviour
    {
        private void Start()
        {
            float randomsize = Random.Range(0.6f, 0.8f);
            transform.localScale = new Vector3(randomsize, randomsize, randomsize);
            GetComponent<Renderer>().material.color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), 1f);
            Invoke("DestroyMe", Random.Range(7f, 22f));
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
