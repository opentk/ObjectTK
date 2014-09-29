using System.Diagnostics;

namespace DerpGL
{
    public class FrameTimer
    {
        public int FramesRendered { get; private set; }
        public double FrameTime { get; private set; }
        public double FpsBasedOnFrameTime { get; private set; }
        public double FpsBasedOnFramesRendered { get; private set; }

        private readonly Stopwatch _stopwatch;
        private double _elapsed;
        private int _fpsFrameCounter;

        private double _lastIntermediateTime;

        public FrameTimer()
        {
            _stopwatch = new Stopwatch();
            //Application.Idle += (sender, args) => Time();
            _stopwatch.Start();
        }

        public void Time()
        {
            // retrieve time spent since last frame
            _stopwatch.Stop();
            FrameTime = _stopwatch.Elapsed.TotalMilliseconds;
            _stopwatch.Restart();
            // calculate fps based on time spent on one frame
            FpsBasedOnFrameTime = (int) (1000/FrameTime);
            // calculate fps based on frames rendered during one second
            _elapsed += FrameTime;
            _fpsFrameCounter++;
            if (_elapsed > 1000)
            {
                _elapsed -= 1000;
                FpsBasedOnFramesRendered = _fpsFrameCounter;
                _fpsFrameCounter = 0;
            }
            // count frames rendered
            FramesRendered++;
            // reset intermediate timings
            _lastIntermediateTime = 0;
        }

        public double IntermediateTiming()
        {
            var elapsed = _stopwatch.Elapsed.TotalMilliseconds;
            var time = elapsed - _lastIntermediateTime;
            _lastIntermediateTime = elapsed;
            return time;
        }
    }
}