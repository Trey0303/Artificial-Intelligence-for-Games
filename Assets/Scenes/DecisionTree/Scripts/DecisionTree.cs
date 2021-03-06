using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    public Agent agent;

    public IDecision decisionTreeRoot;

    //Sets up your tree here, assigning additional decisions to the
    //left and right of the tree
    void Start()
    {
        BooleanDecision boolDecision = new BooleanDecision(true);
        boolDecision.trueDecision = new CustomPrintDecision("True");
        boolDecision.falseDecision = new CustomPrintDecision("False");

        decisionTreeRoot = boolDecision;
    }

    // Update is called once per frame
    void Update()
    {
        //decisionTreeRoot.MakeDecision();

        IDecision currentDecision = decisionTreeRoot;

        while (currentDecision != null)
        {
            currentDecision = currentDecision.MakeDecision();
        }
        //decisionTreeRoot = new PrintDecision(true);
    }
}


//The PrintDecision type implements the IDecision interface
//- When a type implements an interface, in must define all members defined by
//the interface. This case, thats's just the MakeDecision() method.
public class PrintDecision : IDecision//implements IDecision
{
    public bool branch = false;

    //default constructor
    public PrintDecision() { }

    //parameterized constructor allows you to define branch on construction
    public PrintDecision(bool branch)
    {
        //this.branch means the branch bool outside this function
        this.branch = branch;
    }

    //evaluate the decision
    public IDecision MakeDecision()
    {
        Debug.Log(branch ? "Yes" : "No");

        return null;
    }
}

public class BooleanDecision : IDecision
{
    public bool branch = false;
    public IDecision trueDecision;
    public IDecision falseDecision;

    public BooleanDecision(bool val)
    {
        branch = val;
    }

    public IDecision MakeDecision()
    {
        //return true if branch is true, else return false
        return branch ? trueDecision : falseDecision;//this line is the same as writing out the commented code right below
        //if (branch)//if true
        //{
        //    return trueDecision;
        //}
        //else
        //{
        //    return falseDecision;
        //}
    }
}

public class CustomPrintDecision : IDecision
{
    public string text = "";

    public CustomPrintDecision(string text)
    {
        this.text = text;
    }
    public IDecision MakeDecision()
    {
        Debug.Log(text);
        return null;
    }
}

