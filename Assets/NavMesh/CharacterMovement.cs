using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    public Vector3 _dest = new Vector3(3, 0, 0);
    private float _forwardAmount = 0.5f;

    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = true; //applying position from animation
        //_animator.applyRootMotion = false;
        _animator.enabled = false;

        _agent.updatePosition = true;
        _agent.updateRotation = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.enabled = true;
            _animator.Play("Walk");
        }
    }

    private void OnAnimatorMove()
    {
        this.UpdatePosition();

        //_agent.SetDestination(_dest);
    }

    private void FixedUpdate()
    {
        //_animator.SetFloat("Forward", _forwardAmount, 0.1f, Time.deltaTime);
    }


    private void UpdatePosition()
    {
        var position = _animator.rootPosition;

        transform.position = position;

        Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;

        if (worldDeltaPosition.magnitude > _agent.radius)
            _agent.nextPosition = transform.position + 0.3f * worldDeltaPosition;
    }


}
