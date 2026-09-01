/*
 * GR Expression Solver Demo 01
 *
 * GR Expression Solver is a lightweight, mathematical expression solver package.
 * With it, you can easily instantiate or animate objects based on complex
 * mathematical expressions.
 *
 * This is the free version, which can interpret +, -, *, /, %, ^, ( and )
 * and can take one variable. Checkout https://assetstore.unity.com/packages/slug/166250
 * for the full version.
 *
 * Let's go over the basics of the class.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GRES_Demo_01: MonoBehaviour
{
    public InputField expressionField;
    [Space]
    public Text result;
    [Space]
    public InputField a;

    private void OnEnable()
    {
        GRES_Solver solver = new GRES_Solver();

        /*
         * GRES_Solver has 7 public functions,
         * one of which is a static method.
         * Namely,
         * * SetExpression(string)
         * * Prepare()
         * * float Evaluate()
         * * SetVariable(string, float)
         * * SetExpVariable(string, GRES_Solver)
         * * SetVariables(float, ..., float)
         * *
         * * static float InlineEvaluation(string, float, ..., float)
         * Let's go over them.
         */

        solver.SetExpression("-((-90.5 / (7 - (1.75 + 4) ^ 2)) * twopi) - (2 + (8 % 1.5))");
        /*
         * SetExpression(string) function sets the equation that will be used
         * for evaluation. The example above shows all the available
         * operations.
         */

        solver.Prepare();
        /*
         * Prepare() function converts the (relatively) human readable function
         * into the string of tokens that the GRES_Solver will use to evaluate
         * the expression.
         *
         * IMPORTANT: For expressions without any variables, such as the one
         * above, it is not crucial. Evaluate() function will call it, if the
         * solver isn't prepared yet. But, if the expression has variables,
         * you need to prepare it before setting the variables.
         */

        float evaluation = solver.Evaluate();
        Debug.Log(evaluation);

        /*
         * Evaluate() function evaluates the expression and returns the value
         * as float. Notice that the return value is always float.
         */

        solver.SetExpression("($x - 1)^2");
        solver.Prepare();

        /*
         * Variables in expressions are denoted by a '$' prefix.
         * 
         * The equation above is a simple quadratic equation with one variable, x"
         */

        evaluation = solver.Evaluate();
        Debug.Log("($x - 1)^2 but without setting the variables: " + evaluation);

        /*
         * When an expression is prepared for evaluation, all the constant variables
         * are created with the value zero. So, as a result, the evaluation for the
         * expression above is (0 - 1) ^ 2 = (-1)^2 = 1.
         */

        solver.SetVariable(5.67f);
        evaluation = solver.Evaluate();
        Debug.Log("($x - 1)^2 where x is 5.67: " + evaluation);

        /*
         * Once an expression is prepared, you do not need to prepare it over
         * and over for new evaluations.
         *
         * Let's calculate a 100 values for different values of x between -3 to 3.
         * Why -3 to 3? I mean, why not?
         *
         * Note: Notice that the name is provided as "x", not "$x". GRES_Solver
         * can understand both with and without the variable prefix.
         */

        float[] evaluations = new float[20];
        for (int i = 0; i < 20; i++)
        {
            solver.SetVariable(6f * i / 19f - 3f);
            evaluations[i] = solver.Evaluate();
        }
        Debug.Log("($x - 1)^2 for -3 <= x <= 3:\n" + string.Join("\n", evaluations));

        /*
         * Fun fact. when you change the expression, but use the same variable
         * names, GRES_Solver retains the values across expressions.
         */

        solver.SetExpression("$x");
        Debug.Log("Last x value: " + solver.Evaluate());
        /*
         * This is not an expression so much as a constant value.
         * But, this example illustrates the variable retention.
         *
         * Since the previous loop ended where $x is equal to 3, the evaluation
         * above yields 3.
         */


        solver.SetExpression("10i/3i");
        Debug.Log("Dividing integer 10i by 3i\n" + solver.Evaluate());

        /*
         * Any number in an expression is a float, unless stated otherwise.
         * And how do you state otherwise? By putting an i at the end of a number.
         *
         * Unlike C#, integers are marked by an i at the end, instead of having
         * floats marked by an f.
         */
    }

    public void EvaluateDemoExpression()
    {
        float.TryParse(a.text, out float af);

        GRES_Solver s = new GRES_Solver(expressionField.text);
        s.Prepare();

        s.SetVariable(af);

        result.text = s.Evaluate().ToString();
    }
}
