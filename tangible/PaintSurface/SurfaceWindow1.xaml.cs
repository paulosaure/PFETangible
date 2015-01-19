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

        private const int largueurTrait = 3;
        private Color colorLink = Colors.LightGreen;
        private SolidColorBrush colorValidationObjects = Brushes.LightGreen;
        private SolidColorBrush colorInValidationObjects = Brushes.Red;

        Dictionary<long, Tag> tagList = new Dictionary<long, Tag>(); //Objet + action
        Dictionary<Tuple<Action, Item>, Line> links = new Dictionary<Tuple<Action, Item>, Line>();
        Dictionary<long, Tuple<string, string>> linksFrieze = new Dictionary<long, Tuple<string, string>>();
        Dictionary<string, Border> linksBorder = new Dictionary<string, Border>();

        private MediaPlayer son = new MediaPlayer();

        public SurfaceWindow1()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(Window1_Closing);

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            //Modifie les valeurs des TAG dans le XAML
            giveTagValueToXaml();

        }

        public void giveTagValueToXaml()
        {
            tagObj1.Value = MyResources.valueBrosse;
            tagObj2.Value = MyResources.valueDenti;
            tagObj3.Value = MyResources.valueVerre;

            tagAct1.Value = MyResources.valueAction1;
            tagAct2.Value = MyResources.valueAction2;
            tagAct3.Value = MyResources.valueAction3;
            tagAct4.Value = MyResources.valueAction4;
            tagAct5.Value = MyResources.valueAction5;
            tagAct6.Value = MyResources.valueAction6;

            tagAct1b.Value = MyResources.valueAction1b;
            tagAct2b.Value = MyResources.valueAction2b;
            tagAct3b.Value = MyResources.valueAction3b;
            tagAct4b.Value = MyResources.valueAction4b;
            tagAct5b.Value = MyResources.valueAction5b;
            tagAct6b.Value = MyResources.valueAction6b;

            linksFrieze.Add(MyResources.valueAction1, new Tuple<string, string>(MyResources.friseTag1, MyResources.friseTag1b));
            linksFrieze.Add(MyResources.valueAction2, new Tuple<string, string>(MyResources.friseTag2, MyResources.friseTag2b));
            linksFrieze.Add(MyResources.valueAction3, new Tuple<string, string>(MyResources.friseTag3, MyResources.friseTag3b));
            linksFrieze.Add(MyResources.valueAction4, new Tuple<string, string>(MyResources.friseTag4, MyResources.friseTag4b));
            linksFrieze.Add(MyResources.valueAction5, new Tuple<string, string>(MyResources.friseTag5, MyResources.friseTag5b));
            linksFrieze.Add(MyResources.valueAction6, new Tuple<string, string>(MyResources.friseTag6, MyResources.friseTag6b));

            linksFrieze.Add(MyResources.valueAction1b, new Tuple<string, string>(MyResources.friseTag1, MyResources.friseTag1b));
            linksFrieze.Add(MyResources.valueAction2b, new Tuple<string, string>(MyResources.friseTag2, MyResources.friseTag2b));
            linksFrieze.Add(MyResources.valueAction3b, new Tuple<string, string>(MyResources.friseTag3, MyResources.friseTag3b));
            linksFrieze.Add(MyResources.valueAction4b, new Tuple<string, string>(MyResources.friseTag4, MyResources.friseTag4b));
            linksFrieze.Add(MyResources.valueAction5b, new Tuple<string, string>(MyResources.friseTag5, MyResources.friseTag5b));
            linksFrieze.Add(MyResources.valueAction6b, new Tuple<string, string>(MyResources.friseTag6, MyResources.friseTag6b));

            linksBorder.Add(MyResources.friseTag1, borderbloc1);
            linksBorder.Add(MyResources.friseTag2, borderbloc2);
            linksBorder.Add(MyResources.friseTag3, borderbloc3);
            linksBorder.Add(MyResources.friseTag4, borderbloc4);
            linksBorder.Add(MyResources.friseTag5, borderbloc5);
            linksBorder.Add(MyResources.friseTag6, borderbloc6);

            linksBorder.Add(MyResources.friseTag1b, borderbloc1Bot);
            linksBorder.Add(MyResources.friseTag2b, borderbloc2Bot);
            linksBorder.Add(MyResources.friseTag3b, borderbloc3Bot);
            linksBorder.Add(MyResources.friseTag4b, borderbloc4Bot);
            linksBorder.Add(MyResources.friseTag5b, borderbloc5Bot);
            linksBorder.Add(MyResources.friseTag6b, borderbloc6Bot);
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
            await Task.Delay(1000);
            son.Open(new Uri(@"Resources\sonBrosseDent.wav", UriKind.Relative));
            son.Play();

            await Task.Delay(2000);
            son.Open(new Uri(@"Resources\sonDentifrice.wav", UriKind.Relative));
            son.Play();

            await Task.Delay(2000);
            son.Open(new Uri(@"Resources\sonVerre.wav", UriKind.Relative));
            son.Play();
            await Task.Delay(1000);
        }

        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            long value = e.TagVisualization.VisualizedTag.Value;
            Point pt = calculPoint(e);

            if (!tagList.ContainsKey(value))
            {
                if (value == MyResources.valueBrosse || value == MyResources.valueDenti || value == MyResources.valueVerre)
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
            Console.WriteLine("Res : " + tagList[value].getId());

            switch (value)
            {
                case MyResources.valueBrosse:
                    borderAideBrosseDent.BorderBrush = borderAideBrosseDent2.BorderBrush = colorValidationObjects;
                    addLineWithObjects(value);
                    hideHelp();
                    break;
                case MyResources.valueDenti:
                    borderDentifrice.BorderBrush = borderDentifrice2.BorderBrush = colorValidationObjects; 
                    addLineWithObjects(value);
                    hideHelp();
                    break;
                case MyResources.valueVerre:
                    borderVerre.BorderBrush = borderVerre2.BorderBrush = colorValidationObjects;
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
                case MyResources.valueBrosse:
                        borderAideBrosseDent.BorderBrush = Brushes.Transparent;
                        borderAideBrosseDent2.BorderBrush = Brushes.Transparent;
                        removeObject(value);
                        break;
                case MyResources.valueDenti:
                        borderDentifrice.BorderBrush = Brushes.Transparent;
                        borderDentifrice2.BorderBrush = Brushes.Transparent;
                        removeObject(value);
                        break;
                case MyResources.valueVerre:
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
            long value = e.TagVisualization.VisualizedTag.Value;
            Point p = calculPoint(e);
            Tag tag = tagList[value];
           
            tag.setPosition(p);

            if(tag.GetType() == typeof(Item))
            {
                Item item = (Item)tagList[value];//on choppe l'action

                foreach (long action in item.getActions())
                {
                    if (tagList.ContainsKey(action)) //Si l'action existe
                    {
                        if (tagList[action].getPut()) //Si l'action est posé
                        {
                            Tuple<Action, Item> tmp = new Tuple<Action, Item>((Action)tagList[action], item);
                            Line line = links[tmp];
                            line.X1 = tagList[action].getPosition().X;
                            line.Y1 = tagList[action].getPosition().Y;
                            line.X2 = item.getPosition().X;
                            line.Y2 = item.getPosition().Y;
                        }
                    }
                }
            }
            else
            {
                Action action = (Action)tagList[value];//on choppe l'action

                if (tagList.ContainsKey(action.getItem()))
                {
                    if (tagList[action.getItem()].getPut())
                    {
                        Item item = (Item)tagList[action.getItem()]; //on récupère l'objet dans la liste
                        Tuple<Action, Item> tmp = new Tuple<Action, Item>(action, item);
                        Line line = links[tmp];
                        line.X1 = action.getPosition().X;
                        line.Y1 = action.getPosition().Y;
                        line.X2 = item.getPosition().X;
                        line.Y2 = item.getPosition().Y;
                    }
                }
            }
            
        }

        private void hideHelp()
        {
            if (tagList.ContainsKey(MyResources.valueBrosse) && tagList.ContainsKey(MyResources.valueDenti) && tagList.ContainsKey(MyResources.valueVerre))
            {
                if (tagList[MyResources.valueBrosse].getPut() && tagList[MyResources.valueDenti].getPut() && tagList[MyResources.valueVerre].getPut())
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
                    Tuple<Action, Item> pair = new Tuple<Action, Item>(action, item);
                    if (!links.ContainsKey(pair)) 
                    {
                        Line myLine = createLine(action.getPosition(), item.getPosition());
                        links.Add(pair, myLine); 
                        objet.Children.Add(myLine); 
                    }
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
                        Tuple<Action, Item> pair = new Tuple<Action, Item>((Action)tagList[action], item);
                        if (!links.ContainsKey(pair)) 
                        {
                            Line myLine = createLine(tagList[action].getPosition(), item.getPosition());
                            links.Add(pair, myLine); 
                            objet.Children.Add(myLine); 
                        }
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
            greenBrush.Color = colorLink;

            // Set Line's width and color
            myLine.StrokeThickness = largueurTrait;
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
            if (allTagPut && cpt == MyResources.nbTag)
            {
                ordonnancement.Visibility = Visibility.Visible;
            }
        }

        public void association(long value)
        {
            switch (value)
            {
                case MyResources.valueBrosse:
                    Item brosse = (Item)tagList[value];
                    brosse.addAction(MyResources.valueAction1);
                    brosse.addAction(MyResources.valueAction3);
                    brosse.addAction(MyResources.valueAction4);
                    break;
                case MyResources.valueDenti:
                    Item denti = (Item)tagList[value];
                    denti.addAction(MyResources.valueAction2);
                    break;
                case MyResources.valueVerre:
                    Item verre = (Item)tagList[value];
                    verre.addAction(MyResources.valueAction5);
                    verre.addAction(MyResources.valueAction6);
                    break;
                case MyResources.valueAction1:
                    Action action1 = (Action)tagList[value];
                    action1.setItem(MyResources.valueBrosse);
                    break;
                case MyResources.valueAction2:
                    Action action2 = (Action)tagList[value];
                    action2.setItem(MyResources.valueDenti);
                    break;
                case MyResources.valueAction3:
                    Action action3 = (Action)tagList[value];
                    action3.setItem(MyResources.valueBrosse);
                    break;
                case MyResources.valueAction4:
                    Action action4 = (Action)tagList[value];
                    action4.setItem(MyResources.valueBrosse);
                    break;
                case MyResources.valueAction5:
                    Action action5 = (Action)tagList[value];
                    action5.setItem(MyResources.valueVerre);
                    break;
                case MyResources.valueAction6:
                    Action action6 = (Action)tagList[value];
                    action6.setItem(MyResources.valueVerre);
                    break;
                default: break;
            }
        }

        void Window1_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void tagAddedFrieze(object sender, TagVisualizerEventArgs e)
        {
            string tagChoose = ((TagVisualizer)sender).Name; //Choper le name de la frieze
//            Console.WriteLine("AddFrieze");
 //           Console.WriteLine("On veut le nom : " + tagChoose);
            Action action = (Action)tagList[e.TagVisualization.VisualizedTag.Value];//On choppe l'action 

            if (linksFrieze[action.getValue()].Item1 == tagChoose || linksFrieze[action.getValue()].Item2 == tagChoose )
            {
                action.setPutInRightCase(true);
 //               Console.WriteLine("Color line");
                linksBorder[tagChoose].BorderBrush = colorValidationObjects;
            }
            else
            {
                action.setPutInRightCase(false);
                linksBorder[tagChoose].BorderBrush = colorInValidationObjects;
            }

            friezesCompletes();
        }

        private void tagRemovedFrieze(object sender, TagVisualizerEventArgs e)
        {
            Action action = (Action)tagList[e.TagVisualization.VisualizedTag.Value];//On choppe l'action
            action.setPutInRightCase(false);

            string tagChoose = ((TagVisualizer)sender).Name; //Choper le name de la frieze
            linksBorder[tagChoose].BorderBrush = Brushes.Transparent;
        }

       private void friezesCompletes()
        {
           bool complete = true;
           int allActions = 0;
 
           foreach(KeyValuePair<long, Tag> tag in tagList)
           {
               if(tag.GetType() == typeof(Action))// Si on a une action
               {
                   Action action = (Action) tag.Value;
                   allActions ++;
                   if (!action.getPutInRightCase())
                   {
                       complete = false;
                       break;
                   }
               }
           }
           if(complete && allActions == MyResources.nbActions)
           {
               ordonnancement.Visibility = Visibility.Hidden;
               video.Visibility = Visibility.Visible;
           }
        }

       // Play the media.
       void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
       {

           // The Play method will begin the media if it is not currently active or 
           // resume media if it is paused. This has no effect if the media is
           // already running.
           videoTop.Play();

           // Initialize the MediaElement property values.
           InitializePropertyValues();

       }

       // Pause the media.
       void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
       {

           // The Pause method pauses the media if it is currently running.
           // The Play method can be used to resume.
           videoTop.Pause();

       }

       // Stop the media.
       void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
       {

           // The Stop method stops and resets the media to be played from
           // the beginning.
           videoTop.Stop();

       }

       // Change the volume of the media.
       private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
       {
           videoTop.Volume = (double)volumeSlider.Value;
       }

       // Change the speed of the media.
       private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
       {
           videoTop.SpeedRatio = (double)speedRatioSlider.Value;
       }

       // When the media opens, initialize the "Seek To" slider maximum value
       // to the total number of miliseconds in the length of the media clip.
       private void Element_MediaOpened(object sender, EventArgs e)
       {
           timelineSlider.Maximum = videoTop.NaturalDuration.TimeSpan.TotalMilliseconds;
       }

       // When the media playback is finished. Stop() the media to seek to media start.
       private void Element_MediaEnded(object sender, EventArgs e)
       {
           videoTop.Stop();
       }

       // Jump to different parts of the media (seek to). 
       private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
       {
           int SliderValue = (int)timelineSlider.Value;

           // Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
           // Create a TimeSpan with miliseconds equal to the slider value.
           TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
           videoTop.Position = ts;
       }

       void InitializePropertyValues()
       {
           // Set the media's starting Volume and SpeedRatio to the current value of the
           // their respective slider controls.
           videoTop.Volume = (double)volumeSlider.Value;
           videoTop.SpeedRatio = (double)speedRatioSlider.Value;
       }
    }
}

