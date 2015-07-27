using System;
using System.Runtime.InteropServices;

namespace GameUtil
{
	/// <summary>
	/// A high resolution query performance timer.
	/// </summary>
	public class QPTimer 
	{
		#region Imported Methods

		/// <summary>
		/// The current system ticks (count).
		/// </summary>
		/// <param name="lpPerformanceCount">Current performance count of the system.</param>
		/// <returns>False on failure.</returns>
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		/// <summary>
		/// Ticks per second (frequency) that the high performance counter performs.
		/// </summary>
		/// <param name="lpFrequency">Frequency the higher performance counter performs.</param>
		/// <returns>False if the high performance counter is not supported.</returns>
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		#endregion

		#region Member Variables

		private long _startTime = 0;
		private long _recordTime = 0;
		private float _ticksPerSecond = 0;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new QPTimer.
		/// </summary>
		public QPTimer() 
		{
			// No need to do anything.
		}

		#endregion

		#region Methods

		/// <summary>
		/// Records stop time and ticks per second from when this QPTimer was started.
		/// </summary>
		public void Record() 
		{
			// Record when the timer was recorded and the ticks per second.
			_recordTime = QPTimer.Counter;
			_ticksPerSecond = (float) (_recordTime - _startTime) / (float) QPTimer.Frequency;
		}

		/// <summary>
		/// Starts this QPTimer at the specified tick count.
		/// </summary>
		/// <param name="ticks">Tick count to start at.</param>
		public void Start(long ticks) 
		{
			// Record when the timer was started.
			_startTime = ticks;
		}

		#endregion

		#region Static Properties

		/// <summary>
		/// Gets the frequency that this QPTimer performs at.
		/// </summary>
		public static long Frequency 
		{
			get {
				long freq = 0;
				QueryPerformanceFrequency(out freq);

				return freq;
			}
		}

		/// <summary>
		/// Gets the current system ticks.
		/// </summary>
		public static long Counter 
		{
			get {
				long ticks = 0;
				QueryPerformanceCounter(out ticks);

				return ticks;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the number of ticks per second based off the current start and record times.
		/// </summary>
		public float TicksPerSecond 
		{
			get {
				return _ticksPerSecond;
			}
		}

		/// <summary>
		/// Gets the tick count of when this QPTimer was started.
		/// </summary>
		public long StartTime 
		{
			get {
				return _startTime;
			}
		}

		/// <summary>
		/// Gets the tick count of when this QPTimer was last recorded.
		/// </summary>
		public long RecordTime 
		{
			get {
				return _recordTime;
			}
		}

		#endregion
	}
}
