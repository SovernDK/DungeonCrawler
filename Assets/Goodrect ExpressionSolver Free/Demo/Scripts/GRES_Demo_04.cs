/*
 * GR Expression Solver Demo 04
 *
 *
 * This demo originally shows how to use expressions as variables.
 * This is a feature of the paid version, available here:
 * https://assetstore.unity.com/packages/slug/166250
 *
 * But, since you happened to download the free version, here is a
 * heart for you, too. (along with the original commentary):
 *
 * ---------------------------------------------------------------
 *
 * This object has four expression solver objects attached to it.
 *
 * The two subexpressions are little expressions that repeat in the other
 * two expressions.
 *
 * <3 top and <3 bottom expressions, however, display my gratitude towards you
 * for buying this asset.
 *
 * Notice how we use heartTop and heartBottom expressions to generate line
 * pixels, even though we set variables for the subexpressions.
 *
 * This is because these subexpressions are the ones with the x variable
 * and are nested in the heart top and bottom expressions, which just define
 * the mathematical relation between the two - not how to utilize the $x variable.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRES_Demo_04 : MonoBehaviour
{
    public GRES_Solver heartBottom; 
    public GRES_Solver heartTop; 

    public Material unlitMat;

    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        heartTop    = new GRES_Solver("3/4*($x^2)^(1/3) + (1-$x^2)^(1/2)");
        heartBottom = new GRES_Solver("3/4*($x^2)^(1/3) - (1-$x^2)^(1/2)");

        heartTop.Prepare();
        heartBottom.Prepare();

        int res = 50;

        lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.positionCount = res * 2;

        for(int i = 0; i < res; i++)
        {
            float x = 2f * i / (res-1) - 1;

            heartTop.SetVariable(x);
            heartBottom.SetVariable(x);

            float yt = heartTop.Evaluate();
            float yb = heartBottom.Evaluate();

            lr.SetPosition(i, new Vector3(x, yt));
            lr.SetPosition(res*2-i-1, new Vector3(x, yb));
            lr.loop = true;

            lr.material = unlitMat;
            lr.material.SetColor("_Color", new Color(1f, 0.2f, 0.35f));
        }
    }

    private void OnEnable()
    {
        Camera.main.transform.localPosition = new Vector3(3.25f, 0.2f, -5);
        Camera.main.transform.localEulerAngles = new Vector3(0, -40, 0);
    }
}
