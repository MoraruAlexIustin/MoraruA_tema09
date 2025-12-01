using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleApp1
{
    public partial class Form1 : Form
    {
        // Slot Machine Logic
        private readonly Random _random = new Random();
        private readonly int _symbolCount = 4; 
        private readonly int _slotCount = 3;   
        private int[] _currentSymbols;

        // Animation/Timing
        private Timer _animationTimer;
        private int _remainingCycles = 0;
        private float _timePerCycle = 0.5f; 
        private DateTime _lastUpdateTime;
        private float _rotationOffset = 0.0f; 
        private float _SpinSpeed = 20.0f; 

        // OpenGL/Texture Handling
        private int[] _textures; // IDs pentru cele 4 texturi (simboluri)
        private readonly string[] _imageFiles = { "symbol1.png", "symbol2.png", "symbol3.png", "symbol4.png" }; 
        public Form1()
        {
            InitializeComponent();
            _currentSymbols = new int[_slotCount];
            for (int i = 0; i < _slotCount; i++)
            {
                _currentSymbols[i] = _random.Next(_symbolCount);
            }

            // Initializare Timer pentru animatie
            _animationTimer = new Timer { Interval = 16 }; 
            _animationTimer.Tick += AnimationTimer_Tick;

            glControlSlots.Load += glControlSlots_Load;
            glControlSlots.Paint += glControlSlots_Paint;
            glControlSlots.Resize += glControlSlots_Resize;
            btnPull.Click += btnPull_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void glControlSlots_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.15f, 0.15f, 0.15f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc((BlendingFactor)BlendingFactorSrc.SrcAlpha, (BlendingFactor)BlendingFactorDest.OneMinusSrcAlpha);
            LoadTextures();
            glControlSlots_Resize(sender, e);
        }

        private void LoadTextures()
        {
            _textures = new int[_symbolCount];
            GL.GenTextures(_symbolCount, _textures);

            for (int i = 0; i < _symbolCount; i++)
            {
                try
                {
                    GL.BindTexture(TextureTarget.Texture2D, _textures[i]);

                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(_imageFiles[i]))
                    {
                        System.Drawing.Imaging.BitmapData data = bmp.LockBits(
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            System.Drawing.Imaging.ImageLockMode.ReadOnly,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                        bmp.UnlockBits(data);
                    }

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la încărcarea texturii {_imageFiles[i]}: {ex.Message}. Asigură-te că fișierul este prezent.", "Eroare Critică", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void glControlSlots_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControlSlots.Width, glControlSlots.Height);
        }

        private void glControlSlots_Paint(object sender, PaintEventArgs e)
        {
            if (_textures == null || _textures.Length == 0) return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Seteaza Proiectia 2D
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, glControlSlots.Width, glControlSlots.Height, 0, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            float slotWidth = glControlSlots.Width / _slotCount;
            float slotHeight = glControlSlots.Height;
            float symbolSize = Math.Min(slotWidth, slotHeight) * 0.7f;
            float xPadding = (slotWidth - symbolSize) / 2.0f;
            float yCenter = slotHeight / 2.0f;

            for (int i = 0; i < _slotCount; i++)
            {
                float xPos = i * slotWidth + xPadding;

                if (_remainingCycles > 0)
                {
                    float animatedSymbolIndex = (_currentSymbols[i] + _rotationOffset);

                    // Deseneaza doua simboluri (curent si urmatorul) pentru efectul de derulare
                    DrawSymbol(xPos, yCenter, symbolSize, (int)Math.Floor(animatedSymbolIndex) % _symbolCount, animatedSymbolIndex - (int)Math.Floor(animatedSymbolIndex));
                    DrawSymbol(xPos, yCenter, symbolSize, (int)Math.Ceiling(animatedSymbolIndex) % _symbolCount, (animatedSymbolIndex - (int)Math.Floor(animatedSymbolIndex)) - 1.0f);
                }
                else
                {
                    // Simbol fix
                    DrawSymbol(xPos, yCenter, symbolSize, _currentSymbols[i]);
                }
            }

            glControlSlots.SwapBuffers();
        }

        private void DrawSymbol(float x, float yCenter, float size, int symbolIndex, float verticalOffsetFactor = 0.0f)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textures[symbolIndex]);

            float halfSize = size / 2.0f;
            float yOffset = verticalOffsetFactor * size;

            float yTop = yCenter - halfSize + yOffset;
            float yBottom = yCenter + halfSize + yOffset;

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(x, yTop);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(x + size, yTop);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(x + size, yBottom);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(x, yBottom);

            GL.End();
        }

        private void btnPull_Click(object sender, EventArgs e)
        {
            if (_remainingCycles > 0) return;

            _remainingCycles = (int)numericUpDownCycles.Value;

            // Genereaza rezultatul final
            for (int i = 0; i < _slotCount; i++)
            {
                _currentSymbols[i] = _random.Next(_symbolCount);
            }

            btnPull.Enabled = false;
            numericUpDownCycles.Enabled = false;

            _lastUpdateTime = DateTime.Now;
            _rotationOffset = 0.0f;
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - _lastUpdateTime;
            float deltaTime = (float)elapsed.TotalSeconds;
            _lastUpdateTime = DateTime.Now;

            float deltaRotation = _SpinSpeed * deltaTime;

            // Logica de Oprire
            if (_remainingCycles <= 0)
            {
                // Opreste-te exact pe 0.0f
                if (_rotationOffset < deltaRotation)
                {
                    _rotationOffset = 0.0f;
                    _remainingCycles = 0;
                    _animationTimer.Stop();
                    btnPull.Enabled = true;
                    numericUpDownCycles.Enabled = true;
                    CheckWinCondition();
                }
            }

            // Logica de Rotire
            else
            {
                _rotationOffset += deltaRotation;

                // Un ciclu de rotire (simbol vizual)
                if (_rotationOffset >= 1.0f)
                {
                    _rotationOffset -= 1.0f;
                    _remainingCycles--;
                    if (_remainingCycles < 0) _remainingCycles = 0;
                }
            }

            glControlSlots.Invalidate();
        }

        private void CheckWinCondition()
        {
            bool win = (_currentSymbols[0] == _currentSymbols[1] && _currentSymbols[1] == _currentSymbols[2]);
            string message;
            string title;
            MessageBoxIcon icon;

            if (win)
            {
                message = "!!! AI CÂȘTIGAT !!!";
                title = "CÂȘTIG!";
                icon = MessageBoxIcon.Exclamation;
            }
            else
            {
                message = $"AI PIERDUT";
                title = "PIERDERE";
                icon = MessageBoxIcon.Information;
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

    }
}
