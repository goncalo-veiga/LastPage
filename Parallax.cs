using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Parallax : MonoBehaviour
{

    public CinemachineVirtualCamera cam;
    public Transform subject;

    Vector2 startPosition;
    float startZ;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    float distanceFromSubject => transform.position.z - subject.position.z;

    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.m_Lens.FarClipPlane : cam.m_Lens.NearClipPlane));



    float parallaxFactor => Mathf.Abs(distanceFromSubject)/clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPosition + travel;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
