using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///   임시 핸들 작동
/// </summary>
public class Handle : MonoBehaviour
{
    public List<Road> roads;

    #region Interaction
    private void OnRotateHandle(InputValue value)
    {
        if (transform.eulerAngles.x == 0)
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.x = 90f;
            transform.eulerAngles = currentRotation;

            foreach (Road road in roads)
            {
                road.roadLinks.Remove(road.UncertainRoad);
            }
        }
        else
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.x = 0f;
            transform.eulerAngles = currentRotation;

            foreach (Road road in roads)
            {
                road.roadLinks.Add(road.UncertainRoad);
            }
        }
    }
    #endregion
}