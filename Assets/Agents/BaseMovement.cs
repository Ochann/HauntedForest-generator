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

    private float wanderRadius = 30f;
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
        
    }


    public void ChaseObject(GameObject obj)
    {
        if (ref_Manager != null)
        {
            //GameObject obj = ref_Manager.FindGameObjByName(name);
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

    public GameObject GetNearestObjByType(string type)
    {
        List<GameObject> list = null;
        GameObject nearestObj = null;
        switch(type)
        {
            case "adventurer":
                list = ref_Manager.adventurerObjs; break;
            case "forestSpirit":
                list = ref_Manager.forestSpiritObjs; break;
            case "treasure":
                list = ref_Manager.treasureObjs; break;
        }

        if (list != null)
        {
            float minDistance = Mathf.Infinity;
            foreach (GameObject obj in list)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObj = obj;
                }
            }
        }

        return nearestObj;
    }

    public void RemoveObject(GameObject obj, string type)
    {
        if (obj != null) ref_Manager.RemoveObjectByType(obj, type);
    }


    public void Wander()
    {
        //Debug.Log("called wander");
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            Vector3 targetPos = hit.position;
            if (targetPos != null) MoveToTarget(targetPos);
        }
    }

    public void FleeFromObj(GameObject obj)
    {
        Vector3 fleeDirection = transform.position - obj.transform.position;
        MoveToTarget(transform.position + fleeDirection);
    }

}
