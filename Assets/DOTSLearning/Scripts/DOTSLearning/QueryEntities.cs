using DOTSLearningCore;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

public class QueryEntities : MonoBehaviour
{
    private void Update() {
        
        if (Input.GetKey(KeyCode.Space)) {            
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, MoveSpeed>()
                .Build(entityManager); // Automatically disposed at the end of the frame.

            var entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            var transformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
            var moveSpeedArray = entityQuery.ToComponentDataArray<MoveSpeed>(Allocator.Temp);

            for (int i = 0; i < entityArray.Length; i++) {
                var currentTransform = transformArray[i];
                currentTransform.Scale += Time.deltaTime*moveSpeedArray[i].moveSpeed;
                transformArray[i] = currentTransform;
            }

            entityQuery.CopyFromComponentDataArray(transformArray);
        }
    }

}
