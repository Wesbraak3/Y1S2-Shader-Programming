using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilizer : MonoBehaviour
{
	public SetVector rotAxis;
	public float rotVectorScalar = 10;
	Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		// TODO:
		//  if transform.up is not pointing up (=world up),
		//  calculate the rotation axis that would fix this
		Vector3 axis = Vector3.zero;

		rotAxis.vector = axis * rotVectorScalar;

		// TODO:
		//  With the calculated rotation axis, apply a "rotational force"
		//  to the rigidbody, using AddTorque:
		// ..


		// TODO (optional):
		//  Similarly, make the object move up and down, when the current
		//  position is higher or lower than the target position.
		//  (use AddForce)
		// ..
    }
}
