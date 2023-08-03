using System.Collections.Generic;
using UnityEngine;

public class Say : MonoBehaviour
{
    List<ITest> tests = new List<ITest>();
    // Start is called before the first frame update
    void Start()
    {
        tests.Add(new ICanSayHello());
        tests.Add(new ICanSayFuck());


        foreach (ITest test in tests)
        {
            test.Say();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
