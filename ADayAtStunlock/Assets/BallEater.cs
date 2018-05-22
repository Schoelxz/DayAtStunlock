using UnityEngine;

public class BallEater : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
            Destroy(collision.gameObject);
    }
}
