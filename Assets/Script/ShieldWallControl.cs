using UnityEngine;
using System.Collections;

public class ShieldWallControl : MonoBehaviour {
	
	public GameObject shieldWallGui;
	public GameObject player;
	public float distance;
	public bool inRange = false;

	
	void FixedUpdate(){
	 	distance = Vector3.Distance(transform.position,player.transform.position);
		if(distance<40){
			inRange=true;
			shieldWallGui.SetActive(true);	
		}
		else
		{
			if(inRange)
				shieldWallGui.SetActive(false);
			inRange=false;
		}
	}
}
