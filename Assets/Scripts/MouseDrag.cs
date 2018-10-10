using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour {

    bool isClicked = false;
    Vector2 startPos;
    Vector2 curPos;

    /// <summary>
    /// don't manipulate with this value. 
    /// </summary>
    public Vector2 direction;
	
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("startPos " + startPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClicked = false;

            //스네이크에는 해당사항없음
            //direction = Vector2.zero;
        }

        if (isClicked)
        {
            curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = curPos - startPos;
            //direction = direction.normalized;
            direction = RoundDirection(direction);     

            //Debug.Log("direction " + direction);
        }

	}

    Vector2 RoundDirection(Vector2 dir)
    {
        Vector2 result;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            result = new Vector2(Mathf.Sign(dir.x), 0);
        }
        else
        {
            result = new Vector2(0, Mathf.Sign(dir.y));
        }

        return result;
    }
}
