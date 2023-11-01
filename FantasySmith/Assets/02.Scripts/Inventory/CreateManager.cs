using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData;
using System;
using UnityEditor.UIElements;

/*
 * [작업 사항]
 * 
 * v1.0 - 2023_1101_최원준
 * 테스트용 클래스 작성
 */
public class CreateManager : MonoBehaviour
{
    public Item item;
    public Item item2;
    public ItemImageCollection imageCollection;
    Dictionary<string, Item> allItemDic;
    List<Item> playerMakingList;
    List<Item> playerInventory;


    void Start()
    {
        allItemDic=new Dictionary<string, Item>()
        {
            { "철", new ItemMisc( ItemType.Basic,"0000000", "철", 3.0f ) },
            { "강철", new ItemMisc( ItemType.Basic,"0000001", "강철", 5.0f ) },
            { "흑철", new ItemMisc( ItemType.Basic,"0000002", "흑철", 7.0f ) },
            { "미스릴", new ItemMisc( ItemType.Basic,"0000003", "미스릴", 20.0f ) }
        };
        
        item2 = new ItemMisc();

        print(allItemDic["0000000"].Name);

    }




}
