using UnityEngine;
using UnityEngine.Rendering;

public class ParticleSelfDestruct : MonoBehaviour
{
    private Volume volume;

    private float startWeight;
    private float currentWeight;

    private void Start()
    {
        volume = GetComponent<Volume>();

        if (volume == null)
        {
            Debug.LogError("Volume component not found. Exiting...");
            enabled = false;
            return;
        }

        startWeight = volume.weight;
        currentWeight = startWeight;
    }

    private void Update()
    {
        if (currentWeight <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        currentWeight -= Time.deltaTime;

        volume.weight = currentWeight;
    }
}
