using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Sakbot : Bot
{
    private double enemyX, enemyY;
    private bool enemyDetected = false;
    private int lastSeenTurn = 0; 

    private const int SAFE_DISTANCE = 50; 
    private const int FIRE_DISTANCE = 150; 
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
            if (!enemyDetected || (TurnNumber - lastSeenTurn > 5)) 
            {
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
        enemyX = e.X;
        enemyY = e.Y;
        enemyDetected = true;
        lastSeenTurn = TurnNumber; 

        double angleToEnemy = BearingTo(enemyX, enemyY);
        double radarAdjust = NormalizeAngle(angleToEnemy - RadarDirection);
        TurnRadarRight(radarAdjust);
    }

    private void MoveTowardEnemy()
    {
        double distance = CalculateDistance(enemyX, enemyY);
        double angleToEnemy = BearingTo(enemyX, enemyY);

        TurnLeft(angleToEnemy);

        if (distance > SAFE_DISTANCE + 20) 
        {
            Forward(distance - SAFE_DISTANCE);
        }
    }

    private void FireIfInRange()
    {
        double distance = CalculateDistance(enemyX, enemyY);

        if (distance <= FIRE_DISTANCE && GunHeat == 0)
        {
            Fire(3);
        }

        double angleToEnemy = BearingTo(enemyX, enemyY);
        TurnRadarRight(angleToEnemy - RadarDirection);

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

    private double NormalizeAngle(double angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
