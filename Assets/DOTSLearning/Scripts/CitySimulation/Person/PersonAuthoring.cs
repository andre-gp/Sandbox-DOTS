using Unity.Entities;
using UnityEngine;

class PersonAuthoring : MonoBehaviour {    
    class Baker : Baker<PersonAuthoring> {
        public override void Bake(PersonAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Status());
        }
    }
}

public struct Status : IComponentData {
    public int hunger;
    public int thirst;

    public int sleep;
    public int bladder;
    public int hygiene;
    public int energy;

    public int health;

    public int fun;
    public int social;

    public int stress;
    public int temperature;

    public int intoxication;
    public int fear;
}

