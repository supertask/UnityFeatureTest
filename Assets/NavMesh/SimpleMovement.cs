using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Vector3 _dest = new Vector3(3, 5, 0);

    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        //_agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.SetDestination(_dest);
    }

    private void Update()
    {
        //transform.rotation = Quaternion.Euler(new Vector3(50, 0, 50));

    }
}
