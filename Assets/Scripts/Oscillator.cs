using UnityEngine;

public class Oscillator : MonoBehaviour
{
    //Bu script objenin yukarı aşağı yönlendirilmesi için yapıldı.

    //hareketin yönünü, hızını ve kaç saniye hareket edeceğini bilmek için movementVector.
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;

    Vector3 startPositon;
    Vector3 endPosition;
    
   //movementFactor değeri 0 ile 1 arasında değişecek değer olacak
    float movementFactor;

    void Start()
    {
        //objenin başlangıç pozisyonunu belirliyoruz.
        startPositon = transform.position;

        //objenin gidebileceği son konumunu belirliyoruz. 
        //movementVector (unity üzerinde 0, -8, 0 olarak ayarladık yani y ekseninde aşağı doğru hareket edecek.)
        endPosition = startPositon + movementVector;

    }

    void Update()
    {
        //movementFactor 0 ile 1 arasında değişen değer. PingPong() metodu 0 ile length(biz burada 1f verdik 5f verseydik 5 olacaktı) arasında gidip gelmesi için kullanıyoruz. 
        //Time.time metodu Unity için zaman metodudur. Oyun başladığı an 0, 1. saniyesinde 1, 20. saniyede 20 olur. 

        movementFactor = Mathf.PingPong(Time.time * speed, 1f);

        //Lerp metodu iki nokta arasında bir değer alır. 
        //Biz burada movementFactor'e 0 ile 1 arasında git dediğimiz için Lerp metodu 0 ile 1 arasında değer alır ve objenin alacağı pozisyonu da bu şekilde oluşturur.
        
        transform.position = Vector3.Lerp(startPositon, endPosition, movementFactor);
        
    }

}
