using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeIndicator : MonoBehaviour
{
    public float range;
    private SpriteRenderer sprite;

    private void Start()
    {
        SetRangeIndicator(range);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sprite.color = new Color(0f, 1f, 0f, 0.3f);
    }


    public void SetRangeIndicator(float range)
    {
        // Assume the circle sprite has a diameter of 1 unit in its import settings
        float scale = range * 2f; // Scale it to match the range diameter
        gameObject.transform.localScale = new Vector3(scale, scale, 1f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a visual representation of the range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
