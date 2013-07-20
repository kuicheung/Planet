using UnityEngine;
using System.Collections;

public class CreateTrees : MonoBehaviour {

	public GameObject trees;
	public GameObject tigers;
	public GameObject box;
	public float rand;
	public int quadTreeCount=100;
	public int tigerCount=10;
	public int boxCount=20;
	public float y=0;
	
	void Start () {
		float lgPrime = 1299827f;
		float x,z;
		Vector3 pos;
		Quaternion rotate=new Quaternion(0,0,0,0);
		
		//quad1 trees
		y=0;
		for(int i=0;i<quadTreeCount;i++){
			rand=Random.value;			
			x=(rand*lgPrime)%490+5;
			rand=Random.value;
			z=(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(trees,pos,rotate);
		}	
		y=1.977266f;
		for(int i=0;i<tigerCount;i++){
			rand=Random.value;			
			x=(rand*lgPrime)%490+5;
			rand=Random.value;
			z=(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(tigers,pos,rotate);
		}
		y=1.025026f;
		for(int i=0;i<boxCount;i++){
			rand=Random.value;			
			x=(rand*lgPrime)%490+5;
			rand=Random.value;
			z=(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(box,pos,rotate);
		}
		
		//quad2 trees
		y=0;
		for(int i=0;i<quadTreeCount;i++){
			rand=Random.value;			
			x=-(rand*lgPrime)%490+5;
			rand=Random.value;
			z=(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(trees,pos,rotate);
		}
		y=1.977266f;
		for(int i=0;i<tigerCount;i++){
			rand=Random.value;			
			x=-(rand*lgPrime)%490+5;
			rand=Random.value;
			z=(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(tigers,pos,rotate);
		}
		y=1.025026f;
		for(int i=0;i<boxCount;i++){
			rand=Random.value;			
			x=-(rand*lgPrime)%490+5;
			rand=Random.value;
			z=(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(box,pos,rotate);
		}
		
		//quad3 trees
		y=0;
		for(int i=0;i<quadTreeCount;i++){
			rand=Random.value;			
			x=(rand*lgPrime)%490+5;
			rand=Random.value;
			z=-(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(trees,pos,rotate);
		}
		y=1.977266f;
		for(int i=0;i<tigerCount;i++){
			rand=Random.value;			
			x=(rand*lgPrime)%490+5;
			rand=Random.value;
			z=-(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(tigers,pos,rotate);
		}
		y=1.025026f;
		for(int i=0;i<boxCount;i++){
			rand=Random.value;			
			x=(rand*lgPrime)%490+5;
			rand=Random.value;
			z=-(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(box,pos,rotate);
		}
		
		//quad4 trees
		y=0;
		for(int i=0;i<quadTreeCount;i++){
			rand=Random.value;			
			x=-(rand*lgPrime)%490+5;
			rand=Random.value;
			z=-(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(trees,pos,rotate);
		}
		y=1.977266f;
		for(int i=0;i<tigerCount;i++){
			rand=Random.value;			
			x=-(rand*lgPrime)%490+5;
			rand=Random.value;
			z=-(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(tigers,pos,rotate);
		}
		y=1.025026f;
		for(int i=0;i<boxCount;i++){
			rand=Random.value;			
			x=-(rand*lgPrime)%490+5;
			rand=Random.value;
			z=-(rand*lgPrime)%490+5;
			pos = new Vector3(x,y,z);
			Instantiate(box,pos,rotate);
		}
	}
}
