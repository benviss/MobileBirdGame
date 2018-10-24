using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  public float movementSpeed;
  private StateMachine stateMachine = new StateMachine();
	// Use this for initialization
	void Start () {
    this.stateMachine.ChangeState(new Roam(this.gameObject, movementSpeed, 10, -10));
	}
	
	// Update is called once per frame
	void Update () {
    this.stateMachine.ExecuteStateUpdate();
	}
}
