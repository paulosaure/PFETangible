using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace SurfacePaint
{
    /// <summary>
    /// Interaction logic for ColorPalette.xaml
    /// </summary>
    public partial class ColorPalette : TagVisualization
    {
        public ColorPalette()
        {
            InitializeComponent();
        }

        private void ColorPalette_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize ColorPalette's UI based on this.VisualizedTag here
            
        }

        /// <summary>
        /// Change the ellipse color based on the clicked SurfaceButton. It also change the InkCanvas color too.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onClick(object sender, RoutedEventArgs e)
        {
            //Get the clicked SurfaceButton
            SurfaceButton s = e.OriginalSource as SurfaceButton;

           
            //Set canvas color
            SolidColorBrush brush = s.Background as SolidColorBrush;
           // SurfaceWindow._currentColor = brush.Color;
            //SurfaceWindow.GetInstance().InkCanvas.DefaultDrawingAttributes.Color = brush.Color;
        }
    }
}
