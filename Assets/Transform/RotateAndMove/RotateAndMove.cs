using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndMove : MonoBehaviour
{
    private GameObject character;
    public Vector3 rotationAngles;
    public float distanceY = 1.0f;
    public float rotationDuration = 1.0f;
    public float waitDuration = 1.0f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    void Start()
    {
        character = this.gameObject;
        initialRotation = character.transform.rotation;
        initialPosition = character.transform.position;
        StartCoroutine(RotateAndMoveCharacter());
    }

    IEnumerator RotateAndMoveCharacter()
    {
        float elapsedTime = 0.0f;
        Vector3 targetPosition = character.transform.position + new Vector3(0, distanceY, 0);

        while (elapsedTime < rotationDuration)
        {
            character.transform.Rotate(rotationAngles * (Time.deltaTime / rotationDuration));
            character.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        character.transform.rotation = initialRotation * Quaternion.Euler(rotationAngles);
        character.transform.position = targetPosition;

        yield return new WaitForSeconds(waitDuration);

        elapsedTime = 0.0f;
        while (elapsedTime < rotationDuration)
        {
            character.transform.Rotate(-rotationAngles * (Time.deltaTime / rotationDuration));
            character.transform.position = Vector3.Lerp(targetPosition, initialPosition, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        character.transform.rotation = initialRotation;
        character.transform.position = initialPosition;
    }
}
