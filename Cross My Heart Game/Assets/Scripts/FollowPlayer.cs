using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    
    public GameObject map;
    public bool custom=false;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public Vector2 minPositionCustom;
    public Vector2 maxPositionCustom;

    // Start is called before the first frame update
    void Start()
    {
        if(custom){
            minPosition = minPositionCustom;
            maxPosition = maxPositionCustom;
        } else {
            minPosition = new Vector2(map.GetComponent<Tilemap>().cellBounds.xMin,map.GetComponent<Tilemap>().cellBounds.yMin);
            maxPosition = new Vector2(map.GetComponent<Tilemap>().cellBounds.xMax, map.GetComponent<Tilemap>().cellBounds.yMax);
        }
    }

    // LateUpdate is called last per frame
    void LateUpdate()
    {
        if(transform.position != target.position){
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            // clamping
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y,maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
