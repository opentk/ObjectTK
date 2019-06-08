//
// FrameTimer.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using System.Diagnostics;

namespace ObjectTK.Tools
{
    /// <summary>
    /// Handles frame timing and fps calculation.
    /// </summary>
    public class FrameTimer
    {
        /// <summary>
        /// Total number of frames rendered.
        /// </summary>
        public int FramesRendered { get; private set; }

        /// <summary>
        /// Time spent for the last completed frame in milliseconds.
        /// </summary>
        public double FrameTime { get; private set; }

        /// <summary>
        /// Frames per second calculated from the time spent on the last frame.
        /// </summary>
        public double FpsBasedOnFrameTime { get; private set; }

        /// <summary>
        /// Frames per second calculated from the number of frames completed within the last second.
        /// </summary>
        public double FpsBasedOnFramesRendered { get; private set; }

        /// <summary>
        /// Total time running in milliseconds.
        /// </summary>
        public double TimeRunning { get; protected set; }

        private readonly Stopwatch _stopwatch;
        private double _elapsed;
        private int _fpsFrameCounter;

        private double _lastIntermediateTime;

        /// <summary>
        /// Initializes a new instance of the FrameTimer class.
        /// </summary>
        public FrameTimer()
        {
            _stopwatch = new Stopwatch();
            //Application.Idle += (sender, args) => Time();
            _stopwatch.Start();
        }

        /// <summary>
        /// Calculates timings based on the intervals between subsequent calls. Call once each frame.<br/>
        /// Determines frames per seconds and other statistics.
        /// </summary>
        public void Time()
        {
            // retrieve time spent since last frame
            FrameTime = _stopwatch.Elapsed.TotalMilliseconds;
            _stopwatch.Restart();
            // count time running
            TimeRunning += FrameTime;
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

        /// <summary>
        /// Calculates the interval between two subsequent calls in milliseconds.
        /// </summary>
        public double IntermediateTiming()
        {
            var elapsed = _stopwatch.Elapsed.TotalMilliseconds;
            var time = elapsed - _lastIntermediateTime;
            _lastIntermediateTime = elapsed;
            return time;
        }
    }
}