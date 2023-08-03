using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITest
{
    public void Say();
}

public abstract class Test : ITest
{
    public abstract void Say();
}


public class ICanSayHello : Test
{
    public override void Say()
    {
        Debug.Log("hello");
    }
}

public class ICanSayFuck : Test
{
    public override void Say()
    {
        Debug.Log("Fuck");
    }
}

