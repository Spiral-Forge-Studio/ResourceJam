using UnityEngine;

public interface INode
{
    void takeHealthDamage(float amount);

    Transform GetTransform();
}
