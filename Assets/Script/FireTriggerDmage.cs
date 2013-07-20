using UnityEngine;
using System.Collections;

public class FireTriggerDmage : MonoBehaviour {
	
	public GameObject player;
	public bool burning=false;
	public int fireDamageAmount;
	
	void Start(){
		player = GameObject.Find("Player");
		fireDamageAmount = 5;
	}

	void OnTriggerStay(Collider theCollider){
		if(!burning)
			if(theCollider.name=="Player"){
				burning=true;
				StartCoroutine("Burn");
			}
		
	}
	
	IEnumerator Burn(){		
		player.SendMessage("ChangeLife",-fireDamageAmount);
		yield return new WaitForSeconds(0.5f);
		burning=false;
	}
}
