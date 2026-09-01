/*
 * GR Expression Solver Demo 02
 *
 * In this example, we see how to use a mathematical expression to create
 * a bouncing effect, as well as creating an expression solver object.
 * 
 * The sphere is transformed across the function with one variable.
 * $x variable controls the x position.
 *
 * FUN FACT:
 * If this were the paid version (available here: https://assetstore.unity.com/packages/slug/166250)
 * with functions and multiple variables and more, it would only take a single line,
 * without needing all the mess with the two extra expression solvers, string
 * conversion and plugging in bits and pieces manually.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRES_Demo_02 : MonoBehaviour
{
    public GRES_Solver bounce;
    public GRES_Solver prepForSine;
    public GRES_Solver sineSolver;
    private float x = 0;
    private Color maxColor = new Color(.79f, 1, .55f);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Color"));
        GetComponent<Renderer>().material.SetColor("_Color", Color.black);

        prepForSine = new GRES_Solver("(($x/5)^($x/5))%6.28-3.14");
        prepForSine.Prepare();

        sineSolver = new GRES_Solver("($x) - ($x)^3/6 + ($x)^5/120 - ($x)^7/5040 + ($x)^9/362880 - ($x)^11/39916800 + ($x)^13/6227020800");
        sineSolver.Prepare();

        bounce = new GRES_Solver();
        bounce.Prepare();
        bounce.SetVariable(x); //use loopX evaluation for x
    }

    private void OnEnable()
    {
        Camera.main.transform.localPosition = new Vector3(3f, 3f, -28f);
        Camera.main.transform.localEulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = (x + 0.1f) % 15;

        prepForSine.SetVariable(x);
        sineSolver.SetVariable(prepForSine.Evaluate());

        bounce.SetExpression("((" + sineSolver.Evaluate().ToString() + "/2^((($x/5)^($x/5)-1.57)/3.14))^2)^0.5*5");
        bounce.Prepare();
        bounce.SetVariable(x);
        float y = bounce.Evaluate();

        GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(Color.black, maxColor, y / 5));
        transform.position = new Vector3(x, y, 0); //use last loopX evaluation for x and bounce evaluation for y
    }
}
