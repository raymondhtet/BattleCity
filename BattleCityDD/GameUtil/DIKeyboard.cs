using System;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace GameUtil
{
	/// <summary>
	/// Responsible for keyboard device.
	/// </summary>
	public class DIKeyboard
		: DIDevice
	{
		#region Member Variables

		private KeyboardState _keys = null;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new DIKeyboard.
		/// </summary>
		/// <param name="window">Form which this DIKeyboard obtains input for.</param>
		public DIKeyboard(Form window) 
			: base(window)
		{
			// Just calls base constructor.
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Attempts to acquire access to the keyboard device.
		/// </summary>
		override protected void _acquireDevice()
		{
			_keys = null;
			base._acquireDevice();
		}

		/// <summary>
		/// Creates the DIKeyboard device.
		/// </summary>
		override protected void _createDevice() 
		{
			_device = new Device(SystemGuid.Keyboard);

			_device.SetDataFormat(DeviceDataFormat.Keyboard);
			_device.SetCooperativeLevel(
				_window, 
				CooperativeLevelFlags.NonExclusive | 
				CooperativeLevelFlags.NoWindowsKey | 
				CooperativeLevelFlags.Foreground
				);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates the DIKeyboard state.
		/// </summary>
		override public void Update()  
		{
			try {
				_keys = _device.GetCurrentKeyboardState();
			}
			catch {
				// Lost access to keyboard.
				_acquireDevice();
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets whether the specified keyboard key is pressed.
		/// </summary>
		public bool this[Key key] 
		{
			get {
				if (_keys != null) {
					return _keys[key];
				}

				return false;
			}
		}

		#endregion
	}
}
