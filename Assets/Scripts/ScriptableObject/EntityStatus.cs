using UnityEngine;

[CreateAssetMenu(fileName = "Entity Status", menuName = "Scriptable Object/Entity Status", order = int.MaxValue)]
public class EntityStatus : ScriptableObject
{
    [SerializeField] private int maxHP;
    public int MaxHP { get { return maxHP; } }

    [SerializeField] private float minSpeed;
    public float MinSpeed { get { return minSpeed; } }
}
