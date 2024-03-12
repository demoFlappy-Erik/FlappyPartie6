using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public float vitesseX;
    public float vitesseY;
    public GameObject laPiece;
    public GameObject PackVie;
    public GameObject Champignon;
    public GameObject Decor;
    public GameObject elementGrille;
    public Sprite flappyBlesse;
    public Sprite flappyNormal;
    public Sprite flappyNormalAile;
    public Sprite flappyBlesseAile;

    public AudioClip sonColonne;
    public AudioClip sonOr;
    public AudioClip sonPack;
    public AudioClip sonChampignon;
    public AudioClip sonFinPartie;

    Boolean siFlappyBlesse = false;
    Boolean partieTerminee = false;

    public TextMeshProUGUI CompteurText;
    public TextMeshProUGUI FinJeuText;
    int compteur = 0;

    // Update is called once per frame
    void Update()
    {
        if (partieTerminee == false)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                vitesseX = 0.02f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                vitesseX = -0.02f;
            }
            else
            {
                vitesseX = GetComponent<Rigidbody2D>().velocity.x;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                vitesseY = 5;

                if (siFlappyBlesse==false)
                {
                    GetComponent<SpriteRenderer>().sprite = flappyNormalAile;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = flappyBlesseAile;
                }
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
            
                if (siFlappyBlesse == false)
                {
                    GetComponent<SpriteRenderer>().sprite = flappyNormal;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = flappyBlesse;
                }
            }
            else
            {
                vitesseY = GetComponent<Rigidbody2D>().velocity.y;
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
        }
    }
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        if (infoCollision.gameObject.name == "ColonneHaut" || infoCollision.gameObject.name == "ColonneBas" || infoCollision.gameObject.name =="Decor")
        {
            compteur -= 5;
            CompteurText.text = compteur.ToString();

            GetComponent<AudioSource>().PlayOneShot(sonColonne, 2f);

            if (siFlappyBlesse==false)
            {
                GetComponent<SpriteRenderer>().sprite = flappyBlesse;
                siFlappyBlesse = true;
                
            }
            else
            {
                partieTerminee = true;
                GetComponent<Rigidbody2D>().freezeRotation = false;
                GetComponent<Rigidbody2D>().angularVelocity = 720f;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AudioSource>().PlayOneShot(sonFinPartie);
                FinJeuText.gameObject.SetActive(true);

                Invoke("Rejouer", 4f);
            }

        }
        else if(infoCollision.gameObject.name == "PieceOr")
        {
            elementGrille.GetComponent<Animator>().enabled = true;
            compteur += 5;
            CompteurText.text = compteur.ToString();

            GetComponent<AudioSource>().PlayOneShot(sonOr, 2f);

            infoCollision.gameObject.SetActive(false);

            Invoke("ReactiverPiece", 3f);
            Invoke("ReactiverAnimationGrille", 4f);
        }
        else if (infoCollision.gameObject.name == "PackVie")
        {
            compteur += 5;
            CompteurText.text = compteur.ToString(); 

            siFlappyBlesse =false;

            GetComponent<AudioSource>().PlayOneShot(sonPack, 2f);

            GetComponent<SpriteRenderer>().sprite = flappyNormal;


            infoCollision.gameObject.SetActive(false);

            Invoke("ReactiverPackVie", 3f);
        }
        else if(infoCollision.gameObject.name == "Champignon")
        {
            compteur += 10;
            CompteurText.text = compteur.ToString();

            GetComponent<AudioSource>().PlayOneShot(sonChampignon, 2f);

            infoCollision.gameObject.SetActive(false);

            gameObject.transform.localScale *= 1.3f;

            Invoke("ReactiverChampignon", 7f);
        }
    }
    void ReactiverPiece()
    {
        laPiece.SetActive(true);
        laPiece.transform.position = new Vector2(laPiece.transform.position.x, UnityEngine.Random.Range(-1,2));
    }

    void ReactiverPackVie()
    {
        PackVie.SetActive(true);
    }
    void ReactiverChampignon()
    {
        gameObject.transform.localScale /= 1.3f;
        Champignon.SetActive(true);
    }

    void Rejouer()
    {
        SceneManager.LoadScene("flappy6");
    }

    void ReactiverAnimationGrille()
    {
        elementGrille.GetComponent<Animator>().enabled = false;
    }
}
