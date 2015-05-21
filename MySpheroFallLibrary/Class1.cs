﻿namespace MySpheroFallLibrary
{
    using RobotKit;
    using System;
    using System.Threading.Tasks;
    using Windows.Foundation;

    public sealed class SpheroControl
    {
        Sphero sphero;
        int r;
        int g;
        int rotation;
        float backlightBrightness;

        private SpheroControl(Sphero sphero)
        {
            this.sphero = sphero;
            this.r = this.g = this.b = 0;
            this.backlightBrightness = 0.0f;
            this.rotation = 0;
        }
        public int Red
        {
            get
            {
                return (this.r);
            }
            set
            {
                this.r = value;
                this.SetColour();
            }
        }
        public int Blue
        {
            get
            {
                return (this.b);
            }
            set
            {
                this.b = value;
                this.SetColour();
            }
        }
        public float BacklightBrightness
        {
            get
            {
                return (this.backlightBrightness);
            }
            set
            {
                this.backlightBrightness = value;
                this.sphero.SetBackLED(this.backlightBrightness);
            }
        }
        void setColour()
        {
            this.sphero.SetRGBLED(this.r, this.g, this.b);
        }
        public int Rotation
        {
            get
            {
                return (this.rotation);
            }
            set
            {
                this.rotation = value;
                this.sphero.Roll(this.rotation, 0);
            }
        }
        public void Roll(float speed)
        {
            this.sphero.Roll(this.rotation, speed);
        }

        public static IAsyncOperation<SpheroControl> GetFirstConnectedSpheroAsnyc()
        {
            Task<SpheroControl> task = InternalGetFirstConnectedSpheroAsync();
            return (task.AsAsyncOperation());
        }

        static Task<SpheroControl> InternalGetFirstConnectedSpheroAsync()
        {
            TaskCompletionSource<SpheroControl> task = new TaskCompletionSource<SpheroControl>();
            var provider = RobotProvider.GetSharedProvider();
            EventHandler<Robot> handler = null;

            handler = (s, robot) =>
                {
                    provider.ConnectedRobotEvent -= handler;
                    task.SetResult(new SpheroControl((Sphero)cxnRobot));
                };
            provider.ConnectedRobotEvent += handler;
            provider.FindRobots();

            return (task.Task);
        }
    }
}
