#include <WiFi.h>
#include <WebSocketServer.h>
#include <ArduinoJson.h>

#define oikeaAnturi_PIN 33
#define kytkinEteen_PIN 32
#define nappi_PIN 34 


#define WIFI_YHTEYS "IOTLABRA"
#define WIFI_SALASANA "iotlabra2020"
#define PORTTI 80
WiFiServer serveri(PORTTI);
WebSocketServer sokettiServeri;

String lahetettavaData;

void setup() {
Serial.begin(115200);

pinMode(oikeaAnturi_PIN, INPUT_PULLDOWN);
pinMode(nappi_PIN, INPUT_PULLDOWN);
pinMode(kytkinEteen_PIN, INPUT_PULLDOWN);


if( yhteydenotto() == 0 ){
  Serial.println("Yhteyttä ei saatu");
  return;
}
}

int yhteydenotto(){
  WiFi.begin(WIFI_YHTEYS, WIFI_SALASANA);
  Serial.println("Odotellaan wifiä");
  delay(5000);
  if (WiFi.status() == WL_CONNECTED)
  {
    Serial.println("Wifi yhteys yhdistetty");
    Serial.print(WiFi.localIP());
    return 1;
  }
  else
  {
    Serial.println("Wifi yhteyttä ei saatu");
    return 0;
  }
}

void loop() {

  Serial.println("Käynnistetään websocket serveri");

  serveri.begin();
  delay(1000);
  Serial.println("Serveri on käynnissä");

  while(true)
  {
    WiFiClient kuuntelija = serveri.available();
    if ( kuuntelija.connected() && sokettiServeri.handshake(kuuntelija))
    {
      Serial.println("Kuuntelija otti yhteyttä");
      String tieto;
      while(kuuntelija.connected())
      {
        // Luetaan tietoa soketista
        tieto = sokettiServeri.getData();
        if ( tieto.length() > 0)
        {
          Serial.println(tieto);
        }

        StaticJsonDocument<250> jsondokkari;

        int oikeaAnturi = analogRead(oikeaAnturi_PIN);
        int nappi = analogRead(nappi_PIN);
        int kytkinE = digitalRead(kytkinEteen_PIN);


        jsondokkari["oikeaAnturi1"] = String(oikeaAnturi, DEC);
        jsondokkari["nappi1"] = String(nappi);
        jsondokkari["kytkinE1"] = String(kytkinE);





        String lahetettavaData;
        serializeJson(jsondokkari, lahetettavaData);
        sokettiServeri.sendData(lahetettavaData);
        delay(50);
      }
    }
  }
}
