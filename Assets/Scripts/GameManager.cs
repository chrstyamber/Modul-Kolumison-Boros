using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Pengaturan Halaman Menu")]
    public string namaFileData = "soal.json";
    public string namaSceneHalamanMenu  = "Menu";
    public string namaSceneHalamanKuis  = "Kuis";
    public string namaSceneHalamanHasil = "Hasil";

    [Header("Pilih Algoritma")]
    [Tooltip("Checklist untuk mengacak soal, dan jangan dichecklist jika ingin soal tampil secara berurutan")]
    public bool acakSoal;


    [Header("Pengaturan Halaman Kuis")]
    public Text textSoal;
    public Text text_A;
    public Text text_B;
    public Text text_C;
    public Text text_D;
    public Button button_A;
    public Button button_B;
    public Button button_C;
    public Button button_D;
    public RawImage gambar;

    public AudioSource suaraBenar;
    public AudioSource suaraSalah;
    public AudioSource suara;

    public Texture2D[] dataGambar;
    public AudioClip[] dataSuara;
    public int nilai;

    [Header("Gambar")]
    [Tooltip("Checklist untuk Menggunakan soal ber-Gambar, dan jangan dichecklist jika ingin soal tanpa gambar")]
    public bool gunakanGambar;

    [Header("Pengaturan Halaman Hasil")]
    public GameObject[] bintang;
    public int batas_bintang_1;
    public int batas_bintang_2;
    public int batas_bintang_3;
    public Text text_nilai;


    string url;
    string currentScene;
    string jawabanBenar;
    int bobot;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        nilai = PlayerPrefs.GetInt("nilai");

        if (currentScene == namaSceneHalamanMenu)
        {
            PlayerPrefs.SetString("halaman_menu", namaSceneHalamanMenu);
            PlayerPrefs.SetString("halaman_kuis", namaSceneHalamanKuis);
            PlayerPrefs.SetString("halaman_hasil", namaSceneHalamanHasil);
            PlayerPrefs.SetInt("step", 0);
            PlayerPrefs.SetInt("nilai", 0);
        }
      

        if (currentScene == PlayerPrefs.GetString("halaman_hasil"))
        {
            SettingBintangBerdasarkanNilai();
        }

        url = Application.streamingAssetsPath + "/" + namaFileData;
        StartCoroutine(GetData());
    }

    public void SettingBintangBerdasarkanNilai()
    {
        if (nilai <= batas_bintang_1)
        {
            bintang[0].SetActive(true);
            bintang[1].SetActive(false);
            bintang[2].SetActive(false);
        }
        else if (nilai <= batas_bintang_2)
        {
            bintang[0].SetActive(true);
            bintang[1].SetActive(true);
            bintang[2].SetActive(false);
        }
        else if (nilai <= batas_bintang_3)
        {
            bintang[0].SetActive(true);
            bintang[1].SetActive(true);
            bintang[2].SetActive(true);
        }

        text_nilai.text = "Nilai: " + nilai.ToString();
    }
   


    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if(currentScene != PlayerPrefs.GetString("halaman_menu"))
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("halaman_menu"));
            }
        }
    }


    IEnumerator GetData()
    {
        using (UnityWebRequest webData = UnityWebRequest.Get(url))
        {
            yield return webData.SendWebRequest();
            if(webData.isNetworkError || webData.isHttpError)
            {
                Debug.Log("ada error di url");
            }
            else
            {
                if (webData.isDone)
                {
                    JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(webData.downloadHandler.data));
                    if (currentScene == namaSceneHalamanMenu)
                    {

                        int total_soal = jsonData["data"].Count;
                        PlayerPrefs.SetInt("total_soal", total_soal);
                        PlayerPrefs.SetInt("step", 0);
                        if (acakSoal)
                        {
                            AcakSoal(total_soal);
                        }
                        else
                        {
                            TampilkanBerurutan(total_soal);
                        }
                        
                    }
                    else if(currentScene == PlayerPrefs.GetString("halaman_kuis"))
                    {
                        // proses pengisian data dari json, ke masing-masing variable dan gameobject di unity //

                        int step        = PlayerPrefs.GetInt("step");
                        int soal        = PlayerPrefs.GetInt("SOAL_" + step);
                       
                        textSoal.text   = jsonData["data"][soal]["soal"];
                        text_A.text     = jsonData["data"][soal]["a"];
                        text_B.text     = jsonData["data"][soal]["b"];
                        text_C.text     = jsonData["data"][soal]["c"];
                        text_D.text     = jsonData["data"][soal]["d"];
                        jawabanBenar    = jsonData["data"][soal]["jawaban_benar"];

                        if (gunakanGambar)
                        {
                            gambar.texture = dataGambar[soal];
                        }


                        // ini adalah proses konversi data dari string ke integer
                        int.TryParse(jsonData["data"][soal]["bobot"], out int int_bobot);

                        bobot           = int_bobot;
                    }
                }
            }
        }
    }


    public void Jawaban_User(string jawaban)
    {
        DisableButton();
        StartCoroutine(Cek_Jawaban(jawaban));
    }

    IEnumerator Cek_Jawaban(string jawaban)
    {

        if (jawabanBenar == jawaban)
        {
            int nilai = PlayerPrefs.GetInt("nilai");
            nilai     = nilai + bobot;
            PlayerPrefs.SetInt("nilai", nilai);
            suaraBenar.Play();
        }
        else
        {
            suaraSalah.Play();
        }
    
        yield return new WaitForSeconds(1f);

        int step = PlayerPrefs.GetInt("step");
        step = step + 1;
        PlayerPrefs.SetInt("step", step);

        if (PlayerPrefs.GetInt("step") < PlayerPrefs.GetInt("total_soal"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("halaman_kuis"));
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("halaman_hasil"));
        }
    }

    public void PindahHalaman(string halaman)
    {
        SceneManager.LoadScene(halaman);
    }

    public void Buka_Popup(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    public void Tutup_Popup(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void Nyalakan_Suara()
    {
        int step = PlayerPrefs.GetInt("step");
        int soal = PlayerPrefs.GetInt("SOAL_" + step);
        suara.clip = dataSuara[PlayerPrefs.GetInt("SOAL_" + step)];
        suara.Play();
    }

    public void Keluar_Aplikasi()
    {
        Application.Quit();
    }


    public void DisableButton()
    {
        button_A.enabled = false;
        button_B.enabled = false;
        button_C.enabled = false;
        button_D.enabled = false;
    }


    // PROSES PENGACAKAN SOAL, SUPAYA SOAL TIDAK ADA YANG SAMA URUTANYA //

    void AcakSoal(int total_soal)
    {
        List<int> ints = new List<int>();
        List<int> values = new List<int>();

        for (int i = 0; i < total_soal; ++i)
        {
            ints.Add(i);
        }

        for (int i = 0; i < total_soal; ++i)
        {
            int index = Random.Range(0, ints.Count);
            values.Add(ints[index]);
            ints.RemoveAt(index);
        }

        for (int i = 0; i < values.Count; ++i)
        {
            PlayerPrefs.SetInt("SOAL_" + i, values[i]);
            Debug.Log("SOAL_" + i + " : " + values[i]);
        }
    }

    // PROSES SETTING SUPAYA SOAL TAMPIL BERURUTAN //

    void TampilkanBerurutan(int total_soal)
    {
        for (int i = 0; i < total_soal; ++i)
        {
            PlayerPrefs.SetInt("SOAL_" + i, i);
        }
    }

    public void MainLagi()
    {
        PlayerPrefs.SetInt("step", 0);
        PlayerPrefs.SetInt("nilai", 0);
        SceneManager.LoadScene(PlayerPrefs.GetString("halaman_kuis"));
    }

}

/*Created By CIKARA STUDIO */
/* TIDAK UNTUK DIPERJUALBELIKAN */
/* TIDAK UNTUK DIPERJUALBELIKAN */
