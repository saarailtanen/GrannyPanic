#define SENSOR_A 32
#define SENSOR_B 33

volatile unsigned long pulseA = 0;
volatile unsigned long pulseB = 0;

// 1 = ETEEN, -1 = TAAKSE, 0 = ei vielä määritetty
volatile int8_t direction = 0;

// 1 = A, 2 = B, 0 = ei vielä
volatile int8_t lastSensor = 0;

void IRAM_ATTR onA() {
  pulseA++;
  if (lastSensor == 1) {
    direction = -direction; // sama anturi kahdesti peräkkäin → suunta vaihtuu
  } else if (lastSensor == 0) {
    direction = 1; // ensimmäinen pulssi → oletetaan ETEEN
  }
  lastSensor = 1;
}

void IRAM_ATTR onB() {
  pulseB++;
  if (lastSensor == 2) {
    direction = -direction; // sama anturi kahdesti peräkkäin → suunta vaihtuu
  } else if (lastSensor == 0) {
    direction = 1; // ensimmäinen pulssi → oletetaan ETEEN
  }
  lastSensor = 2;
}

void setup() {
  Serial.begin(115200);
  pinMode(SENSOR_A, INPUT_PULLUP);
  pinMode(SENSOR_B, INPUT_PULLUP);

  attachInterrupt(digitalPinToInterrupt(SENSOR_A), onA, RISING);
  attachInterrupt(digitalPinToInterrupt(SENSOR_B), onB, RISING);

  Serial.println("Suuntatunnistus: A-B vuorottelu, suunta vaihtuu kun sama anturi peräkkäin");
}

void loop() {
  static unsigned long lastPrintedA = 0;
  static unsigned long lastPrintedB = 0;
  static int8_t lastPrintedDir = 0;

  noInterrupts();
  unsigned long a = pulseA;
  unsigned long b = pulseB;
  int8_t dir = direction;
  interrupts();

  if (a != lastPrintedA) {
    Serial.printf("Pulssi havaittu: A  total=%lu Δ=%lu\n", a, a - lastPrintedA);
    lastPrintedA = a;
  }

  if (b != lastPrintedB) {
    Serial.printf("Pulssi havaittu: B  total=%lu Δ=%lu\n", b, b - lastPrintedB);
    lastPrintedB = b;
  }

  if (dir != lastPrintedDir) {
    if (dir == 1) Serial.println("Suunta: ETEEN");
    else if (dir == -1) Serial.println("Suunta: TAAKSE");
    lastPrintedDir = dir;
  }

  delay(20);
}
