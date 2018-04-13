using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("dashboard");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
