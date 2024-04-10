using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
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
        float speedX = m_AIMovement * m_Speed * Time.deltaTime;
        float speedY = m_AIMovement * m_Speed * Time.deltaTime;
        m_Rigidbody.velocity = new Vector2(speedX, speedY);
    }

    public void AIMove(float move)
    {
        m_AIMovement = (move > 1) ? 1 : (move < -1) ? -1 : move;
        //Debug.Log(m_AIMovement);
    }
}
