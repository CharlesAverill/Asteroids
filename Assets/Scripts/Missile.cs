using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    float speed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float life = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0);

        cooldown += Time.deltaTime;
        life -= Time.deltaTime;

        if (life <= 0) {
            Destroy(gameObject);
        }
    }

    float cooldown = 0f;
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 6) {
            if (cooldown < 0.1f) {
                return;
            }
            cooldown = 0f;

            transform.Rotate(0, Random.Range(170, 190), 0);
        }
    }
}
