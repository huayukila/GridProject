using System;
using System.Collections.Generic;
using Framework.BuildProject;

public class BuildingPool : Singleton<BuildingPool>
{
    private SimpleObjectPool<BuildingBase> m_BuildingObjPool;

    private Dictionary<Type, List<BuildingBase>> m_BuildingObjDic;
    public void Init()
    {
        m_BuildingObjPool = new SimpleObjectPool<BuildingBase>(() => new BuildingBase(),
            e => { e.Reset(); }, 50);
    }

    public T GetBuildingObj<T>(BuildingData data) where T : BuildingBase
    {
        return (T)m_BuildingObjPool.Allocate();
    }

    public void RecycleBuildingObj(BuildingBase @base)
    {
        m_BuildingObjPool.Recycle(@base);
    }
}