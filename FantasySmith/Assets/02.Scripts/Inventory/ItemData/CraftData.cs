using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * [�۾� ����]
 * 
 * <v1.0 - 2023_1105_�ֿ���>
 * 1- ���۰��� Ŭ���� ���Ϻи�   
 */
namespace CraftData
{

    /// <summary>
    /// ���� ���õ� ���� �Ӽ����� ��Ƴ��� Ŭ�����μ� �÷��̾ �������� �����ϰ� �Ǵ� ��� ������ �־�� �� ��Ͽ� ���Ե� �����Դϴ�.
    /// </summary>
    public class CraftProficiency
    {        
        private int proficiency;
        /// <summary>
        /// ����� ���� ���õ��� ���
        /// </summary>
        public int Proficiency
        {
            set {  proficiency = Mathf.Clamp(value, 0, 100); }    
            get { return proficiency; }
        }

        private int recipieHitCount;
        /// <summary>
        /// �����ǰ� ��Ȯ�ϰ� ���� Ƚ��
        /// </summary>
        public int RecipieHitCount 
        { 
            get { return recipieHitCount; } 
            set { recipieHitCount = value; }
        }

        /// <summary>
        /// ����� ��� ���� 0���� �ʱ�ȭ�Ͽ� �����մϴ�.
        /// </summary>
        public CraftProficiency()
        {
            proficiency = 0;
            RecipieHitCount = 0;
        }
    }


}
