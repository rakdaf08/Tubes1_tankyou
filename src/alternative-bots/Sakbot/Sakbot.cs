using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Sakbot : Bot
{
    private double enemyX, enemyY; // posisi musuh yang di scan
    private bool enemyDetected = false;
    private int lastSeenTurn = 0; // turn terakhir dimana musuh terlihat

    private const int SAFE_DISTANCE = 50; // jarak max terdekat dari musuh untuk gerak
    private const int FIRE_DISTANCE = 150; // jarak max terjauh dari musuh untuk tembak
    private const int RADAR_SWEEP_ANGLE = 20;

    static void Main(string[] args)
    {
        new Sakbot().Start();
    }

    Sakbot() : base(BotInfo.FromFile("Sakbot.json")) { }

    public override void Run()
    {
        while (IsRunning)
        {
            // Jika musuh tidak terdeteksi atau sudah lama tidak terlihat, scan area
            if (!enemyDetected || (TurnNumber - lastSeenTurn > 5))
            {
                // Lakukan scanning
                TurnRadarRight(RADAR_SWEEP_ANGLE);
            }
            else
            {
                MoveTowardEnemy();
                FireIfInRange();
            }
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Simpan posisi musuh
        enemyX = e.X;
        enemyY = e.Y;
        enemyDetected = true;
        lastSeenTurn = TurnNumber;

        // Hitung sudut ke musuh, lalu sesuaikan radar
        double angleToEnemy = BearingTo(enemyX, enemyY);
        double radarAdjust = NormalizeAngle(angleToEnemy - RadarDirection);
        TurnRadarRight(radarAdjust);
    }

    private void MoveTowardEnemy()
    {
        // Hitung jarak dan arah ke musuh
        double distance = CalculateDistance(enemyX, enemyY);
        double angleToEnemy = BearingTo(enemyX, enemyY);

        TurnLeft(angleToEnemy);

        // Gerak hanya jika masih jauh dari safe_distance
        if (distance > SAFE_DISTANCE + 20)
        {
            Forward(distance - SAFE_DISTANCE);
        }
    }

    private void FireIfInRange()
    {
        double distance = CalculateDistance(enemyX, enemyY);

        // Nembak sekuat tenaga jika sudah dalam range dan gunheat 0
        if (distance <= FIRE_DISTANCE && GunHeat == 0)
        {
            Fire(3);
        }

        // Arahin lagi radar ke musuh
        double angleToEnemy = BearingTo(enemyX, enemyY);
        TurnRadarRight(angleToEnemy - RadarDirection);

        // Kalau musuh tidak terdeteksi selama 5 turn, reset ke sweeping awal
        if (TurnNumber - lastSeenTurn > 5)
        {
            enemyDetected = false;
            TurnRadarRight(RADAR_SWEEP_ANGLE);
        }
    }

    private double CalculateDistance(double x, double y)
    {
        return Math.Sqrt(Math.Pow(x - X, 2) + Math.Pow(y - Y, 2));
    }

    private double NormalizeAngle(double angle) // normalisasi sudut ke rentang [-180,180]
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
