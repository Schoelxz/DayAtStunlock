using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BollHav : MonoBehaviour
{
    private float ballX = 20;
    private float ballY = 20;

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
        for (int i = 0; i < ballX; i++)
            for (int j = 0; j < ballY; j++)
                StartCoroutine(BollarSkapas(i, j));
    }

    private IEnumerator BollarSkapas(int i, int j)
    {
        yield return new WaitForSeconds(0.01f*(i+j));
        GameObject temp;
        ///Balls all over the player
        temp = Instantiate(myInstance.bollPrefab, DAS.PlayerMovement.s_myInstance.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(15, 25), Random.Range(-2, 2)), Quaternion.identity);
        ///Make balls fall fast down.
        temp.GetComponent<Rigidbody>().velocity = new Vector3(0, -35.86f, 0);
        ///Balls all over the room
        //temp = Instantiate(myInstance.bollPrefab, new Vector3(Mathf.Lerp(-19f, 19f, ((float)i / ballX)) + Random.Range(-0.1f, 0.1f), Random.Range(75f, 150f), Mathf.Lerp(-15f, 19f, ((float)j / ballY) + Random.Range(-0.1f, 0.1f))), Quaternion.identity);
        temp.AddComponent<Boll>();
    }

    public class Boll : MonoBehaviour
    {
        private float deathTime;

        private void Start()
        {
            float randomsize = Random.Range(0.6f, 0.8f);
            transform.localScale = new Vector3(randomsize, randomsize, randomsize);
            GetComponent<Renderer>().material.color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), 1f);
            deathTime = Random.Range(7f, 22f);
            StartCoroutine(DestroyMe(deathTime));
        }

        private IEnumerator DestroyMe(float deathTime)
        {
            yield return new WaitForSeconds(deathTime-1);
            Vector3 orgSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            float time = 1;
            do
            {
                yield return new WaitForEndOfFrame();
                time -= Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, orgSize, time);
            } while (time > 0);
            

            Destroy(gameObject);
        }
    }
}
