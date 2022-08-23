using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpismaKontrol : MonoBehaviour
{
    public GameManager _gameManager;
    public int CarpismaIndex;


    private void Update() {
        
        Collider[] HitColl = Physics.OverlapBox(transform.position,transform.localScale / 2,Quaternion.identity);

        for (int i = 0; i < HitColl.Length; i++){

            if(HitColl[i].CompareTag("KabloParcasi"))
                _gameManager.CarpismayiKontrolEt(CarpismaIndex,false);
            else
                _gameManager.CarpismayiKontrolEt(CarpismaIndex,true);
            
            
        }

    }

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,transform.localScale / 2);

    }
}
