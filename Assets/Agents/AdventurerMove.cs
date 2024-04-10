using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerMove : MonoBehaviour
{
    private float m_AIMovement;
    private Rigidbody2D m_Rigidbody;
    public float m_Speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
    }

    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        //Vector2 movement = transform.forward * m_AIMovement * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        //m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        //Debug.Log(movement);
        //Debug.Log(m_Rigidbody.position);

        float speedX = m_AIMovement * m_Speed * Time.deltaTime;
        float speedY = m_AIMovement * m_Speed * Time.deltaTime;
        m_Rigidbody.velocity = new Vector2(speedX, speedY);
    }

    public void AIMove(float move)
    {
        //Debug.Log("ai wandering...");
        m_AIMovement = (move > 1) ? 1 : (move < -1) ? -1 : move;
        Debug.Log(m_AIMovement);
    }
}
