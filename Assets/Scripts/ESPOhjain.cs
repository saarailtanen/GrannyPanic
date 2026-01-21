using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

// Tämä on oikean puolen ESP koodi

public class EspOhjain : MonoBehaviour
{
    [SerializeField]
    string serverilleMenevaViesti = "heippa";

/*
    int oikeaAnturi;

    int nappi;

    int kytkinE;
*/


    public ESPDatat espData = new ESPDatat();

    CancellationTokenSource yhteysPoletti;

    void Start()
    {
        TaskinKaynnistys();
    }

    void TaskinKaynnistys()
    {
        Debug.Log("taskin käynnistys on käynnistetty");
        var sokettiYhteys = Task.Run(() => ESPYhteys());
    }

    async Task ESPYhteys()
    {
        Debug.Log("ESPyhteys käynnistetty");

        using (ClientWebSocket soketti = new ClientWebSocket())
        {
            //muistakaa vaihtaa ip osoite wifin mukaan
            Uri osoite = new Uri("");
            yhteysPoletti = new CancellationTokenSource();
            //yhteysPoletti.CancelAfter(20000);
            await soketti.ConnectAsync(osoite, yhteysPoletti.Token);
            Debug.Log("Soketti yhteys ESP32 luotu");

            while (soketti.State == WebSocketState.Open && yhteysPoletti.IsCancellationRequested == false)
            {
                //viesti arduinon suuntaan
                if (serverilleMenevaViesti.Length > 0)
                {
                    ArraySegment<byte> viesti = new ArraySegment<byte>(Encoding.UTF8.GetBytes(serverilleMenevaViesti));
                    await soketti.SendAsync(viesti, WebSocketMessageType.Text, true, yhteysPoletti.Token);
                    serverilleMenevaViesti = "";
                }

                //viestin vastaanotto
                var vastaanOtto = new byte[250];
                int offset = 0;
                int datayhdessapaketissa = 10;
                while (yhteysPoletti.IsCancellationRequested == false)
                {
                    ArraySegment<byte> vastaanotettavaviesti = new ArraySegment<byte>(
                        vastaanOtto, offset, datayhdessapaketissa);
                    WebSocketReceiveResult tulokset = await soketti.ReceiveAsync(vastaanotettavaviesti, yhteysPoletti.Token);
                    offset = offset + tulokset.Count;
                    if (tulokset.EndOfMessage)
                    {
                        break;
                    }
                }
                string vastaanottettuviesti = Encoding.UTF8.GetString(vastaanOtto, 0, offset);
                //Debug.Log("Vastaanotettiin viesti: " + vastaanottettuviesti);
                espData = JsonUtility.FromJson<ESPDatat>(vastaanottettuviesti);
            }
        }
        yhteysPoletti.Dispose();

    }

    void OnDisable()
    {
        Debug.Log("OnDisable; postetaan thread");
        if (yhteysPoletti != null) yhteysPoletti.Cancel();
    }
}

