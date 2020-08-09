using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestLight : MonoBehaviour
{
    public EnemyMovementLoop.Direction currentDirection;
    private Transform lightTransform;
    private UnityEngine.Experimental.Rendering.Universal.Light2D lightComponent;
    // Start is called before the first frame update
    void Start()
    {
        lightTransform = this.gameObject.transform.GetChild(2);
        lightComponent = lightTransform.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        setLightRotation();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       setLightRotation(); 
    }

    void setLightRotation() {
        currentDirection = GetComponent<EnemyMovementLoop>().currentDirection;
        lightComponent.pointLightInnerRadius = GetComponent<EnemyMovementLoop>().hitDistance;
        lightComponent.pointLightOuterRadius = GetComponent<EnemyMovementLoop>().hitDistance;
        int desiredRotation = 0;
        if (currentDirection == EnemyMovementLoop.Direction.Up) { desiredRotation = 0; }
        if (currentDirection == EnemyMovementLoop.Direction.Left) { desiredRotation = 90; }
        if (currentDirection == EnemyMovementLoop.Direction.Down) { desiredRotation = 180; }
        if (currentDirection == EnemyMovementLoop.Direction.Right) { desiredRotation = 270; }
        lightTransform.rotation = Quaternion.Lerp(lightTransform.rotation, Quaternion.Euler(lightTransform.eulerAngles.x, lightTransform.eulerAngles.y, desiredRotation), Time.deltaTime * 10);
    }
}
