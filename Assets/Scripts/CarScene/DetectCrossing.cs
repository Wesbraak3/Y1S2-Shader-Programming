using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectCrossing : MonoBehaviour
{
	public Transform target;
	public Transform[] towers;
	public TextMeshPro text;
	public Transform mirrorImage;
	public Transform mirror;
	public SetVector normalArrow;

	void ActivateAlarm() {
		foreach (Transform t in towers) {
			t.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
		}
	}
	void DeactivateAlarm() {
		foreach (Transform t in towers) {
			t.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (towers.Length > 0) {
			// TODO: calculate the normal for reflection

			Vector3 normal = Vector3.forward; // not correct

			if (towers.Length == 2) {
                // In 2D, just apply the 2D rotation formula for 90 degrees
                //  on the xz-plane:
                // normal = ... ;
				
				// vector to the point to measure  
                Vector3 dir = (towers[1].position - towers[0].position).normalized;
				// 90 degrees from vector
                normal = new Vector3(-dir.z, 0, dir.x);

            } else if (towers.Length>=3) {
                // In 3D, the cross product can be used:
                // normal = ... ;

                Vector3 a = towers[1].position - towers[0].position;
                Vector3 b = towers[2].position - towers[0].position;

				// dot product but then 3d
                // calculate .cross without unity .cross function
                normal = new(
					a.y * b.z - a.z * b.y,
					a.z * b.x - a.x * b.z,
					a.x * b.y - a.y * b.x
				);

                normal = normal.normalized;

                //normal = Vector3.Cross(a, b).normalized;
            }

			if (normalArrow!=null) {
				normalArrow.vector = normal;
			}


            // TODO:
            //  Use dot product to calculate the distance between this line / plane and
            //  the target:
            Vector3 planePoint = towers[0].position;
            float distance = Vector3.Dot(target.position - planePoint, normal);

			if (distance < 0) {
				ActivateAlarm();
			} else {
				DeactivateAlarm();
			}
			if (text!=null) {
				text.text = $"Distance:\n{distance:0.00}";
			}

			// TODO:
			//  Apply vector reflection to place a mirror image object of the target
			//  "behind" the mirror:
			if (mirrorImage!=null) {
                Vector3 mirrorPosition =
                    target.position - 2f * distance * normal;

                mirrorImage.position = mirrorPosition;

                // TODO (challenge):
                //  Also rotate the mirror image object according to the reflection!
            }

            // Visually place the semi-transparent mirror object:
            //  (If you calculate the normal correctly, nothing needs to be changed here)
            if (mirror!=null) {
				Vector3 avgPosition=Vector3.zero;
				foreach (Transform t in towers) {
					avgPosition += t.position;
				}
				avgPosition /= 3;
				mirror.position = avgPosition;
				mirror.forward = -normal; // why does this even work?
			}
		}
    }
}
