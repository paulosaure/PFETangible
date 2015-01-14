using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Media.Animation;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;


namespace PaintSurface
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        const long valueBrosse = 0x1 ;
        const long valueDenti = 0x2;
        const long valueVerre = 0x3;
        const long action1Value = 0xA;
        const long action2Value = 0xB;
        const long action3Value = 0xC5;
        const long action4Value = 0xD;
        const long action5Value = 0xE;
        const long action6Value = 0xF;

        private bool brosseadentBool = false, verreBool = false, dentifriceBool = false;
        private Point brossePt, verrePt, dentifricePt;
        private Point action1Pt, action2Pt, action3Pt, action4Pt, action5Pt, action6Pt;

        Dictionary<long, long> listObjectAction = new Dictionary<long, long>();
        Dictionary<long, bool> listActionsPut = new Dictionary<long, bool>();
        Dictionary<long, bool> listObjetsPut = new Dictionary<long, bool>();
        Dictionary<long, Point> listActionsValuePositions = new Dictionary<long, Point>();
        Dictionary<Tuple<long, long>, Line> allLinks = new Dictionary<Tuple<long, long>, Line>();

        //Vue choix lieu
        private MediaPlayer cuisine = new MediaPlayer();
        private MediaPlayer salon = new MediaPlayer();
        private MediaPlayer salledebain = new MediaPlayer();

        //Vue actions
        private MediaPlayer coiffez = new MediaPlayer();
        private MediaPlayer rasez = new MediaPlayer();
        private MediaPlayer brossezdent = new MediaPlayer();
        private MediaPlayer douchez = new MediaPlayer();

        //Vue objets
        private MediaPlayer brosseadentSon = new MediaPlayer();
        private MediaPlayer dentifriceSon = new MediaPlayer();
        private MediaPlayer verreSon = new MediaPlayer();
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(Window1_Closing);
            //les sons
            cuisine.Open(new Uri(@"Resources\cuisine.wav", UriKind.Relative));
            salon.Open(new Uri(@"Resources\salon.wav", UriKind.Relative));
            salledebain.Open(new Uri(@"Resources\salledebain.wav", UriKind.Relative));
            coiffez.Open(new Uri(@"Resources\coiffez.wav", UriKind.Relative));
            rasez.Open(new Uri(@"Resources\rasez.wav", UriKind.Relative));
            douchez.Open(new Uri(@"Resources\douchez.wav", UriKind.Relative));
            brossezdent.Open(new Uri(@"Resources\brossezlesdents.wav", UriKind.Relative));
            brosseadentSon.Open(new Uri(@"Resources\sonBrosseDent.wav", UriKind.Relative));
            dentifriceSon.Open(new Uri(@"Resources\sonDentifrice.wav", UriKind.Relative));
            verreSon.Open(new Uri(@"Resources\sonVerre.wav", UriKind.Relative));

            listObjectAction.Add(action1Value, valueBrosse);
            listObjectAction.Add(action2Value, valueDenti);
            listObjectAction.Add(action3Value, valueBrosse);
            listObjectAction.Add(action4Value, valueBrosse);
            listObjectAction.Add(action5Value, valueVerre);
            listObjectAction.Add(action6Value, valueVerre);

            listActionsPut.Add(action1Value, false);
            listActionsPut.Add(action2Value, false);
            listActionsPut.Add(action3Value, false);
            listActionsPut.Add(action4Value, false);
            listActionsPut.Add(action5Value, false);
            listActionsPut.Add(action6Value, false);

            listObjetsPut.Add(valueBrosse, false);
            listObjetsPut.Add(valueDenti, false);
            listObjetsPut.Add(valueVerre, false);


            listActionsValuePositions.Add(action1Value, action1Pt);
            listActionsValuePositions.Add(action2Value, action2Pt);
            listActionsValuePositions.Add(action3Value, action3Pt);
            listActionsValuePositions.Add(action4Value, action4Pt);
            listActionsValuePositions.Add(action5Value, action5Pt);
            listActionsValuePositions.Add(action6Value, action6Pt);

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

        }


        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        private void itemPutOnTable(object sender, TagVisualizerEventArgs e)
        {
            myGrid.Visibility = Visibility.Hidden;
            objet.Visibility = Visibility.Visible;
            foundObject();
        }

        private async void foundObject()
        {
            //Objet en Texte
            await Task.Delay(3000);
            brosseDent.Source = new BitmapImage(new Uri("/Resources/brosseadents.png", UriKind.Relative));
            dentifrice.Source = new BitmapImage(new Uri("/Resources/dentifrice.png", UriKind.Relative));
            verre.Source = new BitmapImage(new Uri("/Resources/verre.png", UriKind.Relative));
            brosseDent2.Source = new BitmapImage(new Uri("/Resources/brosseadents.png", UriKind.Relative));
            dentifrice2.Source = new BitmapImage(new Uri("/Resources/dentifrice.png", UriKind.Relative));
            verre2.Source = new BitmapImage(new Uri("/Resources/verre.png", UriKind.Relative));

            //Objet en Image
            await Task.Delay(3000);
            brosseDent.Source = new BitmapImage(new Uri("/Resources/brosse_grandT.png", UriKind.Relative));
            dentifrice.Source = new BitmapImage(new Uri("/Resources/dentifrice_grand.png", UriKind.Relative));
            verre.Source = new BitmapImage(new Uri("/Resources/verre_grand.png", UriKind.Relative));
            brosseDent2.Source = new BitmapImage(new Uri("/Resources/brosse_grandT.png", UriKind.Relative));
            dentifrice2.Source = new BitmapImage(new Uri("/Resources/dentifrice_grand.png", UriKind.Relative));
            verre2.Source = new BitmapImage(new Uri("/Resources/verre_grand.png", UriKind.Relative));
            //Objet Son
            await Task.Delay(3000);
            try
            {
                brosseadentSon.Play();
            }
            catch (System.NullReferenceException) { }

            await Task.Delay(2000);
            try
            {
                dentifriceSon.Play();
            }
            catch (System.NullReferenceException) { }

            await Task.Delay(2000);
            try
            {
                verreSon.Play();
            }
            catch (System.NullReferenceException) { }
        }

        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            TagVisualizer visualizer = e.TagVisualization.Visualizer;
            Point pt = e.TagVisualization.Center;
            pt.Y = pt.Y + 210;
            long valueObjectPut = e.TagVisualization.VisualizedTag.Value;

        //   if (valueObjectPut == valueBrosse || valueObjectPut == valueDenti || valueObjectPut == valueVerre)
        //    {
                switch (valueObjectPut)
                {
                    case valueBrosse:
                        brossePt = pt;
                        borderAideBrosseDent.BorderBrush = Brushes.Green; 
                        borderAideBrosseDent2.BorderBrush = Brushes.Green; 
                        listObjetsPut[valueBrosse] = true;
                        valideObjet(brossePt, valueBrosse); 
                        break;
                    case valueDenti:
                        verrePt = pt;
                        borderDentifrice.BorderBrush = Brushes.Green; 
                        borderDentifrice2.BorderBrush = Brushes.Green; 
                        borderDentifrice.Visibility = Visibility.Visible;
                        listObjetsPut[valueDenti] = true;
                        valideObjet(dentifricePt, valueDenti); 
                        break;
                    case valueVerre:
                        dentifricePt = pt;
                        borderVerre.BorderBrush = Brushes.Green; 
                        borderVerre2.BorderBrush = Brushes.Green; 
                        borderVerre.Visibility = Visibility.Visible;
                        listObjetsPut[valueVerre] = true;
                        valideObjet(verrePt, valueVerre); 
                        break;
             //       default: break;
             //   }
        //    }
          //  else {
          //      switch (valueObjectPut)
         //       {
                    case action1Value:
                        action1Pt = pt;
                        listActionsPut[action1Value] = true;
                        valideActions(pt, brossePt, action1Value);
                        break;
                    case action2Value:
                        action2Pt = pt;
                        listActionsPut[action2Value] = true;
                        valideActions(pt, dentifricePt, action2Value);
                        break;
                    case action3Value:
                        action3Pt = pt;
                        listActionsPut[action3Value] = true;
                        valideActions(pt, brossePt, action3Value);
                        break;
                    case action4Value:
                        action4Pt = pt;
                        listActionsPut[action4Value] = true;
                        valideActions(pt, brossePt, action4Value);
                        break;
                    case action5Value:
                        action5Pt = pt;
                        listActionsPut[action5Value] = true;
                        valideActions(pt, verrePt, action5Value);
                        break;
                    case action6Value:
                        action6Pt = pt;
                        listActionsPut[action6Value] = true;
                        valideActions(pt, verrePt, action6Value);
                        break;
                    default: break;
                }
           // }
        }

        public void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            long valueObjectPut = e.TagVisualization.VisualizedTag.Value;

            switch (valueObjectPut)
            {
                case valueBrosse:
                    removeObject(valueBrosse);
                    brossePt = new Point();
                    borderAideBrosseDent.BorderBrush = null;
                    borderAideBrosseDent2.BorderBrush = null;
                    listObjetsPut[valueBrosse] = false;
                    break;
                case valueDenti:
                    removeObject(valueDenti);
                    verrePt = new Point();
                    borderDentifrice.BorderBrush = null;
                    borderDentifrice2.BorderBrush = null;
                    borderDentifrice.Visibility = Visibility.Hidden;
                    listObjetsPut[valueDenti] = false;
                    break;
                case valueVerre:
                    removeObject(valueVerre);
                    dentifricePt = new Point();
                    borderVerre.BorderBrush = null;
                    borderVerre2.BorderBrush = null ;
                    borderVerre.Visibility = Visibility.Hidden;
                    listObjetsPut[valueVerre] = false;
                    break;
                case action1Value:
                    listActionsPut[action1Value] = false;
                    removeAction(action1Value);
                    break;
                case action2Value:
                    listActionsPut[action2Value] = false;
                    removeAction(action2Value);
                    break;
                case action3Value:
                    listActionsPut[action3Value] = false;
                    removeAction(action3Value);
                    break;
                case action4Value:
                    listActionsPut[action4Value] = false;
                    removeAction(action4Value);
                    break;
                case action5Value:
                    listActionsPut[action5Value] = false;
                    removeAction(action5Value);
                    break;
                case action6Value:
                    listActionsPut[action6Value] = false;
                    removeAction(action6Value);
                    break;
                default: break;
            }
        }

        private void OnVisualizationMoved(object sender, TagVisualizerEventArgs e)
        {

        }

        private void valideObjet(Point a, long valueA)
        {
            drawLineWithObjects(a, valueA);

            if ( brosseadentBool && verreBool && dentifriceBool)
            {
                aideTop.Visibility = Visibility.Hidden;
                aideBot.Visibility = Visibility.Hidden;
            }
        }

        private void valideActions(Point a, Point b, long valueA)
        {
            drawLineWithActions(a, b, valueA);
            bool allActionPut = true;

            //Allez à la prochaine vue
            foreach (KeyValuePair<long, bool> pair in listActionsPut)
            {
                if (pair.Value == false)
                {
                    allActionPut = false;
                }
            }
            if (allActionPut)
             {
                 ordonnancement.Visibility = Visibility.Visible;
             }
        }

        private Line createLine(Point a, Point b)
        {
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = a.X;
            myLine.X2 = b.X;
            myLine.Y1 = a.Y;
            myLine.Y2 = b.Y;
            // Create a red Brush
            SolidColorBrush greenBrush = new SolidColorBrush();
            greenBrush.Color = Colors.Green;

            // Set Line's width and color
            myLine.StrokeThickness = 2;
            myLine.Stroke = greenBrush;
            return myLine;
        }

        private void drawLineWithActions(Point a, Point b, long valueA)
        {
            long valueB = listObjectAction[valueA];
            if (listObjetsPut[valueB])
            {
                Line myLine = createLine(a, b);
                allLinks.Add(new Tuple<long, long>(valueA, valueB), myLine);
                objet.Children.Add(myLine);
            }
        }

        public void drawLineWithObjects(Point a, long value)
        {
            foreach (KeyValuePair<long, long> pair in listObjectAction)
            {
                //Console.WriteLine("Test Remove : " + pair.Key + " et " + pair.Value);
                if (pair.Value == value && listActionsPut[pair.Key])
                {
                    Line myLine = createLine(listActionsValuePositions[pair.Key], a); 
                    Tuple<long, long> tmp = new Tuple<long, long>(pair.Key, pair.Value);
                    objet.Children.Add(myLine);
                    allLinks.Add(tmp, myLine);
                }
            }
        }

        private void removeObject(long value)
        {
            foreach (KeyValuePair<long, long> pair in listObjectAction)
            {
                //Console.WriteLine("Test Remove : " + pair.Key + " et " + pair.Value);
                if (pair.Value == value && listActionsPut[pair.Key])
                {
                    Tuple<long, long> tmp = new Tuple<long, long>(pair.Key, pair.Value);
                    objet.Children.Remove(allLinks[tmp]);
                    allLinks.Remove(tmp);
                }
            }
        }

        private void removeAction(long value){
            long objectValue = listObjectAction[value];
            Tuple<long,long> tmp = new Tuple<long,long>(value, objectValue);
            if (allLinks.ContainsKey(tmp))
            {
                objet.Children.Remove(allLinks[tmp]);
                allLinks.Remove(tmp);
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                // Save the current Fill brush so that you can revert back to this value in DragLeave.
   

                // If the DataObject contains string data, extract it.
                if (e.Data.GetData(typeof(ImageSource)) != null)
                {
                    //Trace.WriteLine("Entre DROP 2");
                    ImageSource image = e.Data.GetData(typeof(ImageSource)) as ImageSource;

                    img.Source = image;
                }
            }
        }

        void Window1_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

