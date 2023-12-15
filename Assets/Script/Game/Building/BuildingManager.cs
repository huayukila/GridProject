namespace Framework.BuildProject
{
    public class BuildingManager : BuildController
    {
        private IBuildingSystem m_BuildingSystem;
        private IBuildingObjModel m_BuildingObjModel;
        private void Awake()
        {
            m_BuildingSystem=this.GetSystem<IBuildingSystem>();
            m_BuildingObjModel = this.GetModel<IBuildingObjModel>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            m_BuildingObjModel.UpdateBuildings();
        }
    }
}