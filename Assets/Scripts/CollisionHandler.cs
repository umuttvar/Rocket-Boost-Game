using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip finishSFX;
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] ParticleSystem crashParticles;


    AudioSource audioSource;

    bool isControllable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKey();
    }

    private void RespondToDebugKey()
    {
        if (Keyboard.current.lKey.isPressed)
        {
            LoadNextLevel();
            
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //eğer isControllable parametresi true ise !isControllable ile false dönecek ve if statement içine girmeyip switchten devam edecek.
        //eğer isControllable parametresi false ise !isControllable ile true dönecek ve if statement içine girip return ile ilgili metottan (OnCollisionEnter) çıkacak. 
        //bu sayede ses ve kontroller düzenli olacak. bunun öncesinde bir yere çarptığımızda ikinci kez çarptığımızda yine StartCrashSequence metodu çalıştığı için tekrar ses çıkıyordu, StartCrashSequence içerisinde isControllable'ı false olarak işaretlediğimiz için buradaki if bloğuna girecek ve return ile OnCollisionEnter metodundan çıkacak ve bir daha ses gelmeyecek.
        if (!isControllable) { return; } 

        string tagCollision = other.gameObject.tag;

        switch (tagCollision)
        {
            case "Friendly":
                Debug.Log("Start The Game!");
                break;
                
            case "Finish":
                StartSucessSequence();
                break;

            default:
                Debug.Log("You Are Crashed!");
                StartCrashSequence();
                break;
        }
    }

    private void StartSucessSequence()
    {
        //OnCollisonEnter metodunda kullandığımız if statament'ı için yazdık.
        //bu metot çağırıldığında isControllable parametresi false olarak işaretlenecek ve return sayesinde tekrardan OnCollision metodu tekrardan çalışmayacak.
        isControllable = false;
        //Tüm sesleri kapatmak için yazdık(mainEngine dahil çünkü direkt olarak AudioSource componentını durdurduk.)
        audioSource.Stop();

        finishParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
        audioSource.PlayOneShot(finishSFX);
    }

    private void StartCrashSequence()
    {
        //OnCollision metodunun kullanılıp kullanılmayacağını belirlemek için yazıldı.
        isControllable = false;
        audioSource.Stop();

        //Oyuncu bir yere çarptığı zaman kontrolleri kullanamaması gerektiği için Movement Script'ini disable ediyoruz.
        //Movement ve CollisionHandler scriptleri aynı gameObject içinde oldukları için GetComponent<> kullanarak Movement scriptinin referansını alıp enabled propertysini false olarak işaretlersek Movement Script'i kapanır, kullanıcı kontrolleri kullanamaz.
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();


        //Invoke metodu gecikme için kullanılır. Burada ReloadLevel isimli metodun 2 saniye gecikmeyle çalışması için kullanıyoruz.
        //Çarptığımızda direkt olarak yeni level'e geçiyor ve bu da kötü gözüküyor bu yüzden Invoke kullanıyoruz.
        //levelLoadDelay yukarıda oluşturduğumuz property değeri.
        Invoke("ReloadLevel", levelLoadDelay);

    }

    void LoadNextLevel ()
        {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        //Aşağıda bahsettiğimiz gibi sahnelerimin bulunduğu liste buildSettings içerisindedir. scemeCountInBuildSettings propertysi listemiz içerisinde kaç tane elemen olduğunu söyler. 
        //eğer sonraki sahnemiz listemizdeki eleman sayısına eşit olursa hata verir ve oyun buga girer bu yüzden eşit olduğunda başa dönmesini sağlıyoruz.
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

            SceneManager.LoadScene(nextScene);
        }

    void ReloadLevel ()
        {
            //SceneManager sınıfından LoadScene metodu Unity üzerindeki SceneList üzerinden seçilir.(File ==> Build Profiles  =>> Build Settings içerisinde listelerimiz bulunur.) 
            //Tipik programlama sıralanması gibi eklenen sahneler index numarasına göre eklenir. 
            //LoadScene hem int hem string değer de alabilir yani sahne adını yazarak da kullanabiliriz.

            //SceneManager.GetActiveScene() metou bize scene döndürür. bildIndex property'si ile bulunan scene için index değerine ulaşırız.
                int currentScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentScene);
        }
}
