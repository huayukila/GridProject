using System;
using System.Collections.Generic;
using Framework.BuildProject;

public class BuildingPool : Singleton<BuildingPool>
{
    private SimpleObjectPool<BuildingObj> m_BuildingObjPool;

    private Dictionary<Type, List<BuildingObj>> m_BuildingObjDic;
    public void Init()
    {
        m_BuildingObjPool = new SimpleObjectPool<BuildingObj>(() => new BuildingObj(),
            e => { e.Reset(); }, 50);
    }

    public T GetBuildingObj<T>(BuildingData data) where T : BuildingObj
    {
        return (T)m_BuildingObjPool.Allocate();
    }

    public void RecycleBuildingObj(BuildingObj obj)
    {
        m_BuildingObjPool.Recycle(obj);
    }
}