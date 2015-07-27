using System;
using System.Drawing;
using System.IO;
using Microsoft.DirectX.DirectDraw;

namespace GameUtil
{
	/// <summary>
	/// A Graphical Surface.
	/// </summary>
	public class DDGraphic
		: Surface 
	{
		#region Member Variables

		Rectangle _frame = Rectangle.Empty;

		int _frames = 0;
		int _framesPerCol = 0;
		int _framesPerRow = 0;

		int _transColor = -1;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="file">File path of a graphic.</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to created for.</param>
		public DDGraphic(string file, SurfaceDescription desc, DDGameSurface gameSurface)
			: base(file, desc, gameSurface.Device) 
		{
			_initGraphic(Size.Empty, -1);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="file">File path of a graphic.</param>
		/// <param name="frameSize">Size of this DDGraphics frames.</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to be created for.</param>
		public DDGraphic(string file, Size frameSize, SurfaceDescription desc, DDGameSurface gameSurface) 
			: base(file, desc, gameSurface.Device) 
		{
			_initGraphic(frameSize, -1);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="file">File path of a graphic.</param>
		/// <param name="frameSize">Size of this DDGraphics frames.</param>
		/// <param name="transColor">Transparency color to use (0x00RRGGBB format)</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to be created for.</param>
		public DDGraphic(string file, Size frameSize, int transColor, SurfaceDescription desc, DDGameSurface gameSurface) 
			: base(file, desc, gameSurface.Device) 
		{
			_initGraphic(frameSize, transColor);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="file">File path of a graphic.</param>
		/// <param name="transColor">Transparency color to use (0x00RRGGBB format)</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to be created for.</param>
		public DDGraphic(string file, int transColor, SurfaceDescription desc, DDGameSurface gameSurface) 
			: base(file, desc, gameSurface.Device) 
		{
			_initGraphic(Size.Empty, transColor);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="stream">Stream where the graphic is located.</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to created for.</param>
		public DDGraphic(Stream stream, SurfaceDescription desc, DDGameSurface gameSurface)
			: base(stream, desc, gameSurface.Device) 
		{
			_initGraphic(Size.Empty, -1);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="stream">Stream where the graphic is located.</param>
		/// <param name="frameSize">Size of this DDGraphics frames.</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to be created for.</param>
		public DDGraphic(Stream stream, Size frameSize, SurfaceDescription desc, DDGameSurface gameSurface) 
			: base(stream, desc, gameSurface.Device) 
		{
			_initGraphic(frameSize, -1);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="stream">Stream where the graphic is located.</param>
		/// <param name="frameSize">Size of this DDGraphics frames.</param>
		/// <param name="transColor">Transparency color to use (0x00RRGGBB format)</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to be created for.</param>
		public DDGraphic(Stream stream, Size frameSize, int transColor, SurfaceDescription desc, DDGameSurface gameSurface) 
			: base(stream, desc, gameSurface.Device) 
		{
			_initGraphic(frameSize, transColor);
		}

		/// <summary>
		/// Creates a new DDGraphic.
		/// </summary>
		/// <param name="stream">Stream where the graphic is located.</param>
		/// <param name="transColor">Transparency color to use (0x00RRGGBB format)</param>
		/// <param name="desc">SurfaceDescription of this DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface this DDGraphic is to be created for.</param>
		public DDGraphic(Stream stream, int transColor, SurfaceDescription desc, DDGameSurface gameSurface) 
			: base(stream, desc, gameSurface.Device) 
		{
			_initGraphic(Size.Empty, transColor);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Initializes this DDGraphic.
		/// </summary>
		/// <param name="frameSize">Size of this DDGraphics frames.</param>
		/// <param name="transColor">Transparency color in 0x00BBGGRR format (-1 for no transparency).</param>
		private void _initGraphic(Size frameSize, int transColor) 
		{
			// Initialize frames.
			_initFrames(frameSize);

			// Set transparency.
			this.SetTransparency(transColor);
		}

		/// <summary>
		/// Initializes the frames for this DDGraphic.
		/// </summary>
		/// <param name="frameSize">Size of frames.</param>
		private void _initFrames(Size frameSize) 
		{
			// Make sure the frame is of valid size.
			if (frameSize.Width > this.SurfaceDescription.Width || 0 >= frameSize.Width) {
				frameSize.Width = this.SurfaceDescription.Width;
			}

			if (frameSize.Height > this.SurfaceDescription.Height || 0 >= frameSize.Height) {
				frameSize.Height = this.SurfaceDescription.Height;
			}

			_frame.Size = frameSize;

			// Get number of frames.
			_framesPerCol = this.SurfaceDescription.Width / _frame.Size.Width;
			_framesPerRow = this.SurfaceDescription.Height / _frame.Size.Height;

			_frames = _framesPerCol * _framesPerRow;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns a Rectangle representing the area of a specific frame in this DDGraphic.
		/// </summary>
		/// <param name="index">The frame to be retrieved.</param>
		/// <returns>Rectangle area of the specified frame</returns>
		public Rectangle GetFrameArea(int index) 
		{
			// Make sure a valid frame was specified.
			if (index < 0 || index > _frames) {
				return Rectangle.Empty;
			}

			// Set frame to the index specified.
			_frame.X = (index % _framesPerCol) * _frame.Width;
			_frame.Y = (index / _framesPerCol) * _frame.Height;

			// Return area of specified frame location.
			return _frame;
		}

		/// <summary>
		/// Makes this DDGraphic transparent with the specified color.
		/// </summary>
		/// <param name="transColor">Transparent color in 0x00RRGGBB format.</param>
		public void SetTransparency(int transColor) 
		{
			// Make sure a valid color is given.
			if (transColor < 0) {
				return;
			}

			// Get transparent color.
			_transColor = transColor;

			// Set color key.
			ColorKey key = new ColorKey();

			key.ColorSpaceHighValue = _transColor;
			key.ColorSpaceLowValue = _transColor;

			this.SetColorKey(ColorKeyFlags.SourceDraw, key);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the number of Frames this DDGraphic has.
		/// </summary>
		public int Frames 
		{
			get {
				return _frames;
			}
		}

		/// <summary>
		/// Gets the number of frames per row this DDGraphic has.
		/// </summary>
		public int FramesPerRow 
		{
			get {
				return _framesPerRow;
			}
		}

		/// <summary>
		/// Gets the number of frames per column this DDGraphic has.
		/// </summary>
		public int FramesPerCol 
		{
			get {
				return _framesPerCol;
			}
		}

		/// <summary>
		/// Gets or sets the size of this DDGraphics frames.
		/// </summary>
		public Size FrameSize 
		{
			get {
				return _frame.Size;
			}
			set {
				_initFrames(value);
			}
		}

		/// <summary>
		/// Gets whether this DDGraphic is transparent or not.
		/// </summary>
		public bool IsTransparent 
		{
			get {
				return (_transColor != -1);
			}
		}

		/// <summary>
		/// Gets the transparent color of this DDGraphic, or -1 if no transparency is set.
		/// </summary>
		public int TransparentColor 
		{
			get {
				return _transColor;
			}
		}

		#endregion
	}
}
