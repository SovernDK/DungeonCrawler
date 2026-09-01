/*
 * GR Expression Solver Demo 03
 * 
 * The four expressions together create the iconic batman logo, though each
 * of them yield results for different ranges of x. the x range is -7 to 7
 *
 * For the sake of the brevity of code, I generate numbers along the x axis
 * and randomly try to get a result from one of the 4 expressions.
 *
 * FUN FACT:
 * Did you know that you could simulate abs(x) by using the expression
 * (x^2)^0.5 and it looks SO INELEGANT? Please search for '^2)^0.5' in this
 * file and see all the inelegance.
 *
 * Check out https://assetstore.unity.com/packages/slug/166250
 * for the full version of GR Expression Solver, with Mathf functions, UI and more.
 *
 * This demo is the modified version of the same example from the paid version,
 * which originally displays how to use the component form of the solver object
 * and how to name them.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRES_Demo_03 : MonoBehaviour
{
    public GRES_Solver[] batPersonLogoExpressions;
    public GameObject particles; 
    float t = -6.99f;
    int density = 3;

    // Start is called before the first frame update
    void Start()
    {
        batPersonLogoExpressions = new GRES_Solver[4];
        batPersonLogoExpressions[0] = new GRES_Solver("2*((-1*((($x^2)^0.5-1)^2)^0.5)*((3-(($x^2)^0.5))^2)^0.5/(((($x^2)^0.5)-1)*(3-(($x^2)^0.5))))^0.5 * (1+(((($x^2)^0.5)-3)^2)^0.5/((($x^2)^0.5)-3)) * (1-($x/7)^2)^0.5 +(5+0.97*((($x-0.5)^2)^0.5+(($x+0.5)^2)^0.5)-3*((($x-0.75)^2)^0.5+(($x+0.75)^2)^0.5))*(1+((1-($x^2)^0.5)^2)^0.5/(1-(($x^2)^0.5)))");
        batPersonLogoExpressions[1] = new GRES_Solver("(-3)*(1-($x/7)^2)^0.5 * ((((($x^2)^0.5)-4)^2)^0.5/((($x^2)^0.5)-4))^0.5");
        batPersonLogoExpressions[2] = new GRES_Solver("(2.71052+1.5-0.5*(($x^2)^0.5)-1.35526* (4-((($x^2)^0.5)-1)^2)^0.5 )*((((($x^2)^0.5)-1)^2)^0.5/((($x^2)^0.5)-1))^0.5 +0.9");
        batPersonLogoExpressions[3] = new GRES_Solver("(($x/2)^2)^0.5-0.0913722*$x^2-3+ (1-((((($x^2)^0.5)-2)^2)^0.5-1)^2)^0.5");

        foreach (GRES_Solver es in batPersonLogoExpressions) es.Prepare();
    }

    private void OnEnable()
    {
        Camera.main.transform.position = new Vector3(9, 25, -10);
        Camera.main.transform.localEulerAngles = new Vector3(58, -25, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GenerateBatticles();
    }

    private void GenerateBatticles()
    {
        t += 0.1f;
        if (t >= 7) t -= 14;

        for(int i = 0; i < density; i++)
        {
            float x = 0;
            float z = float.NaN;

            for(int failsafe = 0; failsafe < 10 && float.IsNaN(z); failsafe++)
            {
                x = Random.Range(-0.5f, 0.5f) + t;
                x = Mathf.Clamp(x, -7, 7);
                int exp = Random.Range(0, batPersonLogoExpressions.Length);
                batPersonLogoExpressions[exp].SetVariable(x);
                z = batPersonLogoExpressions[exp].Evaluate();
            }

            if(!float.IsNaN(z))
            {
                GameObject go = Instantiate(particles);
                go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                go.transform.SetParent(transform);
                go.transform.localPosition = new Vector3(x, 0, z);
            }
        }
    }
}
