using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleFocus : MonoBehaviour
{
    private List<Transform> targets;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float minZoom = 38f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomLimiter = 50f;

    [SerializeField] private Manager ref_manager;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        
    }

    private void LateUpdate()
    {
        targets = ref_manager.GetTransforms();
        if (targets.Count == 0) { return; }

        Move();
        Zoom();
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDist() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        transform.position = centerPoint + offset;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1) { return targets[0].position; }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    float GetGreatestDist()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    public void addTarget(Transform target)
    {
        Debug.Log("add target..");
        targets.Add(target);
    }
}
