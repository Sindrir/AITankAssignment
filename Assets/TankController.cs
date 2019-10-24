using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _physicsBody;

    [SerializeField]
    private GraphNode _targetNode;

    [SerializeField]
    private GameObject _base;

    [SerializeField]
    private GameObject _turret;

    [Range(0f, 10f)]
    [SerializeField]
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        MoveForward();

        if (_targetNode != null)
        {
            PointTurretAtTarget();
            
        }
    }

    void PointTurretAtTarget()
    {
        var targetDistance = _targetNode.gameObject.transform.position - transform.position;
        var targetDirection = targetDistance;

        targetDistance.Normalize();

        _turret.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    }

    void MoveForward()
    {
        float deltaTime = Time.deltaTime;

        var newPosition = gameObject.transform.position + gameObject.transform.forward * _speed * deltaTime;

        gameObject.transform.position = newPosition;
        _physicsBody.MovePosition(newPosition);
    }
}
