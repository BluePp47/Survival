// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;

// [System.Serializable]
// public class MakingArchitectureItem
// {
//     public BuildItem builditemData;
//     public MakingArchitectureItem(BuildItem itemdata)
//     {
//         builditemData =itemdata;
//     }
// }
// public class MakingArchitecture : MonoBehaviour
// {
//     public MakeItemSlot[] slots;
//     public GameObject inventoryWindow;
//     public GameObject ArchitectureWindow;
//     public Transform slotPanel;

//     [Header("Selected Item")]
//     private MakeItemSlot selectedItem;
//     private int selectedItemIndex;
//     public TextMeshProUGUI selectedItemName;
//     public TextMeshProUGUI selectedItemDescription;
//     public TextMeshProUGUI selectedItemStatName;
//     public TextMeshProUGUI selectedItemStatValue;
//     public GameObject makeButton;

//     private int curEquipIndex;

//     private PlayerController controller;
//     private PlayerCondition condition;

//     //1. 아이템슬롯 프리팹스 안 아이콘에 Item들을 순서대로 넣음.


    
// public class InventoryItem
// {
//     public ItemData data;
//     public int quantity;

//     public InventoryItem(ItemData itemData, int amount = 1)
//     {
//         data = itemData;
//         quantity = amount;
//     }
// }

// public class Inventory : MonoBehaviour
// {
//     public List<InventoryItem> items = new List<InventoryItem>();
//     public PlayerController player;


//     // 2. Item의 이름과 설명을 보여줌 
//     // 3. Item에 필요한 재료를 설정함
//     // 4. 필요한 재료를 보여줌
//     // 5. 만들기 버튼을 누르면
//     // 6. 사운드 재생 후
//     // 7. 알림창 뜨고
//     // 8. 설계도에 추가 됨


    





//     void Start()
//     {
//         controller = CharacterManager.Instance.Player.controller;
//         condition = CharacterManager.Instance.Player.condition;

//         controller.inventory += Toggle;
//         CharacterManager.Instance.Player.addItem += AddItem;

//         inventoryWindow.SetActive(false);
//         slots = new MakeItemSlot[slotPanel.childCount];

//         for(int i = 0; i < slots.Length; i++)
//         {
//             slots[i] = slotPanel.GetChild(i).GetComponent<MakeItemSlot>();
//             slots[i].index = i;
//             slots[i].inventory = this;
//             slots[i].Clear();
//         }

//         ClearSelectedItemWindow();
//     }

// 		void ClearSelectedItemWindow()
//     {
//         selectedItem = null;

//         selectedItemName.text = string.Empty;
//         selectedItemDescription.text = string.Empty;
//         selectedItemStatName.text = string.Empty;
//         selectedItemStatValue.text = string.Empty;

//         makeButton.SetActive(false);
//     }

//     public void Toggle()
//     {
//         if (IsOpen())
//         {
//             inventoryWindow.SetActive(false);
//         }
//         else
//         {
//             inventoryWindow.SetActive(true);
//         }
//     }

//     public bool IsOpen()
//     {
//         return inventoryWindow.activeInHierarchy;
//     }

// 		// PlayerController 먼저 수정

//     public void AddItem()
//     {
//         ItemData data = CharacterManager.Instance.Player.itemData;

//         if (data.canStack)
//         {
//             ItemSlot slot = GetItemStack(data);
//             if(slot != null)
//             {
//                 slot.quantity++;
//                 UpdateUI();
//                 CharacterManager.Instance.Player.itemData = null;
//                 return;
//             }
//         }

//         ItemSlot emptySlot = GetEmptySlot();

//         if(emptySlot != null)
//         {
//             emptySlot.item = data;
//             emptySlot.quantity = 1;
//             UpdateUI();
//             CharacterManager.Instance.Player.itemData = null;
//             return;
//         }

//         ThrowItem(data);
//         CharacterManager.Instance.Player.itemData = null;
//     }

//     public void UpdateUI()
//     {
//         for(int i = 0; i < slots.Length; i++)
//         {
//             if (slots[i].item != null)
//             {
//                 slots[i].Set();
//             }
//             else
//             {
//                 slots[i].Clear();
//             }
//         }
//     }

//     ItemSlot GetItemStack(ItemData data)
//     {
//         for(int i = 0; i < slots.Length; i++)
//         {
//             if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
//             {
//                 return slots[i];
//             }
//         }
//         return null;
//     }

//     ItemSlot GetEmptySlot()
//     {
//         for(int i = 0; i < slots.Length; i++)
//         {
//             if (slots[i].item == null)
//             {
//                 return slots[i];
//             }
//         }
//         return null;
//     }



// 		// ItemSlot 스크립트 먼저 수정
//     public void SelectItem(int index)
//     {
//         if (slots[index].item == null) return;

//         selectedItem = slots[index];
//         selectedItemIndex = index;

//         selectedItemName.text = selectedItem.item.displayName;
//         selectedItemDescription.text = selectedItem.item.description;

//         selectedItemStatName.text = string.Empty;
//         selectedItemStatValue.text = string.Empty;

//         for(int i = 0; i< selectedItem.item.consumables.Length; i++)
//         {
//             selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
//             selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
//         }

//         makeButton.SetActive(selectedItem.item.type == ItemType.Consumable); 
//     }

//     public void OnUseButton()
//     {
//         if(selectedItem.item.type == ItemType.Consumable)
//         {
//             for(int i = 0; i < selectedItem.item.consumables.Length; i++)
//             {
//                 switch (selectedItem.item.consumables[i].type)
//                 {
//                     case ConsumableType.Health:
//                         condition.Heal(selectedItem.item.consumables[i].value); break;
//                     case ConsumableType.Hunger:
//                         condition.Eat(selectedItem.item.consumables[i].value);break;
//                 }
//             }
//             RemoveSelctedItem();
//         }
//     }


//     void RemoveSelctedItem()
//     {
//         selectedItem.quantity--;

//         if(selectedItem.quantity <= 0)
//         {
//             if (slots[selectedItemIndex].equipped)
//             {
//                 UnEquip(selectedItemIndex);
//             }

//             selectedItem.item = null;
//             ClearSelectedItemWindow();
//         }

//         UpdateUI();
//     }

//     public bool HasItem(ItemData item, int quantity)
//     {
//         return false;
//     }
// }
