using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    ParticleSystem ps;

    public Vector3 velocity;

    public float size;
    public string name = "";

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
    }

    public bool startNow = false;
    bool done = false;
    // Update is called once per frame
    void Update()
    {
        if (!startNow) {
            return;
        }
        if (!done) {
            CanvasManager.Instance.NewLabel(this);
            GetComponent<Rigidbody>().velocity = velocity;
            done = true;
        }
        // velocity = new Vector3(velocity.x, 0, velocity.z);
        // transform.Translate(velocity * Time.deltaTime);
        // transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (Vector3.Distance(transform.position, player.transform.position) > 300f) {
            Destroy(gameObject);
        }
    }

    public bool isDying = false;
    IEnumerator DestroyStuff(Collision other) {
        isDying = true;
        CanvasManager.Instance.UpdateScore((int)size);

        ps.Play();
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<SphereCollider>());
        Destroy(other.gameObject);

        while(ps.isPlaying) {
            yield return null;
        }

        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Missile") {
            StartCoroutine(DestroyStuff(other));
        }
    }
}
