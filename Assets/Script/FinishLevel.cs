using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour {

	public float health=1;
	public GameObject shatter;
	public float explsionRadius=200f;
	public GameObject finishGui;
	public bool started=false;
	public GameObject guiBar;
	public GameObject monster;
	public GameObject shockWave;
	public bool creatingMonster=false;
	
	void FixedUpdate(){
		if(!creatingMonster&&started){
			StartCoroutine("CreateMonster");			
		}
	}
	
	IEnumerator CreateMonster(){
		creatingMonster=true;
		GameObject mon = (GameObject)Instantiate(monster,transform.position+
					transform.forward*16-Vector3.up*17,transform.rotation);
		mon.transform.LookAt(GameObject.Find("Player").transform.position);
		yield return new WaitForSeconds(5f);
		creatingMonster=false;
	}
	
	void ChangeLife(int amountToChange){
		if(started){
			health += amountToChange;
			guiBar.SendMessage("SetBossLife",health/500f);
			if(health<=0){				
				StartCoroutine("DestroySequence");		
				started=false;
			}
		}
	}
	
	public void StartReactor(){
		started=true;	
		guiBar.SendMessage("SetBossLife",health/500f);
		GameObject mon = (GameObject)Instantiate(monster,transform.position+
					transform.forward*16-Vector3.up*17,transform.rotation);
		mon.transform.LookAt(GameObject.Find("Player").transform.position);
	}
	
	IEnumerator DestroySequence(){
		Instantiate(shatter,transform.position,transform.rotation);	
		yield return new WaitForSeconds(2f);
		Instantiate(shatter,transform.position+transform.right*5,transform.rotation);	
		yield return new WaitForSeconds(2f);
		Instantiate(shatter,transform.position-transform.right*5,transform.rotation);	
		yield return new WaitForSeconds(2f);
		Instantiate(shatter,transform.position,transform.rotation);	
		Collider[] explosionHits = Physics.OverlapSphere(transform.position+Vector3.up,explsionRadius);
		foreach(Collider hit in explosionHits){
					//Debug.Log(hit.collider.name);
			if(hit.collider.tag=="Enemy")
			hit.collider.SendMessage("ChangeLife",-100);
		}
		finishGui.SetActive(true);
		Destroy(gameObject);
	}
}
