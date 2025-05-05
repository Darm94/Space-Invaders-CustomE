using UnityEngine;
public class DestroyOnHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Self destruct on hit
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            GetComponentInParent<AudioSource>().Play();

            Debug.Log("Hit by player bullet", this);

            //Take away from hierarchy to avoid moved with the others by EnemiesMover script
            transform.parent.parent = null;

            GetComponentInParent<EnemyManager>().enabled = false;
            GetComponentInParent<MeshRenderer>().enabled = false;

            foreach (Transform t in transform) {
                Debug.Log(t.name);
                t.gameObject.SetActive(true);

                Rigidbody rb = t.gameObject.AddComponent<Rigidbody>();
                if (rb)
                    rb.AddExplosionForce(20, t.transform.position, 100, 0, ForceMode.Impulse);
            }
            
            transform.parent.gameObject.GetComponent<EnemyManager>().WasHit();
            Destroy(transform.parent.gameObject, 3);
            Destroy(this);
        }
    }
}