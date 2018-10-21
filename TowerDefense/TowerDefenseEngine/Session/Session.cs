using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TowerDefenseEngine
{
    public class TowerEventArgs : EventArgs
    {
        public Tower t;
        public TowerEventArgs(Tower t)
        {
            this.t = t;
        }
    }
    public class Session
    {
        public delegate void PurhcaseTowerEventHandler(object sender, TowerEventArgs ptea);
        public delegate void SellTowerEventHandler(object sender, TowerEventArgs ptea);

        public Map Map
        {
            get;
            private set;
        }

        public string MoneyDisplay
        {
            get;
            private set;
        }

        public string TowersDisplay
        {
            get;
            private set;
        }

        public Player ActivePlayer
        {
            get;
            private set;
        }

        public Tower SelectedTower
        {
            get;
            private set;
        }

        public int health
        {
            get;
            private set;
        }

        public string HealthDisplay
        {
            get;
            private set;
        }

        public UserInterface UI
        {
            get;
            private set;
        }

        public bool IsPaused
        {
            get;
            private set;
        }

        public float MinSpeed
        {
            get;
            private set;
        }

        public float Speed
        {
            get;
            private set;
        }

        public float MaxSpeed
        {
            get;
            private set;
        }

        public static Session singleton;

        public event PurhcaseTowerEventHandler TowerPurchased;
        public event SellTowerEventHandler TowerSold;
        public event EventHandler HealthDecreased;
        public event EventHandler MoneyIncreased;
        public event EventHandler MapFinished;

        public Session(Map map)
        {
            Map = map;
            ActivePlayer = new Player();
            ActivePlayer.Money = (uint)map.Money;
            ActivePlayer.PlacedTowers = new List<Tower>(20);
            health = 20;  //change to map settings

            MoneyDisplay = String.Format("Available Money: {0}", ActivePlayer.Money);
            TowersDisplay = String.Format("Placed Towers: {0}", ActivePlayer.PlacedTowers.Count);
            HealthDisplay = String.Format("Health: {0}", health);

            singleton = this;
            IsPaused = false;

            MinSpeed = 0.5f;
            Speed = 1.0f;
            MaxSpeed = 2.0f;
        }

        public void SetUI(UserInterface UI)
        {
            this.UI = UI;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPaused)
            {
                UI.UpdateUI(gameTime);

                foreach (Tower t in ActivePlayer.PlacedTowers)
                {
                    t.Update(gameTime, UI.mouse);
                }

                Map.Update(gameTime);

                if (Map.State == MapState.Finished && MapFinished != null)
                {
                    MapFinished(this, EventArgs.Empty);
                }
            }
        }

        public void SelectTower(Tower t)
        {
            SelectedTower = t;
        }

        public void DeselectTower()
        {
            SelectedTower = null;
        }

        internal void PurchaseTower(Tower t, Point mapLocation)
        {
            if (t.Cost <= ActivePlayer.Money)
            {
                if (!t.Initialized) t.Initialize(Map);
                ActivePlayer.Money -= (uint)t.Cost;
                MoneyDisplay = String.Format("Available Money: {0}", ActivePlayer.Money.ToString());
                t.MapLocation = mapLocation;
                ActivePlayer.PlacedTowers.Add(t);
                TowersDisplay = String.Format("Placed Towers: {0}", ActivePlayer.PlacedTowers.Count);
                Map.SetValidPlacement(t.MapLocation.X, t.MapLocation.Y, false);
                t.PlaceTower();

                if (TowerPurchased != null)
                    TowerPurchased(this, new TowerEventArgs(t));
            }
        }

        internal void UpgradeTower(Tower t)
        {
            if (t.UpgradeCost <= ActivePlayer.Money && t.Level + 1 < t.MaxLevel)
            {
                ActivePlayer.Money -= (uint)t.UpgradeCost;
                MoneyDisplay = String.Format("Available Money: {0}", ActivePlayer.Money.ToString());
                t.Upgrade();
            }
        }

        internal void SellTower(Tower t)
        {
            ActivePlayer.Money += (uint)(t.TotalCost * t.SellScalar);
            MoneyDisplay = String.Format("Available Money: {0}", ActivePlayer.Money.ToString());
            ActivePlayer.PlacedTowers.Remove(t);
            TowersDisplay = String.Format("Placed Towers: {0}", ActivePlayer.PlacedTowers.Count);
            Map.SetValidPlacement(t.MapLocation.X, t.MapLocation.Y, true);

            if (TowerSold != null)
                TowerSold(this, new TowerEventArgs(t));
        }

        internal void DecreaseHealth(int p)
        {
            health -= p;
            HealthDisplay = String.Format("Health: {0}", health);

            if (HealthDecreased != null)
                HealthDecreased(this, EventArgs.Empty);
        }

        internal void AddMoney(int p)
        {
            ActivePlayer.Money += (uint)p;
            MoneyDisplay = String.Format("Available Money: {0}", ActivePlayer.Money);

            if (MoneyIncreased != null)
                MoneyIncreased(this, EventArgs.Empty);
        }

        internal void IncreaseSpeed(float amount)
        {
            if (Speed < MaxSpeed)
            {
                Speed += amount;
                MathHelper.Clamp(Speed, MinSpeed, MaxSpeed);
            }
        }

        internal void DecreaseSpeed(float amount)
        {
            if (Speed > MinSpeed)
            {
                Speed -= amount;
                MathHelper.Clamp(Speed, MinSpeed, MaxSpeed);
            }
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
    }
}
