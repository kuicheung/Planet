using UnityEngine;
using System.Collections;

public class SpinY : MonoBehaviour {
	
	public float spinSpeed = 5f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up*spinSpeed*Time.deltaTime);
	}
}
