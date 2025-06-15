using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighlightImage;
        [SerializeField] private Button button;

        [Header("格子类型")]
        public SlotType slotType;

        public bool isSelected;

        public ItemDetails itemDetails;
        public int itemAmount;
        public int slotIndex;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        public void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子和UI信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            slotImage.enabled = true;
            amountText.text = amount.ToString();
            button.interactable = true;
        }
        /// <summary>
        /// 将slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = true;
            }
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(itemAmount == 0) return;
            isSelected = !isSelected;
            inventoryUI.UpdateSlotHighlight(slotIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount == 0) return;
            inventoryUI.dragImage.enabled = true;
            inventoryUI.dragImage.sprite = slotImage.sprite;
            inventoryUI.dragImage.SetNativeSize();
            isSelected = true;
            inventoryUI.UpdateSlotHighlight(slotIndex);
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragImage.enabled = false;
            //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) return;
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;

                //在Player自身背包范围内交换
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                //取消所有高亮显示
                inventoryUI.UpdateSlotHighlight(-1);
            }
            //测试仍在地上
            else
            {
                if (itemDetails.canDropped)
                {
                    //鼠标对应世界地图坐标
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    EventHandler.CallInstantiateItemInScence(itemDetails.itemID, pos);
                }
            }
        }
    }
}
