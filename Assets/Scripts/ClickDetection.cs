using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickDetection : MonoBehaviour
{

	public UnityEvent _onClick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseDown(){
		Debug.Log("Teehee");
		_onClick.Invoke();
	}
}
