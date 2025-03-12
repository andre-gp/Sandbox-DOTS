using Sirenix.OdinInspector;
using Unity.Entities;
using UnityEngine;
using DOTSLearningCore;
using Unity.Collections;

public class ForceStopSystem : MonoBehaviour
{
    [Button]
    public void ActivateMoverSystem(bool activate) {
        World world = World.DefaultGameObjectInjectionWorld;
        var systemHandle = world.Unmanaged.GetExistingUnmanagedSystem<UnitMoverSystem>();

        //var system = world.Unmanaged.GetUnsafeSystemRef<UnitMoverSystem>(systemHandle);
        world.Unmanaged.GetExistingSystemState<UnitMoverSystem>().Enabled = activate;        
    }

    [Button]
    public void ActivateAll(bool activate) {
        World world = World.DefaultGameObjectInjectionWorld;
        var systemHandle = world.Unmanaged.GetExistingUnmanagedSystem<UnitMoverSystem>();

        //var system = world.Unmanaged.GetUnsafeSystemRef<UnitMoverSystem>(systemHandle);
        foreach (var item in world.Unmanaged.GetAllSystems(Allocator.Temp)) {
            world.Unmanaged.ResolveSystemStateRef(item).Enabled = activate;
        }
        
    }
}
