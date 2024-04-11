using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMovement : MonoBehaviour
{
    private float m_AIMovement;
    private Rigidbody2D m_Rigidbody;
    public float m_Speed = 20f;

    private Manager ref_Manager;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        GameObject managerObj = GameObject.Find("Manager");
        if (managerObj != null)
        {
            ref_Manager = managerObj.GetComponent<Manager>();
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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

    public void ChaseObject(string name)
    {
        if (ref_Manager != null)
        {
            GameObject obj = ref_Manager.FindGameObjByName(name);
            if (obj != null)
            {
                MoveToTarget(obj.transform.position);
            }
        }
    }

    public void MoveToTarget(Vector3 position)
    {
        float distance = Vector2.Distance(transform.position, position);
        Vector2 direction = position - this.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //float speed = 2f;
        if (distance < 60f)
        {
            //transform.position = Vector2.MoveTowards(this.transform.position, obj.transform.position, speed * Time.deltaTime);
            agent.SetDestination(position);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
