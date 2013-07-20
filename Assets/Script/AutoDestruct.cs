using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour {

	public float destructTime=10f;
	IEnumerator Start () {
		yield return new WaitForSeconds(destructTime);
		Destroy(gameObject);
	}

}
