using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{
    public float force = 5f;
    public float torque = 20f;
    private Rigidbody rb;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private float timeOfActivation;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
        }
    }
    void Swipe()
    {
        timeOfActivation = Time.time;
        rb.isKinematic = false;
        Vector2 swipe = endPoint - startPoint;
        rb.AddForce(swipe * force, ForceMode.Impulse);
        rb.AddTorque(0f, 0f, -Mathf.Sign(swipe.x) * torque, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        float timeInAir = Time.time - timeOfActivation;
        if (timeInAir < .05f) return;
        if (other.tag == "Table")
        {
            rb.isKinematic = true;
        }
        else
        {
            Restart();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        float timeInAir = Time.time - timeOfActivation;
        if (!rb.isKinematic && timeInAir > .1f)
        {
            Restart();
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
