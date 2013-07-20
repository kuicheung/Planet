using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public float scrollSpeed = 10f;
	public float updateSpeed = 10f;
	public float playerDistance = 6f;
	public float cameraHeight = 4f;
	public GameObject player;
	
	public float px;
	public float py;
	public float pz;
	public float x;
	public float y;
	public float z;
	public float w;
	public float rotationYAmount;
	public float rotationXAmount;
	public float mouseTurnSpeed = 10f;
	public bool reset = false;

	void FixedUpdate () {
		px=player.transform.position.x;
		py=player.transform.position.y;
		pz=player.transform.position.z;
		x=transform.rotation.x;
		y=transform.rotation.y;
		z=transform.rotation.z;
		w=transform.rotation.w;
		
		
		camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel")*scrollSpeed*Time.deltaTime;
		camera.fieldOfView = Mathf.Clamp(camera.fieldOfView,1,50);
		
		transform.position = Vector3.Lerp (transform.position,player.transform.position+player.transform.right+
					Vector3.up*cameraHeight-player.transform.forward.normalized*playerDistance,
					Time.deltaTime * updateSpeed);
		
		if(Input.GetButton("Fire1")||Input.GetKey(KeyCode.LeftShift)){
			reset=false;
			rotationYAmount = -Input.GetAxis("Mouse Y")*mouseTurnSpeed*Time.deltaTime;
			transform.Rotate(new Vector3(rotationYAmount,0f,0f));
			rotationXAmount = Input.GetAxis("Mouse X")*mouseTurnSpeed*Time.deltaTime;
			player.transform.Rotate(new Vector3(0f,rotationXAmount,0f));			
		}
		else
			reset=true;
		
		if(reset){
			transform.rotation=new Quaternion(0f,0f,0f,0f);
			transform.Rotate(new Vector3(2f,0f,0f));
		}
	}

}
