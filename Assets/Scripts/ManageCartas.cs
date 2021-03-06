using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageCartas : MonoBehaviour
{

    public GameObject carta;            //carta a ser descartada
    private bool primeiraCartaSelecionada, segundaCartaSelecionada;     //indicadores para cada carta selecionada em cada linha
    private GameObject carta1, carta2;          //gameObjects da primeira e segunda carta selecionadas
    private string linhaCarta1, linhaCarta2;        //linha da carta selecionada

    bool timerPausado, timerAcionado;           //indicador de pause ou start no timer
    float timer;

    int numTentativas = 0;          //número de tentativas na rodada
    int numAcertos = 0;             //número de match de pares acertados 
    AudioSource somOK;              //som de acerto

    int ultimoJogo = 0;
    bool primeiroJogo = true;
    int recorde = 10000;

    // Start is called before the first frame update
    public void Start()
    {
        MostraCartas();
        UpdateTentativas();
        somOK = GetComponent<AudioSource>();
        ultimoJogo = PlayerPrefs.GetInt("Jogadas", 0);
        recorde = PlayerPrefs.GetInt("recorde", ultimoJogo);
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "Jogo anterior = " + ultimoJogo;
        GameObject.Find("recorde").GetComponent<Text>().text = "Recorde = " + recorde;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerAcionado)
        {
            timer += Time.deltaTime;
            print(timer);
            if (timer > 1)
            {
                timerPausado = true;
                timerAcionado = false;
                if (carta1.tag == carta2.tag)
                {
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    somOK.Play();
                    if (numAcertos == 26)
                    {
                        if (numTentativas < recorde || recorde == 0)
                        {
                            recorde = numTentativas;
                            PlayerPrefs.SetInt("recorde", recorde);
                            SceneManager.LoadScene("Recorde");
                        }
                        else
                        {
                            PlayerPrefs.SetInt("Jogadas", numTentativas);
                            SceneManager.LoadScene("EndGame");
                        }
                    }
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    void MostraCartas()
    {
        int[] arrayEmbaralhada = CriaArrayEmbaralhado();
        int[] arrayEmbaralhada2 = CriaArrayEmbaralhado();
        int[] arrayEmbaralhada3 = CriaArrayEmbaralhado();
        int[] arrayEmbaralhada4 = CriaArrayEmbaralhado();
        //Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        //AddUmaCarta();
        for (int i = 0; i < 13; i++)
        {
            //AddUmaCarta(i);
            //AddUmaCarta(i, arrayEmbaralhada[i]);
            AddUmaCarta(0, i, arrayEmbaralhada[i]);
            AddUmaCarta(1, i, arrayEmbaralhada2[i]);
            AddUmaCarta(2, i, arrayEmbaralhada3[i]);
            AddUmaCarta(3, i, arrayEmbaralhada4[i]);
        }
    }

    void AddUmaCarta(int linha, int rank, int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCartaOriginal = carta.transform.localScale.x;
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f;
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f;
        //Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank - 13 / 2) * 1.3f), centro.transform.position.y, centro.transform.position.z);
        //Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank - 13 / 2) * fatorEscalaX), centro.transform.position.y, centro.transform.position.z);
        Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank - 13 / 2) * fatorEscalaX), centro.transform.position.y + ((linha - 4 / 2) * fatorEscalaY), centro.transform.position.z);
        //GameObject c = (GameObject)(Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity));
        //GameObject c = (GameObject)(Instantiate(carta, new Vector3(rank*2.0f, 0, 0), Quaternion.identity));
        GameObject c = (GameObject)(Instantiate(carta, novaPosicao, Quaternion.identity));
        c.tag = "" + (valor + 1);
        //c.name = "" + valor;
        c.name = "" + linha + "_" + valor;
        string nomeDaCarta = "";
        string numeroCarta = "";
        /*switch (rank)
        {
            case 0:
                numeroCarta = "ace";
                break;
            case 10:
                numeroCarta = "jack";
                break;
            case 11:
                numeroCarta = "queen";
                break;
            case 12:
                numeroCarta = "king";
                break;
            default:
                numeroCarta = "" + (rank + 1);
                break;
        }*/                                         //switch case para array ordenada do deck
        switch (valor)
        {
            case 0:
                numeroCarta = "ace";
                break;
            case 10:
                numeroCarta = "jack";
                break;
            case 11:
                numeroCarta = "queen";
                break;
            case 12:
                numeroCarta = "king";
                break;
            default:
                numeroCarta = "" + (valor + 1);
                break;
        }
        switch (linha)
        {
            case 0:
                nomeDaCarta = numeroCarta + "_of_diamonds";
                break;
            case 1:
                nomeDaCarta = numeroCarta + "_of_clubs";
                break;
            case 2:
                nomeDaCarta = numeroCarta + "_of_hearts";
                break;
            case 3:
                nomeDaCarta = numeroCarta + "_of_spades";
                break;
        }        
        Sprite s1 = (Sprite)Resources.Load<Sprite>(nomeDaCarta);
        print("S1: " + s1);
        //GameObject.Find("" + rank).GetComponent<Tile>().SetCartaOriginal(s1);
        //GameObject.Find("" + valor).GetComponent<Tile>().SetCartaOriginal(s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().SetCartaOriginal(s1);
    }

    public int[] CriaArrayEmbaralhado()
    {
        int[] novoArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int temp;
        for (int t = 0; t < 13; t++)
        {
            temp = novoArray[t];
            int r = Random.Range(t, 13);
            novoArray[t] = novoArray[r];
            novoArray[r] = temp;
        }
        return novoArray;
    }

    public void CartaSelecionada(GameObject carta)
    {
        if (!primeiraCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta1 = linha;
            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)
        {
            string linha = carta.name.Substring(0, 1);
            linhaCarta2 = linha;
            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();
            VerificaCartas();
        }
    }

    public void VerificaCartas()
    {
        DisparaTimer();
        numTentativas++;
        UpdateTentativas();
    }

    public void DisparaTimer()
    {
        timerPausado = false;
        timerAcionado = true;
    }

    void UpdateTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
    }
}
