using System;
using UnityEngine;
/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1101_�ֿ���>
 * 1- �����ۼ� �� �׽�Ʈ �Ϸ�
 * 
 * <v2.0 - 2023_1102_�ֿ���>
 * 1- �κ��丮 �ܺο��� ������ �̹����� outerSpriteImage��, outerMaterial�� �����Ͽ���.  
 * �� ������ �� ������Ʈ�� 2d ������ ������Ʈ�� 3d ������ ������Ʈ�� ���ÿ� �����ϱ⿡�� 
 * (transform�� ���� �� ������ ������Ʈ on,off �� �̹��� ����Ī ���鿡��) ��ȿ�����̶�� �ǴܵǾ�⿡,
 * 
 * �κ��丮���� �ܺη� �������� ����� ���� ���Ӱ� ������Ʈ�� �����Ͽ� �����ֵ��� �Ѵ�.
 * (2d, 3d������Ʈ�� ���� ����)
 * 
 * 2- �ּ� ó��
 * 
 *<v3.0 - 2023_1227_�ֿ���>
 *1- �������� �ܺο��� 3D������Ʈ�� �������� ���� MeshFilter�� Material���� �߰�
 * 
 *<v4.0 - 2024_0108_�ֿ���>
 * 1- MeshFitler�� Material������ �����ϰ� GameObject outerPrefab������ �߰��Ͽ���.
 * 3D ������Ʈ�� ǥ���ϱ� ���� �̸� 2D������ ������ ������Ʈ�� �ٿ����� MeshFilter�� Material�� ��ü�ϴ� ��Ŀ���
 * ������ 3D �������� �����Ͽ� �ش� �������� 2D������Ʈ ������ �ٿ��� �������ִ� ������� �����ϱ� ����.
 * 
 * 2- CreateManagement ���ӽ����̽��� ����
 * 
 * 3- ItemImageCollectionŬ���� �� ImageCollection����ü�� VisualCollection ��Ī���� ����
 * 
 * 4- icArrImg�������� vcArr�� ����
 * 
 */

namespace CreateManagement
{

    /// <summary>
    /// �ν����ͺ� ������ ���� ����ϱ� ���Ͽ� Ŭ���� ���ο� �̹��� ����ü�� �����մϴ�.
    /// </summary>
    public class ItemVisualCollection : MonoBehaviour
    {
        /// <summary>
        /// �ν����� �� �󿡼� ����� �̹����� ���� ����ü�Դϴ�.
        /// </summary>
        public VisualCollection[] vcArr;
    }

    /// <summary>
    /// ���� �̹��� �����Դϴ�. ����ȭ �Ǿ� �ν����� �� �󿡼� ������ �� �ֽ��ϴ�.
    /// </summary>
    [Serializable]
    public struct VisualCollection
    {
        public string itemDesc;         // �ν����ͺ信�� ������ ������ �̸� �Ǵ� ����
        public Sprite innerSprite;      // �κ��丮 ���ο��� ������ �̹���
        public Sprite statusSprite;     // �κ��丮 ����â���� Ŀ���� ��Ŀ������ �� ������ �̹���
        public GameObject outerPrefab;  // �������� 3D ���� �󿡼� ������ ������ ����
    }



    




}
