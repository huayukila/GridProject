using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.BuildProject
{
    public class BuildMenu : BuildController
    {
        public List<Button> buttonList;
        // Start is called before the first frame update
        void Start()
        {
            buttonList[0].onClick.AddListener(() =>
            {
                this.SendCommand<SelectHouse>();
            });
            buttonList[1].onClick.AddListener(() =>
            {
                this.SendCommand<SelectFactory1>();
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            foreach (Button button in buttonList)
            {
                button.onClick.RemoveAllListeners();
            }
            buttonList.Clear();
        }
    }
}