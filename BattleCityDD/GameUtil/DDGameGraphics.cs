using System;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Reflection;
using Microsoft.DirectX.DirectDraw;

namespace GameUtil
{
	/// <summary>
	/// Responsible for maintaining a list of DDGraphics.
	/// </summary>
	public class DDGameGraphics
		: IDisposable
	{
		#region Member Variables

		private Hashtable _graphics = new Hashtable();

		private bool _disposed = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new DDGameGraphics.
		/// </summary>
		/// <param name="file">XML list of DDGraphic descriptions.</param>
		/// <param name="gameSurface">DDGameSurface to create a list of DDGraphics with.</param>
		public DDGameGraphics(string file, DDGameSurface gameSurface)
		{
			XmlTextReader reader = new XmlTextReader(file);
			_readElements(reader, gameSurface);
			reader.Close();
		}

		/// <summary>
		/// Creates a new DDGameGraphics.
		/// </summary>
		/// <param name="stream">XML list of DDGraphic descriptions.</param>
		/// <param name="gameSurface">DDGameSurface to create a list of DDGraphics with.</param>
		public DDGameGraphics(Stream stream, DDGameSurface gameSurface)
		{
			XmlTextReader reader = new XmlTextReader(stream);
			_readElements(reader, gameSurface);
			reader.Close();
		}

		#endregion

		#region Dispose And Destructor

		/// <summary>
		/// Dispose of this DDGameGraphics.
		/// </summary>
		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose of this DDGameGraphics.
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
				foreach (DDGraphic graphic in _graphics) {
					graphic.Dispose();
				}
			}

			// DDGameGraphics is now disposed of.
			_disposed = true;
		}

		/// <summary>
		/// Destroys this DDGameGraphics.
		/// </summary>
		~DDGameGraphics() 
		{
			this.Dispose(false);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Reads each graphic element.
		/// </summary>
		/// <param name="reader">Reader to read elements from.</param>
		/// <param name="gameSurface">DDGameSurface that graphics will be created with.</param>
		private void _readElements(XmlTextReader reader, DDGameSurface gameSurface) 
		{
			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "graphic") {
					_readAttributes(reader, gameSurface);
				}
			}
		}

		/// <summary>
		/// Creates a graphic.
		/// </summary>
		/// <param name="reader">XmlTextReader to read graphic attributes from.</param>
		/// <param name="gameSurface">DDGameSurface that graphics will be created with.</param>
		private void _readAttributes(XmlTextReader reader, DDGameSurface gameSurface) 
		{
			// Default graphic values.
			string key = String.Empty;
			string path = String.Empty;

			bool embedded = false;

			Size frameSize = Size.Empty;
			int transColor = -1;

			// Read each graphic attribute.
			while (reader.MoveToNextAttribute()) {
				switch (reader.Name) {
					case "key":
						key = reader.Value;
						break;

					case "path":
						path = reader.Value;
						break;

					case "frameWidth":
						frameSize.Width = int.Parse(reader.Value);
						break;

					case "frameHeight":
						frameSize.Height = int.Parse(reader.Value);
						break;

					case "transColor":
						transColor = int.Parse(reader.Value, NumberStyles.HexNumber);
						break;

					case "embedded":
						embedded = bool.Parse(reader.Value);
						break;

					default:
						// No other attributes to handle.
						break;
				}
			}

			// Add graphic to graphics list.
			_addGraphic(key, path, frameSize, transColor, gameSurface, embedded);
		}

		/// <summary>
		/// Adds a DDGraphic to this DDGameGraphcis list.
		/// </summary>
		/// <param name="key">Key for graphic.</param>
		/// <param name="path">Path of graphic file.</param>
		/// <param name="frameSize">Size of DDGraphics frames.</param>
		/// <param name="transColor">Transparency color of DDGraphic.</param>
		/// <param name="gameSurface">DDGameSurface to create DDGraphic with.</param>
		/// <param name="embedded">Set to true if path is coming from an embedded resource.</param>
		private void _addGraphic(string key, string path, Size frameSize, int transColor, DDGameSurface gameSurface, bool embedded) 
		{
			// Make sure a valid path and key was given.
			if (key == String.Empty || path == String.Empty) {
				return;
			}


			// Graphic to add.
			DDGraphic graphic = null;

			if (embedded) {
				// Create graphic from stream.
				Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream(path);

				try {
					graphic = new DDGraphic(stream, new SurfaceDescription(), gameSurface);
				}
				catch {
					// Couldn't create graphic.
					return;
				}

				stream.Close();
			}
			else {
				// Create graphic from file path.
				try {
					graphic = new DDGraphic(path, new SurfaceDescription(), gameSurface);
				}
				catch {
					// Couldn't create graphic.
					return;
				}
			}

			// Set graphic properties.
			graphic.SetTransparency(transColor);
			graphic.FrameSize = frameSize;

			// Add graphic to graphics list.
			_graphics.Add(key, graphic);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the specified DDGraphic from this DDGameGraphics.
		/// </summary>
		public DDGraphic this[string key] 
		{
			get {
				return (DDGraphic) _graphics[key];
			}
		}

		/// <summary>
		/// Gets the list of DDGraphics in this DDGameGraphics.
		/// </summary>
		public Hashtable List 
		{
			get {
				return _graphics;
			}
		}

		#endregion
	}
}
