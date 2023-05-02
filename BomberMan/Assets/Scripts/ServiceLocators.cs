using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static Dictionary<object, object> _serviceContainer = null;
    public static Dictionary<object, object> serviceContainer => _serviceContainer;
    public static void InitializeContainer()
    {
        _serviceContainer = null;
        _serviceContainer = new Dictionary<object, object>()
        {
            {typeof(IMapGenerator),new MapGenerator() },
            {typeof(IPlayerSpawner),new PlayerSpawner() },
            {typeof(IEnemySpawner),new EnemySpawner() },
            {typeof(IMapProcessing),new MapProcessing() },
            //{typeof(IBomb),new Bomb() }
        };
    }
    public static T GetService<T>()
    {
        try
        {
            return (T)_serviceContainer[typeof(T)];
        }
        catch (Exception)
        {
            throw new Exception("Service not implemented");
        }
    }

}
