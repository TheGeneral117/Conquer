using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurnerAI : MonoBehaviour
{
    #region Variables
    private CharacterController characterController = null;
    private Vector3 moveDirection = Vector3.zero;
    private bool paused = false;
    private float enemyY = 0.0f;
    private float lastTurned = 0.0f;
    #endregion

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyY = transform.position.y;
    }

    private void Update()
    {
        if (!paused)
        {
            moveDirection = transform.forward / 3;
            characterController.Move(moveDirection);
            transform.position = new Vector3(transform.position.x, enemyY, transform.position.z);
            if (Time.time > lastTurned + Random.Range(1, 3))
            {
                transform.Rotate(0, 90, 0);
                lastTurned = Time.time;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.Rotate(0, 90, 0);
    }

    #region Pause
    public void OnPauseGame()
    {
        paused = true;
    }

    public void OnResumeGame()
    {
        paused = false;
    }
    #endregion
}

