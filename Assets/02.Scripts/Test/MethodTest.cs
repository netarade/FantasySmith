using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodTest : MonoBehaviour
{
    private void Start()
    {
        Transform tr = GameObject.Find( "Plane" ).transform;
        if( tr!=null )
        { 
            RemoveTrObj( tr );
            
            if(tr==null)
                print("�޼��尡 ������ ���ŵǾ����ϴ�.");
        }
    }

    public void RemoveTrObj(Transform tr)
    {
        if(tr!=null)
            print("���ڸ� ���޹޾ҽ��ϴ�.");
        GameObject.Destroy(tr.gameObject);
        if(tr==null)
            print("���ŵǾ����ϴ�.");
    }


}
