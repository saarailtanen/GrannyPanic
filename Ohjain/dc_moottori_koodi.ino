int left;
int right;
int diff;
int total = 0;

// Choose two ADC-capable pins on ESP32 (e.g., GPIO32 and GPIO33)
const int pin1 = 32;  // Connect to one motor terminal
const int pin2 = 33;  // Connect to the other motor terminal

void setup() {
  Serial.begin(115200);
}

void loop() {
  // First read (pin2 HIGH, pin1 as analog input)
  pinMode(pin1, INPUT);
  pinMode(pin2, OUTPUT);
  digitalWrite(pin2, HIGH);
  delayMicroseconds(10);
  left = analogRead(pin1);

  // Second read (pin1 HIGH, pin2 as analog input)
  pinMode(pin2, INPUT);
  pinMode(pin1, OUTPUT);
  digitalWrite(pin1, HIGH);
  delayMicroseconds(10);
  right = analogRead(pin2);

  // Calculate difference
  diff = left - right;
  total += diff;

  // Determine direction
  String direction = "Stopped";
  if (diff > 10) {
    direction = "Clockwise";
  } else if (diff < -10) {
    direction = "Counter-Clockwise";
  }

  // Print data
  Serial.print("Left: ");
  Serial.print(left);
  Serial.print("  Right: ");
  Serial.print(right);
  Serial.print("  Diff: ");
  Serial.print(diff);
  Serial.print("  Total: ");
  Serial.print(total);
  Serial.print("  Direction: ");
  Serial.println(direction);

  delay(100);
}