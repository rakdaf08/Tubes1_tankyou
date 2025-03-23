using System;
using System.Drawing; // Library untuk warna
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class KuncirRamBOT : Bot
{
  static void Main(string[] args)
  {
    new KuncirRamBOT().Start();
  }
  int arahSenjata = 1; // Variabel untuk memutar senjata
  bool scanning = false; // Kondisi saat scanning
  bool maju; // Kondisi saat bot maju
  bool kenceng; // Kondisi saat bot dalam kecepatan tinggi

  KuncirRamBOT() : base(BotInfo.FromFile("KuncirRamBOT.json")) { }

  public override void Run()
  {
    BodyColor = Color.White;
    TurretColor = Color.Black;
    RadarColor = Color.Red;
    BulletColor = Color.Black;
    ScanColor = Color.Red;

    while (IsRunning)
    {
      AdjustGunForBodyTurn = false; // Senjata tidak bergerak mengikuti badan
      maju = true; // Bot bergerak maju
      kenceng = false; // Bot bergerak dengan kecepatan normal
      GunTurnRate = 15; // Kecepatan putar senjata

      // Jika energi bot lebih dari 10, kecepatan bot normal dan pergerakan jiggle jiggle
      if (Energy > 10)
      {
        MaxSpeed = 6;
        SetForward(4000);
        SetTurnRight(45);
        SetTurnLeft(45);
      }
      else // Jika energi bot kurang dari 10, kecepatan bot tinggi dan pergerakan berputar untuk mengulur waktu
      {
        MaxSpeed = 8;
        SetTurnLeft(10_000);
        SetForward(10_000);
        kenceng = true;
      }
      SetTurnGunRight(360); // Senjata berputar 360 derajat setiap rondenya
      Go();
    }
  }

  public override void OnScannedBot(ScannedBotEvent e)
  {
    base.OnScannedBot(e);
    scanning = true; // Bot sedang melakukan scanning
    arahSenjata = -arahSenjata; // Arah senjata berputar terus menerus

    AdjustGunForBodyTurn = true; // Senjata bergerak mengikuti badan
    double distance = DistanceTo(e.X, e.Y);  // Variabel untuk mengetahui jarak musuh
    SetTurnGunTowards(e.X, e.Y);  // Mengarahkan senjata ke musuh

    // Jika energi lebih dari 10, bot menembak musuh sesuai jarak dan sambil jiggle jiggle
    if (Energy > 10)
    {
      SetForward(4000);
      if (distance < 75)
      {
        Fire(3);
      }
      else if (distance < 100 && distance > 75)
      {
        Fire(2.8);
      }
      else if (distance < 150 && distance > 100)
      {
        SetTurnRight(45);
        Fire(2.5);
      }
      else if (distance < 250 && distance > 45)
      {
        SetTurnLeft(45);
        Fire(2.3);
      }
      else if (distance < 300 && distance > 250)
      {
        SetTurnRight(45);
        Fire(2);
      }
      else // Jika jarak sangat jauh, bot akan bergerak sesuai jarak ke musuh mod 3 dan menembak musuh dengan peluru kecil
      {
        double jarakbaru = distance % 3;
        if (jarakbaru == 0)
        {
          SetTurnRight(45);
          SetForward(4000);
        }
        else if (jarakbaru == 1)
        {
          SetTurnLeft(45);
          SetForward(4000);
        }
        else
        {
          SetBack(4000);
        }
        Fire(1.5);
      }
    }
    else  // Jika energi kurang dari 10, bot tidak menembak musuh namun bergerak berputar dengan kecepatan tinggi
    {
      MaxSpeed = 8;
      SetTurnLeft(10_000);
      SetForward(10_000);
      kenceng = true;
    }
    SetTurnGunRight(360 * arahSenjata); // Senjata berputar 360 derajat setiap rondenya dan dapat bolak balik karena variable arahSenjata
    Go();
  }

  public override void OnHitBot(HitBotEvent e) // Jika bot menabrak atau ditabrak bot lain
  {
    base.OnHitBot(e);

    // Bot akan memutar senjata ke posisi bot yang ditabrak atau menabrak
    SetTurnGunTowards(e.X, e.Y);
    arahSenjata = -arahSenjata;
    SetTurnGunRight(360 * arahSenjata);

    // Jika bot kita yang menabrak bot lain maka akan maju dan menembak musuh dengan kekuatan maksimal
    if (e.IsRammed)
    {
      MaxSpeed = 8;
      SetForward(300);
      SetTurnLeft(115);
      SetFire(3);
      kenceng = true;
    }
    else // Jika bot kita yang ditabrak bot lain maka akan mundur dan berbelok untuk lari dengan kecepatan tinggi
    {
      MaxSpeed = 8;
      SetBack(100);
      SetTurnRight(50);
      kenceng = true;
    }
    Go();
  }

  public override void OnHitWall(HitWallEvent e) // Bila bot menabrak dinding
  {
    base.OnHitWall(e);
    // Jika bot dalam posisi maju maka bot akan mundur untuk efisiensi pergerakan tanpa berputar
    if (maju)
    {
      SetBack(4000);
      maju = false;
    }
    else // Jika bot dalam posisi mundur, maka bot akan maju kembali
    {
      SetForward(4000);
      maju = true;
    }
    SetTurnLeft(115); // Untuk menghindari bot menabrak dinding lagi diantara dua corner
    if (!scanning) // Jika bot sedang tidak sedang melakukan scanning, maka bot melakukan scan 360 derajat untuk mencari bot musuh
    {
      SetTurnGunRight(360);
    }
    Go();
  }

  private void SetTurnGunTowards(double targetX, double targetY) // Fungsi untuk memutar senjata ke arah musuh
  {
    double bearing = BearingTo(targetX, targetY); // Menghitung sudut arah musuh
    double absoluteBearing = Direction + bearing; // Menghitung sudut absolut arah musuh
    double gunTurn = absoluteBearing - GunDirection; // Menghitung sudut putar senjata
    SetTurnGunRight(NormalizeBearing(gunTurn)); // Memutar senjata
  }

  private double NormalizeBearing(double angle) // Normalisasi sudut saat memutar senjata agar lebih efisien
  {
    while (angle > 180) angle -= 360;
    while (angle < -180) angle += 360;
    return angle;
  }
}
