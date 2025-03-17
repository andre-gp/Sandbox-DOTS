using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PopulationCounter : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    private void Update() {
        if (Input.GetKeyDown(key)) {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var query = new EntityQueryBuilder(Allocator.Temp).WithAll<Status>().Build(entityManager);

            Debug.Log($"Total Population Count: {query.CalculateEntityCount()}");

        }
    }
}
