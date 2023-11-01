using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/*
 * [�۾� ����]
 * 
 * v1.0 - 2023_1101_�ֿ���
 * �����ۼ� �� �׽�Ʈ �Ϸ�
 */
public class ItemImageCollection : MonoBehaviour
{
    public ImageCollection[] imgArr;
}

[System.Serializable]
public struct ImageCollection
{
    public string itemDesc;         // ������ ����
    public Sprite innerSprite;      // �κ��丮 ���ο��� ������ �̹���
    public Sprite statusSprite;     // �κ��丮 ����â���� Ŀ���� ��Ŀ������ �� ������ �̹���
    public Sprite outerSprite;      // �κ��丮 �ܺο��� ������ 2d �̹���
    public Material outerMaterial;  // �κ��丮 �ܺο��� ������ 3d ���͸���
}
