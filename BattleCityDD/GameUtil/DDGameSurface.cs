using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.DirectX.DirectDraw;

namespace GameUtil
{
	/// <summary>
	/// Responsible for main game surfaces and device.
	/// </summary>
	public class DDGameSurface
		: IDisposable
	{
		#region Member Variables

		private Control _window = null;
		private Device _device = null;

		private Clipper _clipper = null;

		private Surface _surface = null;
		private Surface _buffer = null;

		private bool _disposed = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a DDGameSurface.
		/// </summary>
		/// <param name="window">Window to attach this DDGameSurface to.</param>
		public DDGameSurface(Control window) 
		{
			// Set DDGameSurface info.
			_window = window;

			// Create DDGameSurface.
			_createDevice();
			_initSurfaces();
		}

		#endregion

		#region Dispose And Destructor

		/// <summary>
		/// Dispose of this DDGameSurface.
		/// </summary>
		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose of this DDGameSurface.
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
				_surface.Dispose();
				_buffer.Dispose();
				_clipper.Dispose();
			}

			// DDGameSurface is now disposed of.
			_disposed = true;
		}

		/// <summary>
		/// Destroys this DDGameSurface.
		/// </summary>
		~DDGameSurface() 
		{
			this.Dispose(false);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Creates this DDGameSurface device based off the current window and fullscreen mode.
		/// </summary>
		private void _createDevice() 
		{
			_device = new Device();
			_device.SetCooperativeLevel(_window, CooperativeLevelFlags.Normal);
		}

		/// <summary>
		/// Creates this DDGameSurface clipper and surfaces based off the current window and device.
		/// </summary>
		private void _initSurfaces() 
		{
			_createSurface();
			_createBuffer();
		}

		/// <summary>
		/// Creates this DDGameSurface surface clipper based off the current window and device.
		/// </summary>
		private void _createClipper() 
		{
			_clipper = new Clipper(_device);
			_clipper.Window = _window;
		}

		/// <summary>
		/// Creates this DDGameSurface main surface based off the current device and clipper.
		/// </summary>
		private void _createSurface() 
		{
			// Create surface clipper.
			_createClipper();

			// Describe the primary surface.
			SurfaceDescription desc = new SurfaceDescription();
			desc.SurfaceCaps.PrimarySurface = true;

			// Create primary surface.
			_surface = new Surface(desc, _device);

			// Set clipping area.
			_surface.Clipper = _clipper;
		}

		/// <summary>
		/// Creates this DDGameSurface buffer surface based off the current device and main surface.
		/// </summary>
		private void _createBuffer() 
		{
			// Describe the buffer.
			SurfaceDescription desc = new SurfaceDescription();
			desc.Width = _surface.SurfaceDescription.Width;
			desc.Height = _surface.SurfaceDescription.Height;

			// Create the buffer.
			_buffer = new Surface(desc, _device);
		}

		/// <summary>
		/// Attempts to restore surfaces.
		/// </summary>
		private void _restoreSurfaces() 
		{
			try {
				_device.RestoreAllSurfaces();
			}
			catch {
				// Ignore errors.
			}
		}

		/// <summary>
		/// Draws quickly to the buffer.
		/// </summary>
		/// <param name="x">XCoordinate to start drawing at.</param>
		/// <param name="y">YCoordinate to start drawing at.</param>
		/// <param name="source">Surface to draw.</param>
		/// <param name="srcRect">Area of source surface to draw.</param>
		/// <param name="flags">Type of transfer.</param>
		private void _drawToBuffer(int x, int y, Surface source, Rectangle srcRect, DrawFastFlags flags) 
		{
			// Get actual drawing location.
			Point location = _window.PointToScreen(new Point(x, y));

			// Draw source to buffer.
			try {
				_buffer.DrawFast(location.X, location.Y, source, srcRect, flags);
			}
			catch {
				// Surface was lost.
				_restoreSurfaces();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Display the current DDGameSurface on to the screen.
		/// </summary>
		public void Display() 
		{
			// Rectangle for surface and buffer is identical.
			Rectangle rect = Rectangle.Empty;
			rect.Width = _surface.SurfaceDescription.Width;
			rect.Height = _surface.SurfaceDescription.Height;

			// Draw buffer to screen.
			try {
				_surface.Draw(rect, _buffer, rect, DrawFlags.DoNotWait);
			}
			catch {
				// Surface was lost.
				_restoreSurfaces();
			}
		}

		/// <summary>
		/// Clears the DDGameSurface with the specified Color.
		/// </summary>
		/// <param name="color">Color to clear DDGameSurface with.</param>
		public void Clear(Color color) 
		{
			try {
				_buffer.ColorFill(color);
			}
			catch {
				// Surface was lost.
				_restoreSurfaces();
			}
		}

		/// <summary>
		/// Draws on to the DDGameSurface.
		/// </summary>
		/// <param name="x">XCoordinate to draw source at.</param>
		/// <param name="y">YCoordinate to draw source at.</param>
		/// <param name="source">Source surface to draw.</param>
		/// <param name="srcRect">Rectangle area of source surface to draw.</param>
		public void Draw(int x, int y, Surface source, Rectangle srcRect) 
		{
			// Draw source to buffer.
			_drawToBuffer(x, y, source, srcRect, DrawFastFlags.DoNotWait);
		}

		/// <summary>
		/// Draws on to the DDGameSurface with a transparent surface.
		/// </summary>
		/// <param name="x">XCoordinate to draw source at.</param>
		/// <param name="y">YCoordinate to draw source at.</param>
		/// <param name="source">Source surface to draw.</param>
		/// <param name="srcRect">Rectangle area of source surface to draw.</param>
		public void DrawTransparent(int x, int y, Surface source, Rectangle srcRect) 
		{
			// Draw source to buffer.
			_drawToBuffer(x, y, source, srcRect, DrawFastFlags.DoNotWait | DrawFastFlags.SourceColorKey);
		}

		/// <summary>
		/// Draws text at the specified location.
		/// </summary>
		/// <param name="x">XCoordinate to draw at.</param>
		/// <param name="y">YCoordinate to draw at.</param>
		/// <param name="text">Text to draw</param>
		/// <param name="drawAtLastPosition">Draw at last position.</param>
		public void DrawText(int x, int y, string text, bool drawAtLastPosition) 
		{
			// Get actual drawing location.
			Point location = _window.PointToScreen(new Point(x, y));

			// Draw text to buffer.
			_buffer.DrawText(location.X, location.Y, text, drawAtLastPosition);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Device for this DDGameSurface.
		/// </summary>
		public Device Device 
		{
			get {
				return _device;
			}
		}

		/// <summary>
		/// Gets the Control that this DDGameSurface is attached to.
		/// </summary>
		public Control Window 
		{
			get {
				return _window;
			}
		}

		/// <summary>
		/// Gets whether this DDGameSurface can be drawn on.
		/// </summary>
		public bool CanDraw 
		{
			get {
				return _device.TestCooperativeLevel();
			}
		}

		/// <summary>
		/// Gets or sets the font foreground color.
		/// </summary>
		public Color FontForeColor 
		{
			get {
				return _buffer.ForeColor;
			}
			set {
				_buffer.ForeColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the font background color.
		/// </summary>
		public Color FontBackColor 
		{
			get {
				return _buffer.FontBackColor;
			}
			set {
				_buffer.FontBackColor = value;
			}
		}

		#endregion
	}
}
