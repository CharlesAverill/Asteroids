using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : MonoBehaviour
{
    public GameObject[] shipModels;
    public Color[] trailColors;
    public float speed = 5f;

    public GameObject missilePrefab;

    BoxCollider bc;

    ParticleSystem ps;

    Rigidbody rb;

    GameObject shipModel;

    Actions actions;

    public GameObject[] cameras;
    public GameObject[] lights;
    int camIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        actions = new Actions();
        actions.Player.Get().Enable();
        actions.Player.Move.started += ctx => isMoving = true;
        actions.Player.Move.performed += ctx => isMoving = true;
        actions.Player.Move.canceled += ctx => isMoving = false;
        actions.Player.ChangeCamera.started += ctx => ChangeCamera(ctx);

        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        ParticleSystem.MainModule settings = ps.main;

        settings.startColor = new ParticleSystem.MinMaxGradient(trailColors[Random.Range(0, trailColors.Length)]);

        shipModel = Instantiate(shipModels[Random.Range(0, shipModels.Length)]);
        shipModel.transform.parent = transform;
        shipModel.transform.position = transform.position;

        shipModel.gameObject.AddComponent<BoxCollider>();
        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = shipModel.gameObject.GetComponent<BoxCollider>().size;
        bc.size = new Vector3(bc.size.x, bc.size.z, bc.size.y);
        Destroy(shipModel.gameObject.GetComponent<BoxCollider>());

        // cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        cameras[0].SetActive(true);
        lights[0].SetActive(true);
        cameras[1].SetActive(false);
        lights[1].SetActive(false);
    }

    bool isMoving = false;

    float cooldown = 0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown += Time.deltaTime;
        if (isMoving) {
            Move(actions.Player.Move.ReadValue<Vector2>());
        } else {
            ps.Stop();
        }

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void OnDestroy() {
        cameras[1].transform.SetParent(null);
        cameras[1].GetComponent<Camera>().enabled = true;
    }

    public void Move(Vector2 m)
    {
        if (m.x != 0) {
            // transform.Rotate(m.x * Vector3.up * speed * Time.deltaTime);
            rb.angularVelocity += new Vector3(0, m.x * 2 * Time.deltaTime, 0);
        } else if (m.y > 0) {
            ps.Play();
            rb.AddForce(transform.forward * speed * Time.deltaTime);
        } else if (m.y < 0) {
            rb.angularVelocity *= 1 -  Time.deltaTime;
        }
    }

    public void MoveCallback(InputAction.CallbackContext context) {
        isMoving = true;
    }

    public void FireCallback(InputAction.CallbackContext context)
    {
        if (cooldown > 0.3f) {
            cooldown = 0;
        } else {
            return;
        }
        
        GameObject m = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        m.transform.Rotate(0, 180, 0);
        m.transform.forward = transform.forward;
    }

    public void ChangeCamera(InputAction.CallbackContext context) {
        camIndex = (camIndex + 1) % cameras.Length;

        cameras[0].SetActive(!cameras[0].active);
        lights[0].SetActive(!lights[0].active);
        cameras[1].SetActive(!cameras[1].active);
        lights[1].SetActive(!lights[1].active);
    }

}
