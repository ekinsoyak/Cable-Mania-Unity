using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonFis : MonoBehaviour
{
    public GameObject MevcutSoket;
    public string SoketRengi;
    [SerializeField] private GameManager _gameManager;

    bool Secildi;
    bool PosDegistir;
    bool SoketOtur;
    
    GameObject HareketPozisyonu;
    GameObject SoketinKendisi;

    public void HareketEt(string islem, GameObject Soket, GameObject GidilecekObje=null){

        switch(islem){

            case "Secim":
                HareketPozisyonu = GidilecekObje;
                Secildi=true;
                break;
            case "PozisyonDegis":
                SoketinKendisi = Soket;
                HareketPozisyonu = GidilecekObje;
                PosDegistir=true;
                break;
            case "SoketeOtur":
                SoketinKendisi = Soket;
                SoketOtur=true;
                break;
        }
    }

    private void Update() {
        
        if(Secildi){

            transform.position= Vector3.Lerp(transform.position,HareketPozisyonu.transform.position,.040f);
            if(Vector3.Distance(transform.position,HareketPozisyonu.transform.position)<.010){

                Secildi=false;
            }            
        }

        if(PosDegistir){

            transform.position= Vector3.Lerp(transform.position,HareketPozisyonu.transform.position,.040f);
            if(Vector3.Distance(transform.position,HareketPozisyonu.transform.position)<.010){

                PosDegistir=false;
                SoketOtur=true;
            }
        }

        if(SoketOtur){

            transform.position= Vector3.Lerp(transform.position,SoketinKendisi.transform.position,.040f);
            if(Vector3.Distance(transform.position,SoketinKendisi.transform.position)<.010){

                SoketOtur=false;
                _gameManager.HareketVar=false;
                MevcutSoket=SoketinKendisi;
                _gameManager.FisleriKontrolEt();
            }
        }
}
}