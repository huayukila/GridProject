using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.BuildProject
{
    public class GameManager : BuildController
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}