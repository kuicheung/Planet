using UnityEngine;
using System.Collections;

public class SpinX : MonoBehaviour {
	
	public float spinSpeed = 5f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.left*spinSpeed*Time.deltaTime);
	}
}
