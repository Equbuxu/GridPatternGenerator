using PatternGenerator.ClipboardUtils;
using System.ComponentModel;
using System.Drawing;

namespace PatternGenerator
{
    public class ViewModel : INotifyPropertyChanged
    {
        public int _cellWidth;
        public int CellWidth
        {
            get => _cellWidth;
            set
            {
                _cellWidth = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
        public int _cellHeight;
        public int CellHeight
        {
            get => _cellHeight;
            set
            {
                _cellHeight = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
        public int _gridWidth;
        public int GridWidth { 
            get => _gridWidth;
            set {
                _gridWidth = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
        public int _gridHeight;
        public int GridHeight
        {
            get => _gridHeight;
            set
            {
                _gridHeight = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public int WindowWidth { get; set; } = 18;
        public int WindowHeight { get; set; } = 16;

        public int WindowOffsetX { get; set; } = 0;
        public int WindowOffsetY { get; set; } = -2;

        public bool IsGridChecked { get; set; } = true;
        public bool IsWindowsChecked { get; set; } = false;

        public RelayCommand GridCommand { get; set; }
        public RelayCommand WindowsCommand { get; set; }

        public bool IsValid { 
            get
            {
                return CellWidth * GridWidth <= 5000 && CellHeight * GridHeight <= 5000;
            } 
        }

        public ViewModel()
        {
            GridCommand = new RelayCommand(CreateGrid, null);
            WindowsCommand = new RelayCommand(CreateWindows, null);
            CellWidth = 25;
            CellHeight = 25;
            GridWidth = 8;
            GridHeight = 10;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void CreateGrid(object _)
        {
            using (Bitmap image = new Bitmap(CellWidth * GridWidth, CellHeight * GridHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.Clear(System.Drawing.Color.Transparent);
                    for (int i = 0; i < GridWidth; i++)
                    {
                        int pos = i * CellWidth;
                        graphics.DrawLine(Pens.Black, pos, 0, pos, image.Height);
                        graphics.DrawLine(Pens.Black, CellWidth+pos-1, 0, CellWidth + pos - 1, image.Height);
                    }
                    for (int i = 0; i < GridHeight; i++)
                    {
                        int pos = i * CellHeight;
                        graphics.DrawLine(Pens.Black, 0, pos, image.Width, pos);
                        graphics.DrawLine(Pens.Black, 0, CellHeight + pos - 1, image.Width, CellHeight + pos - 1);
                    }
                }

                ClipboardHelper.SetClipboardImage(image);
            }
        }

        public void CreateWindows(object _)
        {
            using (Bitmap image = new Bitmap(CellWidth * GridWidth, CellHeight * GridHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.Clear(System.Drawing.Color.Transparent);
                    for (int i = 0; i < GridWidth; i++)
                    {
                        int pos = i * CellWidth;
                        graphics.DrawLine(Pens.Black, pos, 0, pos, image.Height);
                        graphics.DrawLine(Pens.Black, CellWidth + pos - 1, 0, CellWidth + pos - 1, image.Height);
                    }
                    for (int i = 0; i < GridHeight; i++)
                    {
                        int pos = i * CellHeight;
                        graphics.DrawLine(Pens.Black, 0, pos, image.Width, pos);
                        graphics.DrawLine(Pens.Black, 0, CellHeight + pos - 1, image.Width, CellHeight + pos - 1);
                    }

                    int windowStartX = (int)(CellWidth / 2.0 - WindowWidth / 2.0) + WindowOffsetX;
                    int windowStartY = (int)(CellHeight / 2.0 - WindowHeight / 2.0) + WindowOffsetY;
                    for (int i = 0; i < GridWidth; i++)
                    {
                        for (int j = 0; j < GridHeight; j++)
                        {
                            int x = i * CellWidth;
                            int y = j * CellHeight;
                            graphics.DrawRectangle(Pens.Black, new Rectangle(x + windowStartX, y + windowStartY, WindowWidth, WindowHeight));
                        }
                    }

                }

                ClipboardHelper.SetClipboardImage(image);
            }
        }
    }
}
