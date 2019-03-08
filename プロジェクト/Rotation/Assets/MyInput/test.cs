using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var gamepad = MyInputManager.AllController;

        if (gamepad.A)
        {
            Debug.Log("A");
        }
        if (gamepad.B)
        {
            Debug.Log("B");
        }
        if (gamepad.X)
        {
            Debug.Log("X");
        }
        if (gamepad.Y)
        {
            Debug.Log("Y");
        }
        if (gamepad.START)
        {
            Debug.Log("START");
        }
        if (gamepad.BACK)
        {
            Debug.Log("BACK");
        }
        if (gamepad.LB)
        {
            Debug.Log("LB");
        }
        if (gamepad.RB)
        {
            Debug.Log("RB");
        }

        if (gamepad.Arrow_Y > 0)
        {
            Debug.Log("上矢印");
        }
        if (gamepad.Arrow_Y < -0)
        {
            Debug.Log("下矢印");
        }
        if (gamepad.Arrow_X > 0)
        {
            Debug.Log("右矢印");
        }
        if (gamepad.Arrow_X < -0)
        {
            Debug.Log("左矢印");
        }

        if (gamepad.LStick.x > 0)
        {
            Debug.Log("左アナログ-右");
        }

        if (gamepad.LStick.x < 0)
        {
            Debug.Log("左アナログ-左");
        }

        if (gamepad.LStick.y > 0)
        {
            Debug.Log("左アナログ-上");
        }

        if (gamepad.LStick.y < 0)
        {
            Debug.Log("左アナログ-下");
        }

        if (gamepad.RStick.x > 0)
        {
            Debug.Log("右アナログ-右");
        }

        if (gamepad.RStick.x < 0)
        {
            Debug.Log("右アナログ-左");
        }

        if (gamepad.RStick.y > 0)
        {
            Debug.Log("右アナログ-上");
        }

        if (gamepad.RStick.y < 0)
        {
            Debug.Log("右アナログ-下");
        }

        if (gamepad.LT)
        {
            Debug.Log("LT");
        }

        if (gamepad.RT)
        {
            Debug.Log("RT");
        }

    }
}
