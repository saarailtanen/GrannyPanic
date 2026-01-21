#include <WiFi.h>
#include <WebSocketServer.h>
#include <ArduinoJson.h>

#define oikeaAnturi_PIN 33
#define kytkinEteen_PIN 32
#define nappi_PIN 14 


#define WIFI_YHTEYS "IOTLABRA"
#define WIFI_SALASANA "iotlabra2020"
#define PORTTI 80
WiFiServer serveri(PORTTI);
WebSocketServer sokettiServeri;

volatile unsigned int pulseCount = 0;
unsigned long lastTime = 0;
unsigned long currentTime = 0;
float rpm = 0;
float smoothedRPM = 0;

String lahetettavaData;

const unsigned long measurementInterval = 100; // update every 50 ms


//tää testikohta
const int BUFFER_SIZE = 2;
float rpmBuffer[BUFFER_SIZE] = {0};
int bufferIndex = 0;

void addToRPMBuffer(float newValue) {
    rpmBuffer[bufferIndex] = newValue;
    bufferIndex = (bufferIndex + 1) % BUFFER_SIZE;
}

float getAverageRPM() {
    float sum = 0;
    for (int i = 0; i < BUFFER_SIZE; i++) {
        sum += rpmBuffer[i];
    }
    return sum / BUFFER_SIZE;
}
//loppuu tähän


void IRAM_ATTR countPulse() {
  pulseCount++;
}

void setup() {
Serial.begin(115200);

pinMode(oikeaAnturi_PIN, INPUT_PULLUP);
pinMode(nappi_PIN, INPUT_PULLUP);
pinMode(kytkinEteen_PIN, INPUT_PULLUP);

attachInterrupt(digitalPinToInterrupt(oikeaAnturi_PIN), countPulse, FALLING); // Trigger on falling edge
lastTime = millis();



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

          unsigned long currentTime = millis();
          if (currentTime - lastTime >= measurementInterval) {
            float intervalSeconds = (currentTime - lastTime) / 1000.0; // convert ms to s
            rpm = (pulseCount * 60.0) / intervalSeconds;
            //Serial.print("RPM: ");
            //Serial.println(rpm);
            //testi
            addToRPMBuffer(rpm);
            smoothedRPM = getAverageRPM();
            //Serial.println(smoothedRPM);
            //loppuu tähän
            pulseCount = 0; // Reset pulse count
            lastTime = currentTime;
          }

        //int new_rpm1 = rpm / 9;
        int new_rpm1 = smoothedRPM / 9;
        Serial.println(new_rpm1);

        int nappi = digitalRead(nappi_PIN);
        int kytkinE = digitalRead(kytkinEteen_PIN);


        jsondokkari["oikeaAnturi1"] = String(new_rpm1);
        jsondokkari["nappi1"] = String(nappi);
        jsondokkari["kytkinE1"] = String(kytkinE);


        String lahetettavaData;
        serializeJson(jsondokkari, lahetettavaData);
        sokettiServeri.sendData(lahetettavaData);
        //tähän delay?
      }
    }
    delay(100);
  }
}
