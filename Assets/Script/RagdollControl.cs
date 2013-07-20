using UnityEngine;
using System.Collections;

public class RagdollControl : MonoBehaviour {
	
	public float backwardForce = 100f;
	public float upwardForce = 100f;
	// Use this for initialization
	void Start () {
		rigidbody.AddForce(-Vector3.forward*backwardForce+Vector3.up*upwardForce);
	}
	

}
