using System;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace GameUtil
{
	/// <summary>
	/// Abstract class for a DirectInput Device.
	/// </summary>
	abstract public class DIDevice
		: IDisposable
	{
		#region Member Variables

		protected Device _device = null;
		protected Form _window = null;

		protected bool _disposed = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new DIDevice.
		/// </summary>
		/// <param name="window">Form which this DIDevice obtains input for.</param>
		public DIDevice(Form window) 
		{
			// Set device info.
			_window = window;

			// Set device.
			_createDevice();
			_acquireDevice();
		}

		#endregion

		#region Dispose And Destructor

		/// <summary>
		/// Dispose of this DIDevice.
		/// </summary>
		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose of this DIDevice.
		/// </summary>
		/// <param name="disposing">Set to true if disposing.</param>
		protected void Dispose(bool disposing) 
		{
			// Check if already disposed.
			if (_disposed) {
				return;
			}

			if (disposing) {
				// Cleanup managed resources.
				_device.Dispose();
			}

			// DIDevice is now disposed of.
			_disposed = true;
		}

		/// <summary>
		/// Destroys this DIDevice.
		/// </summary>
		~DIDevice() 
		{
			this.Dispose(false);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Attempts to acquire access to the desired Device.
		/// </summary>
		virtual protected void _acquireDevice() 
		{
			try {
				_device.Acquire();
			}
			catch {
				// Ignore errors.
			}
		}

		/// <summary>
		/// Creates the desired Device.
		/// </summary>
		abstract protected void _createDevice();

		#endregion

		#region Methods

		/// <summary>
		/// Updates the state of this DIDevice.
		/// </summary>
		abstract public void Update();

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Device for this DIDevice.
		/// </summary>
		public Device Device 
		{
			get {
				return _device;
			}
		}

		/// <summary>
		/// Gets the Form this DIDevice is accessing input for.
		/// </summary>
		public Form Window 
		{
			get {
				return _window;
			}
		}

		#endregion
	}
}
