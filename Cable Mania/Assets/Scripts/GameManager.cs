using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject SeciliObje;
    GameObject SeciliSoket;

    public bool HareketVar;

    [Header("----LEVEL AYARLARI")]
    public GameObject[] CarpismaKontrolObjeleri;
    public GameObject[] Fisler;
    public int HedefSoketSayisi;
    public int HamleHakki;
    public List<bool> CarpismaDurumlari;
    int TamamlanmaSayisi;
    int CarpmaKontrolSayisi;

    [Header("----DIGER OBJELER")]

    public GameObject[] Isiklar;

    [Header("----UI OBJELERI")]
    public GameObject KontrolPaneli;
    public TextMeshProUGUI KontrolText;
    public TextMeshProUGUI HamleHakkiText;
    public TextMeshProUGUI[] UITextleri;
    public GameObject[] Paneller;
    public AudioSource FisSesi;
    

    private void Start() {

        HamleHakkiText.text="HAK : " + HamleHakki.ToString();

        for (int i = 0; i < HedefSoketSayisi-1; i++){

            CarpismaDurumlari.Add(false);
            
        }
        
    }
    
    private void Update() {
        
        if(Input.GetMouseButtonDown(0)){

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,100)){

                if(hit.collider !=null){
                    // ## SON FiS
                    if(SeciliObje==null && !HareketVar){

                        if(hit.collider.CompareTag("Mavi_Fis") ||hit.collider.CompareTag("Sari_Fis") ||hit.collider.CompareTag("Kirmizi_Fis")){
                        FisSesi.Play();
                        SonFis _sonFis= hit.collider.GetComponent<SonFis>();

                            _sonFis.HareketEt(
                                "Secim",
                                _sonFis.MevcutSoket,
                                _sonFis.MevcutSoket.GetComponent<Soket>().HareketPozisyonu);

                        SeciliObje=hit.collider.gameObject;
                        SeciliSoket = _sonFis.MevcutSoket;
                        HareketVar=true;   
                    }
                    }
                    // ## SON FiS

                    // ## SOKET

                    if(hit.collider.CompareTag("Soket")){

                        if(SeciliObje!=null && !hit.collider.GetComponent<Soket>().Doluluk && SeciliSoket!=hit.collider.gameObject){

                            SeciliSoket.GetComponent<Soket>().Doluluk=false;

                            Soket _soket=hit.collider.GetComponent<Soket>();

                            SeciliObje.GetComponent<SonFis>().HareketEt(
                                "PozisyonDegis",
                                hit.collider.gameObject,
                                _soket.HareketPozisyonu);

                            _soket.Doluluk=true;

                            SeciliObje  =null;
                            SeciliSoket =null;
                            HareketVar  =true;

                            HamleHakki--;
                            HamleHakkiText.text="HAK : " + HamleHakki.ToString();

                        }else if(SeciliSoket==hit.collider.gameObject){

                            SeciliObje.GetComponent<SonFis>().HareketEt(
                                "SoketeOtur",
                                hit.collider.gameObject);
                            
                            SeciliObje  =null;
                            SeciliSoket =null;
                            HareketVar  =true;
                    
                        }
                    }
                    // ## SOKET
                }
            }
        }
    }

    public void CarpismayiKontrolEt(int CarpismaIndex,bool Durum){

        CarpismaDurumlari[CarpismaIndex]=Durum;   
    }

    public void FisleriKontrolEt(){

        foreach (var item in Fisler){

            if(item.GetComponent<SonFis>().MevcutSoket.name==item.GetComponent<SonFis>().SoketRengi){

                TamamlanmaSayisi++;

            }
        }

        if(TamamlanmaSayisi==HedefSoketSayisi){

            foreach (var item in CarpismaKontrolObjeleri){

                item.SetActive(true);
            }
            StartCoroutine(CarpismaVarmi());

        }else{

            if(HamleHakki<=0)
                Kaybettin();

        }

        TamamlanmaSayisi=0;
    }

    IEnumerator CarpismaVarmi(){

            Isiklar[0].SetActive(false);
            Isiklar[1].SetActive(true);

            KontrolPaneli.SetActive(true);
            KontrolText.text="KONTROL EDİLİYOR....";

        yield return new WaitForSeconds(3f);

        //KONTROL EDILIYOR
        foreach (var item in CarpismaDurumlari){
            
            if(item)
                CarpmaKontrolSayisi=2;


            if(CarpmaKontrolSayisi==CarpismaDurumlari.Count){
                
                Kazandin();
                KontrolPaneli.SetActive(false);
                
            }else{

                Isiklar[1].SetActive(false);
                Isiklar[0].SetActive(true);

                KontrolText.text="ÇARPIŞMA VAR";
                Invoke("PaneliKapat",2f);
                
                foreach (var item1 in CarpismaKontrolObjeleri){
                    item1.SetActive(false);
                }

                if(HamleHakki<=0)
                    Kaybettin();
            }

            CarpmaKontrolSayisi=0;
        }

    }

    void PaneliKapat(){
        KontrolPaneli.SetActive(false);
    }

    public void Butonislemleri(string Deger){

        switch(Deger){

            case "Durdur":
                Time.timeScale=0;
                Paneller[0].SetActive(true);
                break;
            
            case "DevamEt":
                Time.timeScale=1;
                Paneller[0].SetActive(false);
                break;
            
            case "Tekrar":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale=1;
                break;

            case "SonrakiLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
                Time.timeScale=1;
                break;

            case "Cikis":
                Application.Quit();
                break;
            

    }
}

    void Kazandin(){

        Isiklar[1].SetActive(false);
        Isiklar[2].SetActive(true);

        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level")+1);
        UITextleri[0].text="LEVEL  :"+SceneManager.GetActiveScene().name;
        Paneller[1].SetActive(true);
        Time.timeScale=0;

    }

    void Kaybettin(){

        UITextleri[1].text="LEVEL  : "+SceneManager.GetActiveScene().name;
        Paneller[2].SetActive(true);
        Time.timeScale=0;

    }
}
