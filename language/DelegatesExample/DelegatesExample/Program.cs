using System;

namespace DelegatesExample
{

	class MainClass
	{
		public static void Main (string[] args)
		{
			Car c = new Car ();
			c.Move ();
		}
	}

	public delegate void TankEmptyEventHandler(object sender, TankEmptyEventArgs e);

	public class Tank
	{
		public int Fuel { get; set; }
		// publisher, or event
		public event TankEmptyEventHandler tankEmpty;

		public Tank(int fuel)
		{
			Fuel = fuel;
		}
		// method that raise event
		public void OnTankEmpty(TankEmptyEventArgs e)
		{
			if (tankEmpty != null)
				tankEmpty (this, e);
		}
	}

	public class Engine
	{
		public Engine(Tank tank)
		{
			// subscribe to event 
			tank.tankEmpty += Stop;
		}

		// actually the subscriber
		public void Stop(object sender, TankEmptyEventArgs e)
		{
			Console.WriteLine("Engine has stoppeed. Fuel balance: {0}", e.FuelBalance);
		}
	}

	public class TankEmptyEventArgs
	{
		public int FuelBalance { get; set;}
		public TankEmptyEventArgs(int fuelBalance)
		{
			FuelBalance = fuelBalance;
		}
	}

	public class Car
	{
		Tank tank;
		Engine engine;

		public Car()
		{
			tank = new Tank(100);
			engine = new Engine (tank);
		}

		public void Move()
		{
			while(tank.Fuel > 5) {
				tank.Fuel -= 2;
				if (tank.Fuel < 5) {
					TankEmptyEventArgs e = new TankEmptyEventArgs (tank.Fuel);
					tank.OnTankEmpty (e);
				} else
					Console.WriteLine ("Moving... Tank balance is {0}", tank.Fuel);	
			}
		}
	}
}
