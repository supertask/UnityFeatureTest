using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Vector3 dest = new Vector3(3, 0, 0);

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(dest);
        }    
    }
}
