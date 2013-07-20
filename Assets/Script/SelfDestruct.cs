using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	public float destructTime=2f;
	
	void Start () {
		StartCoroutine("startDestroy");
	}
	
	IEnumerator startDestroy(){
		yield return new WaitForSeconds(destructTime);
		Destroy(gameObject);
	}
}
