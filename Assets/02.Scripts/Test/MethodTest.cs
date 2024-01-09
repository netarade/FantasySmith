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
                print("메서드가 끝나고 제거되었습니다.");
        }
    }

    public void RemoveTrObj(Transform tr)
    {
        if(tr!=null)
            print("인자를 전달받았습니다.");
        GameObject.Destroy(tr.gameObject);
        if(tr==null)
            print("제거되었습니다.");
    }


}
