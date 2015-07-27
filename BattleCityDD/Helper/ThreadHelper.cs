using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BattleCity.Utils
{
    public sealed class ThreadHelper
    {
        #region Variables
        /// <summary>
        /// Delegate defining for-loop's body.
        /// </summary>
        /// <param name="index">Loop's index.</param>
        public delegate void ForLoopBody(int index);

        // number of threads for parallel computations
        private static int threadsCount = System.Environment.ProcessorCount;
        // object used for synchronization
        private static object sync = new Object();

        // single instance of the class to implement singleton pattern
        private static volatile ThreadHelper instance = null;
        // background threads for parallel computation
        private Thread[] threads = null;

        // events to signal about job availability and thread availability
        private AutoResetEvent[] jobAvailable = null;
        private ManualResetEvent[] threadIdle = null;

        // loop's body and its current and stop index
        private int currentIndex;
        private int stopIndex;
        private ForLoopBody loopBody;
        #endregion

        #region Constructor
        private ThreadHelper()
        {
        }
        #endregion

        #region Parallel Methods
        // Initialize Parallel class's instance creating required number of threads
        // and synchronization objects
        private void Initialize()
        {
            // array of events, which signal about available job
            this.jobAvailable = new AutoResetEvent[ThreadHelper.threadsCount];
            // array of events, which signal about available thread
            this.threadIdle = new ManualResetEvent[ThreadHelper.threadsCount];
            // array of threads
            this.threads = new Thread[ThreadHelper.threadsCount];

            for (int i = 0; i < ThreadHelper.threadsCount; i++)
            {
                this.jobAvailable[i] = new AutoResetEvent(false);
                this.threadIdle[i] = new ManualResetEvent(true);

                this.threads[i] = new Thread(new ParameterizedThreadStart(this.WorkerThread));
                //threads[i].SetApartmentState(ApartmentState.STA);
                this.threads[i].IsBackground = true;
                this.threads[i].Start(i);
            }
        }

        // Terminate all worker threads used for parallel computations and close all
        // synchronization objects
        private void Terminate()
        {
            // finish thread by setting null loop body and signaling about available work
            this.loopBody = null;
            for (int i = 0, threadsCount = this.threads.Length; i < ThreadHelper.threadsCount; i++)
            {
                this.jobAvailable[i].Set();
                // wait for thread termination
                this.threads[i].Join();

                // close events
                this.jobAvailable[i].Close();
                this.threadIdle[i].Close();
            }

            // clean all array references
            this.jobAvailable = null;
            this.threadIdle = null;
            this.threads = null;
        }

        // Worker thread performing parallel computations in loop
        private void WorkerThread(object index)
        {
            int threadIndex = (int)index;
            int localIndex = 0;

            while (true)
            {
                // wait until there is job to do
                this.jobAvailable[threadIndex].WaitOne();

                // exit on null body
                if (this.loopBody == null)
                    break;

                while (true)
                {
                    // get local index incrementing global loop's current index
                    localIndex = Interlocked.Increment(ref this.currentIndex);

                    if (localIndex >= this.stopIndex)
                        break;

                    // run loop's body
                    this.loopBody(localIndex);
                }

                // signal about thread availability
                this.threadIdle[threadIndex].Set();
            }
        }

        /// <summary>
        /// Executes a for-loop in which iterations may run in parallel. 
        /// </summary>
        /// <param name="start">Loop's start index.</param>
        /// <param name="stop">Loop's stop index.</param>
        /// <param name="loopBody">Loop's body.</param>
        /// <remarks><para>The method is used to parallel for-loop running its iterations in
        /// different threads. The <b>start</b> and <b>stop</b> parameters define loop's
        /// starting and ending loop's indexes. The number of iterations is equal to <b>stop - start</b>.
        /// </para>
        /// <para>Sample usage:</para>
        /// <code>
        /// ThreadHelper.For( 0, 20, delegate( int i )
        /// // which is equivalent to
        /// // for ( int i = 0; i &lt; 20; i++ )
        /// {
        ///     System.Diagnostics.Debug.WriteLine( "Iteration: " + i );
        ///     // ...
        /// } );
        /// </code>
        /// </remarks>
        public static void ParallelFor(int start, int stop, ForLoopBody loopBody)
        {
            lock (sync)
            {
                // get instance of parallel computation manager
                ThreadHelper instance = ThreadHelper.Instance;

                instance.currentIndex = start - 1;
                instance.stopIndex = stop;
                instance.loopBody = loopBody;

                // signal about available job for all threads and mark them busy
                for (int i = 0; i < threadsCount; i++)
                {
                    instance.threadIdle[i].Reset();
                    instance.jobAvailable[i].Set();
                }

                // wait until all threads become idle
                for (int i = 0; i < threadsCount; i++)
                {
                    instance.threadIdle[i].WaitOne();
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Number of threads used for parallel computations.
        /// </summary>
        /// <remarks><para>The property sets how many worker threads are created for paralleling
        /// loops' computations.</para>
        /// <para>By default the property is set to number of CPU's in the system
        /// (see <see cref="System.Environment.ProcessorCount"/>).</para>
        /// </remarks>
        public static int ThreadsCount
        {
            get { return threadsCount; }
            set
            {
                lock (sync)
                {
                    threadsCount = Math.Max(1, value);
                }
            }
        }

        // Get instace of the Parallel class
        private static ThreadHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThreadHelper();
                    instance.Initialize();
                }
                else
                {
                    if (instance.threads.Length != threadsCount)
                    {
                        // terminate old threads
                        instance.Terminate();
                        // reinitialize
                        instance.Initialize();

                        // TODO: change reinitialization to reuse already created objects
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Process Thread
        public delegate bool ProcessThreadCallback(ref object sender, params object[] args);

        public static void RunProcess(ProcessThreadCallback callback, params object[] args)
        {
            Thread oThread = new Thread((object sender) =>
            {
                if (sender is Thread)
                {
                    Thread tThread = sender as Thread;
                    if (tThread.IsAlive)
                    {
                        try
                        {
                            bool running = true;

                            do
                            {
                                running = callback(ref sender, args);
                            } while (running);

                            return;
                        }
                        catch (System.Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.Message);
                            return;
                        }
                    }
                }
            });
            oThread.Start(oThread);
        }
        #endregion

        #region Wait Thread
        public delegate bool WaitCallback(ref object sender);

        public static void Wait(IntPtr handle, WaitCallback callback)
        {
            Thread oThread = new Thread((object sender) =>
            {
                if (sender is Thread)
                {
                    Thread tThread = sender as Thread;
                    if (tThread.IsAlive)
                    {
                        try
                        {
                            bool running = ThreadHelper.IsHandle(handle);

                            do
                            {
                                running = callback(ref sender);
                            } while (running);

                            return;
                        }
                        catch (System.Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.Message);
                            return;
                        }
                    }
                }
            });
            oThread.Start(oThread);
        }

        private static bool IsHandle(IntPtr handle)
        {
            return (handle != IntPtr.Zero);
        }
        #endregion
    }
}
