using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementController
{

    #region Unity Events
    protected new void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            useDampeners = !useDampeners;
        }
        
        Camera.main.gameObject.transform.position = 
            new Vector3(
                transform.position.x, 
                transform.position.y, 
                Camera.main.transform.position.z
            );
    }
    #endregion

    Vector3 getMousePos()
    {
        return Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    transform.position.z - Camera.main.transform.position.z
                )
            );
    }

    protected override Vector3 getTargetPos()
    {
        if (Input.GetMouseButton(1))
        {
            return getMousePos();
        }
        return transform.position;
    }

    protected override float getTargetAngle()
    {
        return Vector2.SignedAngle(Vector2.right, getMousePos() - transform.position);
    }
}
