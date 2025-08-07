using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour, IDataPersistence
//public class Coins : MonoBehaviour
{
    [SerializeField] private string id;
    private bool collected = false;
    

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collected = true;
            Debug.Log(collected);
        }
    }

    public void LoadData(GameData data)
    {
        // TODO: ONLY LOADS WHEN THE LEVEL IS COMPLETED (BOOLEAN SAYING: ISCOMPLETED)
    
        /*data.coinsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }*/
    
    }

    public void SaveData(ref GameData data)
    {
        if (data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }
        data.coinsCollected.Add(id, collected);
    }
}
