using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarControls : MonoBehaviour
{
	public float steeringDegreesPerSecond;
	public float accelerationForce;
	public bool grip = true;
	public TextMeshProUGUI text;
	Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = Vector3.down * 0.5f; // stabilize!
    }

    // Update is called once per frame
    void Update()
    {
		transform.Rotate(0, steeringDegreesPerSecond * Input.GetAxis("Horizontal") * Time.deltaTime, 0, Space.Self);
    }

	void FixedUpdate() {
		rb.AddForce(transform.forward * accelerationForce * Input.GetAxis("Vertical"));
		// TODO: Calculate sideways and up speed
		//  show on canvas
		//  when not drifting, cancel the sideways speed
	}
}
