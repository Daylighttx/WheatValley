using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;
        private Transform itemParent;

        private void OnEnable()
        {
            EventHandler.InstantiateItemInScence += OnInstantiateItemInScence;
        }
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScence -= OnInstantiateItemInScence;
        }
        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        private void OnInstantiateItemInScence(int itemID, Vector3 pos)
        {
            var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = itemID;
        }

    }
}