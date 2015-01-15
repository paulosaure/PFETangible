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

        Dictionary<long, Tag> tagList = new Dictionary<long, Tag>();
        Dictionary<Tuple<Action, Item>, Line> links = new Dictionary<Tuple<Action, Item>, Line>();

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

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        public Point calculPoint(TagVisualizerEventArgs e)
        {
            Point pt = e.TagVisualization.Center;
            pt.Y = pt.Y + 210;

            return pt;
        }

        private void itemPutOnTable(object sender, TagVisualizerEventArgs e)
        {
            myGrid.Visibility = Visibility.Hidden;
            objet.Visibility = Visibility.Visible;

            OnVisualizationAdded(sender, e);

            //Ajout de l'item dans la list
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
            long value = e.TagVisualization.VisualizedTag.Value;
            Point pt = calculPoint(e);

            if (!tagList.ContainsKey(value))
            {
                if (value == Constants.valueBrosse || value == Constants.valueDenti || value == Constants.valueVerre)
                {
                    tagList.Add(value, new Item(value, calculPoint(e)));
                }
                else
                {
                    tagList.Add(value, new Action(value, calculPoint(e)));
                }
                association(value);
            }
            
            tagList[value].setPosition(pt);
            tagList[value].setPut(true);

            switch (value)
            {
                case Constants.valueBrosse:
                    borderAideBrosseDent.BorderBrush = Brushes.Green; 
                    borderAideBrosseDent2.BorderBrush = Brushes.Green;
                    addLineWithObjects(value);
                    hideHelp();
                    break;
                case Constants.valueDenti:
                        borderDentifrice.BorderBrush = Brushes.Green; 
                        borderDentifrice2.BorderBrush = Brushes.Green;   
                        addLineWithObjects(value);
                        hideHelp();
                        break;
                case Constants.valueVerre:
                        borderVerre.BorderBrush = Brushes.Green; 
                        borderVerre2.BorderBrush = Brushes.Green;
                        addLineWithObjects(value);
                        hideHelp();
                        break;
                    default: 
                        addLineWithActions(value);
                        break;
                }
                switchViewOrdonnancement();
        }

        public void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            long value = e.TagVisualization.VisualizedTag.Value;
            tagList[value].setPut(false);
            tagList[value].setPosition(new Point());
            switch (value){
                case Constants.valueBrosse:
                        borderAideBrosseDent.BorderBrush = Brushes.Transparent;
                        borderAideBrosseDent2.BorderBrush = Brushes.Transparent;
                        removeObject(value);
                        break;
                case Constants.valueDenti:
                        borderDentifrice.BorderBrush = Brushes.Transparent;
                        borderDentifrice2.BorderBrush = Brushes.Transparent;
                        removeObject(value);
                        break;
                case Constants.valueVerre:
                        borderVerre.BorderBrush = Brushes.Transparent;
                        borderVerre2.BorderBrush = Brushes.Transparent ;
                        removeObject(value);
                        break;

            default: 
                removeAction(value);
                break;
            }
        }

        private void OnVisualizationMoved(object sender, TagVisualizerEventArgs e)
        {

        }

        private void hideHelp()
        {
            if (tagList.ContainsKey(Constants.valueBrosse) && tagList.ContainsKey(Constants.valueDenti) && tagList.ContainsKey(Constants.valueVerre))
            {
                if (tagList[Constants.valueBrosse].getPut() && tagList[Constants.valueDenti].getPut() && tagList[Constants.valueVerre].getPut())
                {
                    aideTop.Visibility = Visibility.Hidden;
                    aideBot.Visibility = Visibility.Hidden;
                }
            }
        }

        private void addLineWithActions(long value)
        {
            Action action = (Action)tagList[value];//On choppe l'action

            if (tagList.ContainsKey(action.getItem()))
            {
                if (tagList[action.getItem()].getPut())
                {
                    Item item = (Item)tagList[action.getItem()];
                    Line myLine = createLine(action.getPosition(), item.getPosition());
                    links.Add(new Tuple<Action, Item>(action, item), myLine);
                    objet.Children.Add(myLine);
                }
            }
            
        }

        public void addLineWithObjects(long value)
        {
            Item item = (Item)tagList[value]; //on choppe l'objet

            foreach (long action in item.getActions() )// Pour chaque actions associées à l'objet
            {
                if (tagList.ContainsKey(action))// Si l'action existe
                {
                    if (tagList[action].getPut())//Si l'action est posée
                    {
                        Line myLine = createLine(item.getPosition(), tagList[action].getPosition());
                        objet.Children.Add(myLine);
                        links.Add(new Tuple<Action, Item>((Action)tagList[action], item), myLine);
                    }
                }
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

        private void removeObject(long value)
        {
            Item item = (Item)tagList[value]; //on choppe l'objet

            foreach (long action in item.getActions()) // Pour chaque actions associées à l'objet
            {
                if(tagList.ContainsKey(action)) //Si l'action existe
                { 
                    if (tagList[action].getPut()) //Si l'action est posé
                    {
                        Tuple<Action, Item> tmp = new Tuple<Action, Item>((Action)tagList[action], item);
                        objet.Children.Remove(links[tmp]);
                        links.Remove(tmp);
                    }
                }
            }
        }

        private void removeAction(long value){
            Action action = (Action)tagList[value];//on choppe l'action

            if (tagList.ContainsKey(action.getItem())) // Si l'objet associé existe
            {
                if (tagList[action.getItem()].getPut()) // Si l'objet associé est posé sur la table
                {
                    Item item = (Item)tagList[action.getItem()]; //on récupère l'objet dans la liste
                    Tuple<Action, Item> tmp = new Tuple<Action, Item>(action, item);
                    objet.Children.Remove(links[tmp]);
                    links.Remove(tmp);
                }
            }
        }

        public void switchViewOrdonnancement()            //Allez à la prochaine vue
        {
            int cpt = 0;
            bool allTagPut = true;

            foreach (KeyValuePair<long, Tag> tag in tagList)
            {
                cpt++;

                if (tag.Value.getPut() == false)
                {
                    allTagPut = false;
                    break;
                }
            }
            if (allTagPut && cpt == Constants.nbTag)
            {
                ordonnancement.Visibility = Visibility.Visible;
            }
        }

        public void association(long value)
        {
            switch (value)
            {
                case Constants.valueBrosse:
                    Item brosse = (Item)tagList[value];
                    brosse.addAction(Constants.valueAction1);
                    brosse.addAction(Constants.valueAction3);
                    brosse.addAction(Constants.valueAction4);
                    break;
                case Constants.valueDenti:
                    Item denti = (Item)tagList[value];
                    denti.addAction(Constants.valueAction2);
                    break;
                case Constants.valueVerre:
                    Item verre = (Item)tagList[value];
                    verre.addAction(Constants.valueAction5);
                    verre.addAction(Constants.valueAction6);
                    break;
                case Constants.valueAction1:
                    Action action1 = (Action)tagList[value];
                    action1.setItem(Constants.valueBrosse);
                    break;
                case Constants.valueAction2:
                    Action action2 = (Action)tagList[value];
                    action2.setItem(Constants.valueDenti);
                    break;
                case Constants.valueAction3:
                    Action action3 = (Action)tagList[value];
                    action3.setItem(Constants.valueBrosse);
                    break;
                case Constants.valueAction4:
                    Action action4 = (Action)tagList[value];
                    action4.setItem(Constants.valueBrosse);
                    break;
                case Constants.valueAction5:
                    Action action5 = (Action)tagList[value];
                    action5.setItem(Constants.valueVerre);
                    break;
                case Constants.valueAction6:
                    Action action6 = (Action)tagList[value];
                    action6.setItem(Constants.valueVerre);
                    break;
                default: break;
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

