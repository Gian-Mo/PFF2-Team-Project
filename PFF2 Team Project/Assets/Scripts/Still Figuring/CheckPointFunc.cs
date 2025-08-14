using UnityEngine;

public class CheckPointFunc : MonoBehaviour
{
    [SerializeField] Renderer model;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        model.material.color = Color.green;
        SettingsManager.instance.ChangeSpawnPosition(transform.position);

    }

}
