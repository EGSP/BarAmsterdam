using System.Collections;
using System.Collections.Generic;
using Gasanov.Extensions;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float moveTime;
    [SerializeField] private bool useXAxis;
    [SerializeField] private bool useYAxis;
    [SerializeField] private Vector3 offset;
    
    /// <summary>
    /// Текущая цель, за которой двигается камера
    /// </summary>
    public Transform Target { private get =>target; set=>target = value; }
    [SerializeField] private Transform target;

    // For smooth damp
    private Vector3 currentVelocity;
    
    // Start is called before the first frame update
    private void Start()
    {
        currentVelocity = Vector3.zero;
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        // Камера джитерит когда игрок останавливается в ячейке и начинает идти в следующую
        // Поэтому камера пока статична
        
        
        
        if(Target!=null)
            MoveToTarget();
    }

    /// <summary>
    /// Движение в сторону цели
    /// </summary>
    public void MoveToTarget()
    {


        // var newPosition = Vector3.SmoothDamp(transform.position,
        //     Target.position, ref currentVelocity,
        //     1 / moveSpeed, moveSpeed, Time.deltaTime);
        // var newPosition = Vector3.MoveTowards(transform.position,
        //     Target.position, moveSpeed * Time.deltaTime);

        var newPosition = Vector3.Lerp(transform.position, Target.position+offset,
            1 / moveTime);

        newPosition.z = -10;
        if (!useXAxis)
            newPosition.x = transform.position.x;

        if (!useYAxis)
            newPosition.y = transform.position.y;
        
        transform.position = newPosition;
    }
}
