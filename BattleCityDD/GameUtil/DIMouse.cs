using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX.DirectInput;

namespace GameUtil
{
	/// <summary>
	/// Responsible for mouse device.
	/// </summary>
	public class DIMouse
		: DIDevice
	{
		#region Member Variables

		private bool[] _buttons = { false, false, false };
		private DIMouseWheelDirections _wheelDirection = DIMouseWheelDirections.Idle;

		private Point _cursor = Point.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new DIMouse for the specified window.
		/// </summary>
		/// <param name="window">Window to obtain mouse input for.</param>
		public DIMouse(Form window)
			: base(window)
		{
			// Just call base constructor.
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Attempts to acquire access to the mouse device.
		/// </summary>
		override protected void _acquireDevice() 
		{
			_initMouse();
			base._acquireDevice();
		}

		/// <summary>
		/// Creates mouse device.
		/// </summary>
		override protected void _createDevice() 
		{
			_device = new Device(SystemGuid.Mouse);

			_device.SetDataFormat(DeviceDataFormat.Mouse);
			_device.SetCooperativeLevel(_window, 
				CooperativeLevelFlags.NonExclusive | CooperativeLevelFlags.Foreground);
		}

		/// <summary>
		/// Initializes this DIMouse state.
		/// </summary>
		private void _initMouse() 
		{
			// Wheel is idle.
			_wheelDirection = DIMouseWheelDirections.Idle;

			// No buttons are being pressed.
			for (DIMouseButtons button = DIMouseButtons.Left; button <= DIMouseButtons.Wheel; button = button + 1) {
				_buttons[(int) button] = false;
			}
		}

		/// <summary>
		/// Sets this DIMouse cursor information.
		/// </summary>
		private void _setCursor() 
		{
			// Get current cursor position.
			_cursor = _window.PointToClient(Cursor.Position);

			// Make sure cursor coordinates are valid.
			if (_cursor.X < 0) {
				_cursor.X = 0;
			}
			else if (_cursor.X > _window.Width) {
				_cursor.X = _window.Width;
			}

			if (_cursor.Y < 0) {
				_cursor.Y = 0;
			}
			else if (_cursor.Y > _window.Height) {
				_cursor.Y = _window.Height;
			}
		}

		/// <summary>
		/// Sets this DIMouse button information.
		/// </summary>
		/// <param name="buttonState">Array of button states.</param>
		private void _setButtons(byte[] buttonState) 
		{
			// Set each button based off its state.
			for (DIMouseButtons button = DIMouseButtons.Left; button <= DIMouseButtons.Wheel; button = button + 1) {
				if (buttonState[(int) button] == (int) DIMouseClicked.False) {
					_buttons[(int) button] = false;
				}
				else {
					_buttons[(int) button] = true;
				}
			}
		}

		/// <summary>
		/// Sets the wheel direction.
		/// </summary>
		/// <param name="wheelMovement">Movement of the mousewheel.</param>
		private void _setWheelDirection(int wheelMovement) 
		{
			// Check if wheel was moved up.
			if (wheelMovement > 0) {
				_wheelDirection = DIMouseWheelDirections.Up;
			}
				// Check if wheel was moved down.
			else if (wheelMovement < 0) {
				_wheelDirection = DIMouseWheelDirections.Down;
			}
				// Wheel is idle.
			else {
				_wheelDirection = DIMouseWheelDirections.Idle;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates this DIMouse state.
		/// </summary>
		override public void Update() 
		{
			// Set cursor coordinates.
			_setCursor();

			try {
				// Get mouse state.
				MouseState state = _device.CurrentMouseState;	

				// Save state.
				_setButtons(state.GetMouseButtons());
				_setWheelDirection(state.Z);
			}
			catch {
				// Lost access to mouse.
				_acquireDevice();
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets whether the left button is clicked.
		/// </summary>
		public bool LeftClick 
		{
			get {
				return _buttons[(int) DIMouseButtons.Left];
			}
		}

		/// <summary>
		/// Gets whether the right button is clicked.
		/// </summary>
		public bool RightClick 
		{
			get {
				return _buttons[(int) DIMouseButtons.Right];
			}
		}

		/// <summary>
		/// Gets whether the wheel button is clicked.
		/// </summary>
		public bool WheelClick 
		{
			get {
				return _buttons[(int) DIMouseButtons.Wheel];
			}
		}

		/// <summary>
		/// Gets the coordinates of this DIMouse cursor.
		/// </summary>
		public Point Location 
		{
			get {
				return _cursor;
			}
		}

		/// <summary>
		/// Gets the direction that this DIMouse wheel was moved in.
		/// </summary>
		public DIMouseWheelDirections WheelDirection 
		{
			get {
				return _wheelDirection;
			}
		}

		#endregion
	}
}
