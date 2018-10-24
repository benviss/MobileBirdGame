using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roam : IState
{
  private GameObject ownerGameObject;
  private float movementSpeed;

  private Vector2 targetDestination;
  private float maxX;
  private float minX;

  public Roam(GameObject ownerGameObject, float movementSpeed, float maxX, float minX)
  {
    this.ownerGameObject = ownerGameObject;
    this.movementSpeed = movementSpeed;
    this.maxX = maxX;
    this.minX = minX;
  }

  public void Enter()
  {
    targetDestination = NewTargetDestination(maxX, minX);
  }

  public void Execute()
  {
    if (Vector2.Distance(targetDestination, ownerGameObject.transform.position) <= .1) {
      targetDestination = NewTargetDestination(maxX, minX);
    }

    float step = movementSpeed * Time.deltaTime;

    ownerGameObject.transform.position = Vector3.MoveTowards(ownerGameObject.transform.position, targetDestination, step);
  }

  public void Exit()
  {
    
  }

  public Vector2 NewTargetDestination(float _maxX, float _minX)
  {
    return new Vector2(Random.Range(_minX, _maxX),0);
  }
}
