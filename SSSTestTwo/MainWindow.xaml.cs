using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSSTestTwo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Container2D> containers2D;
        List<Container3D> containers3D;
        List<Package2D> packages2D;
        List<Package3D> packages3D;
        List<Bitmap> VisualizerImages = new List<Bitmap>();

        System.Windows.Controls.Image Visualizer2D = new System.Windows.Controls.Image();
        Viewport3D Visualizer3D = new Viewport3D();

        int Dimensions;
        int Length;
        int[] Max;


        public MainWindow()
        {
            InitializeComponent();
            SolutionMethodComboBox.Items.Add("NextFit");
            SolutionMethodComboBox.Items.Add("HighLowNextFit");
            SolutionMethodComboBox.Items.Add("LowHighNextFit");
            SolutionMethodComboBox.Items.Add("FirstFit");
            SolutionMethodComboBox.Items.Add("HighLowFirstFit");
            SolutionMethodComboBox.Items.Add("LowHighFirstFit");
            SolutionMethodComboBox.Items.Add("WorstFit");
            SolutionMethodComboBox.Items.Add("HighLowWorstFit");
            SolutionMethodComboBox.Items.Add("LowHighWorstFit");
            SolutionMethodComboBox.Items.Add("BestFit");
            SolutionMethodComboBox.Items.Add("HighLowBestFit");
            SolutionMethodComboBox.Items.Add("LowHighBestFit");
            LoadInputFileButton.Click += LoadInputFileButton_Click;
            CalculateSolutionButton.Click += CalculateSolutionButton_Click;
            NextButton.IsEnabled = false;
            PreviousButton.IsEnabled = false;
            NextButton.Click += NextButton_Click;
            PreviousButton.Click += PreviousButton_Click;
            RandomBoxesButton.Click += RandomBoxesButton_Click;
        }

        private void RandomBoxesButton_Click(object sender, RoutedEventArgs e)
        {
            RandomBoxes();
        }

        public void RandomBoxes()
        {
            Random r = new Random();
            packages2D = new List<Package2D>();
            for (int i = 0; i < int.Parse(RandomBoxesCount.Text); i++)
            {
                packages2D.Add(new Package2D(0, 0, r.Next(1, 48), r.Next(1, 48)));
            }
            Max = new int[1];
            Max[0] = 8 * 12;
            Max[1] = 40 * 12;
        }

        private void CalculateSolutionButton_Click(object sender, EventArgs e)
        {
            switch (Dimensions)
            {
                default:
                    containers2D = new List<Container2D>();
                    containers2D.Add(new Container2D(Max[0], Max[1])); //ISO 8'x40'  (x8.5' high) also come in 20'

                    packages2D = packages2D.OrderByDescending(b => b.Area()).ToList();

                    foreach (Package2D box in packages2D.ToArray())
                    {
                        bool foundContainer = false;
                        foreach (Container2D c in containers2D)
                        {
                            if (foundContainer = c.AddBox(box)) break;
                        }
                        if (!foundContainer) //If we didn't find a container it would fit in, we need another.
                        {
                            Container2D c = new Container2D(Max[0], Max[1]);
                            containers2D.Add(c);
                            c.AddBox(box);
                        }

                    }
                    foreach (Container2D container in containers2D)
                    {
                        VisualizerImages.Add(container.BinImage());
                    }
                    StepBox.Items.Add(String.Concat("\nThis test used ", containers2D.Count, " bins"));
                    Start2DVisualizer();
                    break;
                case 3:
                    containers3D = new List<Container3D>();
                    containers3D.Add(new Container3D(Max[0], Max[1], Max[2]));

                    packages3D = packages3D.OrderByDescending(b => b.Volume()).ToList();

                    foreach (Package3D box in packages3D.ToArray())
                    {
                        bool foundContainer = false;
                        foreach (Container3D c in containers3D)
                        {
                            if (foundContainer = c.AddBox(box)) break;
                        }
                        if (!foundContainer)
                        {
                            Container3D c = new Container3D(Max[0], Max[1], Max[2]);
                            containers3D.Add(c);
                            c.AddBox(box);
                        }
                    }
                    StepBox.Items.Add(String.Concat("\nThis test used ", containers3D.Count, " bins"));
                    Start3DVisualizer();
                    break;
            }
            //DrawContainer(0);
        }



        private void LoadInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                StepBox.Items.Add(String.Concat("\nOpened File: ", openFileDialog.FileName));
                GetFileInformation(openFileDialog.FileName);
            }
        }

        private void GetFileInformation(string _FileName)
        {
            packages2D = new List<Package2D>();
            var _Inputs = File.ReadAllLines(_FileName);
            Dimensions = (_Inputs[0].Split(',')).Length;
            StepBox.Items.Add(String.Concat("\nFile Dimensions: ", Dimensions.ToString()));
            switch (Dimensions)
            {
                default:
                    var _DimMax = (_Inputs[0].Split(':')[1].Split(','));
                    Max = new int[2];
                    Max[0] = Convert.ToInt16(_DimMax[0]);
                    Max[1] = Convert.ToInt16(_DimMax[1]);
                    StepBox.Items.Add(String.Concat("\nMaximum Bin Dimensions: ", Max[0].ToString(), ",", Max[1].ToString()));
                    Length = Convert.ToInt16((_Inputs[1].Split(':')[1].Split(';')).Length);
                    StepBox.Items.Add(String.Concat("\nNumber of Items: ", Length.ToString()));
                    var _FormattedInputs = _Inputs[1].Split(':')[1];
                    var _SeparatedInputs = _FormattedInputs.Split(';');
                    for (int _Index = 0; _Index < Length - 1; _Index++)
                    {
                        var _ShapeDims = _SeparatedInputs[_Index].Split(',');
                        packages2D.Add(new Package2D(0, 0, Int16.Parse(_ShapeDims[0]), Int16.Parse(_ShapeDims[1])));
                    }
                    break;
                case 3:
                    packages3D = new List<Package3D>();
                    _DimMax = (_Inputs[0].Split(':')[1].Split(','));
                    Max = new int[3];
                    Max[0] = Convert.ToInt16(_DimMax[0]);
                    Max[1] = Convert.ToInt16(_DimMax[1]);
                    Max[2] = Convert.ToInt16(_DimMax[2]);
                    StepBox.Items.Add(String.Concat("\nMaximum Bin Dimensions: ", Max[0].ToString(), ",", Max[1].ToString()));
                    Length = Convert.ToInt16((_Inputs[1].Split(':')[1].Split(';')).Length);
                    StepBox.Items.Add(String.Concat("\nNumber of Items: ", Length.ToString()));
                    _FormattedInputs = _Inputs[1].Split(':')[1];
                    _SeparatedInputs = _FormattedInputs.Split(';');
                    for (int _Index = 0; _Index < Length - 1; _Index++)
                    {
                        var _ShapeDims = _SeparatedInputs[_Index].Split(',');
                        packages3D.Add(new Package3D(0, 0, 0, Int16.Parse(_ShapeDims[0]), Int16.Parse(_ShapeDims[1]), Int16.Parse(_ShapeDims[2])));
                    }
                    break;
            }
        }

        private void Start2DVisualizer()
        {
            VisualizerSpace.Content = Visualizer2D;
            StepSlider.Maximum = containers2D.Count - 1;
            StepSlider.Minimum = 0;
            StepSlider.ValueChanged += StepSlider_ValueChanged2D;
            StepSlider.Value = 0;
            PreviousButton.IsEnabled = true;
            NextButton.IsEnabled = true;
        }
        private void Start3DVisualizer()
        {
            VisualizerSpace.Content = Visualizer3D;
            StepSlider.Maximum = containers3D.Count - 1;
            StepSlider.ValueChanged += StepSlider_ValueChanged3D;
            StepSlider.Minimum = 0;
            StepSlider.Value = 0;
            PreviousButton.IsEnabled = true;
            NextButton.IsEnabled = true;
            PerspectiveCamera myPCamera = new PerspectiveCamera();

            // Specify where in the 3D scene the camera is.
            myPCamera.Position = new Point3D(50, 50, 600);

            // Specify the direction that the camera is pointing.
            myPCamera.LookDirection = new Vector3D(0, 0, -1);

            // Define camera's horizontal field of view in degrees.
            myPCamera.FieldOfView = 60;

            // Asign the camera to the viewport
            Visualizer3D.Camera = myPCamera;
            Visualizer3D.Children.Add(containers3D[0].BinImage());
        }

        private void StepSlider_ValueChanged3D(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            throw new NotImplementedException();
        }

        private void StepSlider_ValueChanged2D(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var _Image = VisualizerImages[(int)e.NewValue];
            var rect = new System.Drawing.Rectangle(0, 0, _Image.Width, _Image.Height);
            var size = (rect.Width * rect.Height) * 4;
            var bitmapData = _Image.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Visualizer2D.Source = BitmapSource.Create(
                _Image.Width, _Image.Height, _Image.HorizontalResolution, _Image.VerticalResolution, PixelFormats.Bgra32, null, bitmapData.Scan0, size, bitmapData.Stride);
            _Image.UnlockBits(bitmapData);
        }
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            StepSlider.Value -= 1;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            StepSlider.Value += 1;
        }
    }
}
