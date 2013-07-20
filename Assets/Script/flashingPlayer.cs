using UnityEngine;
using System.Collections;

public class flashingPlayer : MonoBehaviour {

	public bool toggle = true;
	public bool hold = false;
	
	void Start() {
		gameObject.renderer.material.color = Color.white;	
		
	}
	
	void Update () {
		if(!hold){
			hold = true;
			StartCoroutine(ChangeColor());
			
		}
			
	}
	
	IEnumerator ChangeColor(){
		if(toggle)
			yield return new WaitForSeconds(.2f);
		else
			yield return new WaitForSeconds(.8f);
		if(toggle){
			gameObject.renderer.material.color = Color.green;
			toggle = false;
			}
			else{
				gameObject.renderer.material.color = Color.white;	
				toggle = true;
			}
		hold = false;
		
	}
}
