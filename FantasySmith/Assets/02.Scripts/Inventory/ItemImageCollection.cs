using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1101_�ֿ���>
 * 1 - �����ۼ� �� �׽�Ʈ �Ϸ�
 * 
 * <v1.1 - 2023_1102_�ֿ���>
 * 1 - �κ��丮 �ܺο��� ������ �̹����� outerSpriteImage��, outerMaterial�� �����Ͽ���.  
 * �� ������ �� ������Ʈ�� 2d ������ ������Ʈ�� 3d ������ ������Ʈ�� ���ÿ� �����ϱ⿡�� 
 * (transform�� ���� �� ������ ������Ʈ on,off �� �̹��� ����Ī ���鿡��) ��ȿ�����̶�� �ǴܵǾ�⿡,
 * 
 * �κ��丮���� �ܺη� �������� ����� ���� ���Ӱ� ������Ʈ�� �����Ͽ� �����ֵ��� �Ѵ�.
 * (2d, 3d������Ʈ�� ���� ����)
 * 
 * 2 - �ּ� ó��
 * 
 */


/// <summary>
/// �ν����ͺ� ������ ���� ����ϱ� ���Ͽ� Ŭ���� ���ο� �̹��� ����ü�� �����մϴ�.
/// </summary>
public class ItemImageCollection : MonoBehaviour
{
    /// <summary>
    /// �ν����� �� �󿡼� ����� �̹����� ���� ����ü�Դϴ�.
    /// </summary>
    public ImageCollection[] icArrImg;
}

/// <summary>
/// ���� �̹��� �����Դϴ�. ����ȭ �Ǿ� �ν����� �� �󿡼� ������ �� �ֽ��ϴ�.
/// </summary>
[System.Serializable]
public struct ImageCollection
{
    public string itemDesc;         // �ν����ͺ信�� ������ ������ �̸� �Ǵ� ����
    public Sprite innerSprite;      // �κ��丮 ���ο��� ������ �̹���
    public Sprite statusSprite;     // �κ��丮 ����â���� Ŀ���� ��Ŀ������ �� ������ �̹���
}
