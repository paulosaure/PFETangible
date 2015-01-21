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

        private int nbMinTagToSwitch = 0; //permet de vérifier que les objets sont tous bien placés à partir de 9 objets posés. Ca évite des calcules supplémentaire.

        private Color colorLink = Colors.LightGreen;
        private SolidColorBrush colorValidationObjects = Brushes.LightGreen;
        private SolidColorBrush colorInValidationObjects = Brushes.Red;

        Dictionary<long, Tag> tagList = new Dictionary<long, Tag>(); //Objet + action
        Dictionary<Tuple<Action, Item>, Line> links = new Dictionary<Tuple<Action, Item>, Line>();
        Dictionary<long, Tuple<string, string>> linksFrieze = new Dictionary<long, Tuple<string, string>>();
        Dictionary<string, Border> linksBorder = new Dictionary<string, Border>();
        Dictionary<long, string> linksActionsVideos = new Dictionary<long, string>();
        List<Tuple<long, long>> listeActionsAssociees = new List<Tuple<long, long>>();

        private MediaPlayer son = new MediaPlayer();

        public SurfaceWindow1()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(Window1_Closing);

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            initValue();
        }

        public void initValue()
        {
            //Lier le XAML avec les constantes
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


            //Lier les actions avec les 2 cases correspondantes dans une frise
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


            //Lier les bords d'une image avec la frise
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

            //Lier les actions avec les vidéo
            linksActionsVideos.Add(MyResources.valueAction1, MyResources.videoAction1);
            linksActionsVideos.Add(MyResources.valueAction2, MyResources.videoAction2);
            linksActionsVideos.Add(MyResources.valueAction3, MyResources.videoAction3);
            linksActionsVideos.Add(MyResources.valueAction4, MyResources.videoAction4);
            linksActionsVideos.Add(MyResources.valueAction5, MyResources.videoAction5);
            linksActionsVideos.Add(MyResources.valueAction6, MyResources.videoAction6);
            linksActionsVideos.Add(MyResources.valueAction1b, MyResources.videoAction1);
            linksActionsVideos.Add(MyResources.valueAction2b, MyResources.videoAction2);
            linksActionsVideos.Add(MyResources.valueAction3b, MyResources.videoAction3);
            linksActionsVideos.Add(MyResources.valueAction4b, MyResources.videoAction4);
            linksActionsVideos.Add(MyResources.valueAction5b, MyResources.videoAction5);
            linksActionsVideos.Add(MyResources.valueAction6b, MyResources.videoAction6);

            //Associer les actions
            listeActionsAssociees.Add(new Tuple<long, long>(MyResources.valueAction1, MyResources.valueAction1b));
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

            nbMinTagToSwitch++;

            long value = e.TagVisualization.VisualizedTag.Value;
            Point pt = calculPoint(e);

            if (!tagList.ContainsKey(value))
            {
                if (value == MyResources.valueBrosse || value == MyResources.valueDenti || value == MyResources.valueVerre)
                {
                    tagList.Add(value, new Item(value, pt));
                }
                else
                {
                    tagList.Add(value, new Action(value, pt));
                }
                association(value);
            }

            tagList[value].setPutInFrise(false);
            tagList[value].setPosition(pt);
            tagList[value].setPut(true);

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
            if (nbMinTagToSwitch == MyResources.nbTag)
                switchViewOrdonnancement();
        }

        public void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            nbMinTagToSwitch--;
            long value = e.TagVisualization.VisualizedTag.Value;
            tagList[value].setPut(false);
            tagList[value].setPosition(new Point());
            switch (value)
            {
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
                    borderVerre2.BorderBrush = Brushes.Transparent;
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

            if (tag.GetType() == typeof(Item))
            {
                Item item = (Item)tagList[value];//on choppe l'action

                foreach (long action in item.getActions())
                {
                    if (tagList.ContainsKey(action)) //Si l'action existe
                    {
                        if (tagList[action].getPut() && !((Action)tagList[action]).getPutInFrise()) //Si l'action est posé et pas dans la frise
                        {
                            Tuple<Action, Item> tmp = new Tuple<Action, Item>((Action)tagList[action], item);
                            try
                            {
                                Line line = links[tmp];
                                line.X1 = tagList[action].getPosition().X;
                                line.Y1 = tagList[action].getPosition().Y;
                                line.X2 = item.getPosition().X;
                                line.Y2 = item.getPosition().Y;
                            }
                            catch (System.Collections.Generic.KeyNotFoundException err)
                            {
                                Console.WriteLine("Erreur : " + err);
                            }
                        }
                    }
                }
            }
            else // SI CEST l'action QUI BOUGE
            {
                Action action = (Action)tagList[value];//on choppe l'action

                if (tagList.ContainsKey(action.getItem()))
                {
                    if (tagList[action.getItem()].getPut() && !tagList[action.getItem()].getPutInFrise())
                    {
                        try
                        {
                            Item item = (Item)tagList[action.getItem()]; //on récupère l'objet dans la liste
                            Tuple<Action, Item> tmp = new Tuple<Action, Item>(action, item);
                            Line line = links[tmp];
                            line.X1 = action.getPosition().X;
                            line.Y1 = action.getPosition().Y;
                            line.X2 = item.getPosition().X;
                            line.Y2 = item.getPosition().Y;
                        }
                        catch (System.Collections.Generic.KeyNotFoundException err)
                        {
                            Console.WriteLine("Erreur : " + err);
                        }
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
                    consigneTop.Text = MyResources.consigne;
                    consigneBot.Text = MyResources.consigne;
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

            foreach (long action in item.getActions())// Pour chaque actions associées à l'objet
            {
                if (tagList.ContainsKey(action))// Si l'action existe
                {
                    if (tagList[action].getPut() && !((Action)tagList[action]).getPutInFrise())//Si l'action est posée
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
            myLine.StrokeThickness = MyResources.largueurTrait;
            myLine.Stroke = greenBrush;
            return myLine;
        }

        private void removeObject(long value)
        {
            Item item = (Item)tagList[value]; //on choppe l'objet

            foreach (long action in item.getActions()) // Pour chaque actions associées à l'objet
            {
                Console.WriteLine("foreach");
                if (tagList.ContainsKey(action)) //Si l'action existe
                {
                    Console.WriteLine("IF 1");
                    if (tagList[action].getPut()) //Si l'action est posé
                    {
                        Console.WriteLine("IF 2");
                        Tuple<Action, Item> tmp = new Tuple<Action, Item>((Action)tagList[action], item);
                        if (links.ContainsKey(tmp))
                        {
                            Console.WriteLine("IF 3");
                            objet.Children.Remove(links[tmp]);//Clé pas disponible ... A check
                            links.Remove(tmp);
                        }
                    }
                }
            }
        }

        private void removeAction(long value)
        {
            Action action = (Action)tagList[value];//on choppe l'action

            if (tagList.ContainsKey(action.getItem())) // Si l'objet associé existe
            {
                if (tagList[action.getItem()].getPut()) // Si l'objet associé est posé sur la table
                {
                    Item item = (Item)tagList[action.getItem()]; //on récupère l'objet dans la liste
                    Tuple<Action, Item> tmp = new Tuple<Action, Item>(action, item);
                    if (links.ContainsKey(tmp))
                    {
                        objet.Children.Remove(links[tmp]);
                        links.Remove(tmp);
                    }
                }
            }
        }

        public void switchViewOrdonnancement()//Allez à la prochaine vue
        {
            bool allPut = true;

            foreach (Tuple<long, long> pair in listeActionsAssociees)
            {
                if (!checkAnAction(pair.Item1, pair.Item2))
                {
                    allPut = false;
                    break;
                }
            }

            if (allPut)
            {
                switchToOrdonnancement();
            }
        }

        public bool checkAnAction(long action, long actionb)
        {
            if (tagList.ContainsKey(action))
            {
                if (tagList[action].getPut())
                {
                    return true;
                }
                else if (tagList.ContainsKey(actionb))
                {
                    if (tagList[actionb].getPut())
                    {
                        return true;
                    }
                }
            }
            else if (tagList.ContainsKey(actionb))
            {
                if (tagList[actionb].getPut())
                {
                    return true;
                }
            }
            return false;
        }

        public void switchToOrdonnancement()
        {
            ordonnancement.Visibility = Visibility.Visible;
            consigneTop.Text = MyResources.consigne2;
            consigneBot.Text = MyResources.consigne2;
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
                    brosse.addAction(MyResources.valueAction1b);
                    brosse.addAction(MyResources.valueAction3b);
                    brosse.addAction(MyResources.valueAction4b);
                    break;
                case MyResources.valueDenti:
                    Item denti = (Item)tagList[value];
                    denti.addAction(MyResources.valueAction2);
                    denti.addAction(MyResources.valueAction2b);
                    break;
                case MyResources.valueVerre:
                    Item verre = (Item)tagList[value];
                    verre.addAction(MyResources.valueAction5);
                    verre.addAction(MyResources.valueAction6);
                    verre.addAction(MyResources.valueAction5b);
                    verre.addAction(MyResources.valueAction6b);
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
                case MyResources.valueAction1b:
                    Action action7 = (Action)tagList[value];
                    action7.setItem(MyResources.valueBrosse);
                    break;
                case MyResources.valueAction2b:
                    Action action8 = (Action)tagList[value];
                    action8.setItem(MyResources.valueDenti);
                    break;
                case MyResources.valueAction3b:
                    Action action9 = (Action)tagList[value];
                    action9.setItem(MyResources.valueBrosse);
                    break;
                case MyResources.valueAction4b:
                    Action action10 = (Action)tagList[value];
                    action10.setItem(MyResources.valueBrosse);
                    break;
                case MyResources.valueAction5b:
                    Action action11 = (Action)tagList[value];
                    action11.setItem(MyResources.valueVerre);
                    break;
                case MyResources.valueAction6b:
                    Action action12 = (Action)tagList[value];
                    action12.setItem(MyResources.valueVerre);
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
            long value = e.TagVisualization.VisualizedTag.Value;

            if (!tagList.ContainsKey(value))
            {
                Action action = new Action(value, new Point());
                tagList.Add(value, action);
                action.setPutInFrise(true);
                association(value);
            }


            tagList[value].setPut(true);

            if (tagList[value].GetType() == typeof(Action))
            {
                nbMinTagToSwitch++;
                Action action = (Action)tagList[value];
                string tagChoose = ((TagVisualizer)sender).Name; //Choper le name de la frieze
                action.setPutInFrise(true);
                Console.WriteLine("TRUE");
                try
                {
                    if (linksFrieze[action.getValue()].Item1 == tagChoose || linksFrieze[action.getValue()].Item2 == tagChoose)
                    {
                        action.setPutInRightCase(true);
                        linksBorder[tagChoose].BorderBrush = colorValidationObjects;
                    }
                    else
                    {
                        action.setPutInRightCase(false);
                        linksBorder[tagChoose].BorderBrush = colorInValidationObjects;
                    }
                    friezesCompletes();
                }
                catch (System.Collections.Generic.KeyNotFoundException err)
                {
                    Console.WriteLine("Erreur : " + err);
                }
            }
            else
            {
                ((Item)tagList[value]).setPutInFrise(true);
            }
        }

        private void tagRemovedFrieze(object sender, TagVisualizerEventArgs e)
        {
            long value = e.TagVisualization.VisualizedTag.Value;


            if (tagList[value].GetType() == typeof(Action))
            {
                nbMinTagToSwitch--;
                Action action = (Action)tagList[value];//On choppe l'action
                action.setPutInFrise(false);
                action.setPut(false);
                Console.WriteLine("FALSE");
                action.setPutInRightCase(false);
                linksBorder[((TagVisualizer)sender).Name].BorderBrush = Brushes.Transparent;
            }
        }

        private void tagMovedFrieze(object sender, TagVisualizerEventArgs e)
        {

        }

        private void friezesCompletes()
        {
            /*  bool complete = true;
              int allActions = 0;

              foreach (KeyValuePair<long, Tag> tag in tagList)
              {
                  if (tag.Value.GetType() == typeof(Action))// Si on a une action
                  {
                      Action action = (Action)tag.Value;
                      allActions++;
                      if (!action.getPutInRightCase())
                      {
                          complete = false;
                          break;
                      }
                  }
              }*/
            // if (complete && allActions == MyResources.nbActions)
            try
            {

                if (((Action)tagList[MyResources.valueAction1]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction1]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction2]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction3]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction4]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction5]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction6]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction1b]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction2b]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction3b]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction4b]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction5b]).getPutInRightCase()
                    && ((Action)tagList[MyResources.valueAction6b]).getPutInRightCase()
                    )
                {
                    consigneBot.Visibility = Visibility.Hidden;
                    consigneTop.Visibility = Visibility.Hidden;
                    BorderTagActions.BorderBrush = Brushes.Transparent;
                    ordonnancement.Visibility = Visibility.Hidden;
                    video.Visibility = Visibility.Visible;
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException err)
            {
                Console.WriteLine("Erreur : " + err);
            }
        }

        public void putActionOn(object sender, TagVisualizerEventArgs e)
        {
            long action = e.TagVisualization.VisualizedTag.Value;
            //      videoBot.Source = new Uri (linksActionsVideos[action]);
            //     videoTop.Source = new Uri (linksActionsVideos[action]);
            videoBot.Source = new Uri("Resources/videoBrossage.mp4", UriKind.Relative);
            videoTop.Source = new Uri("Resources/videoBrossage.mp4", UriKind.Relative);

            videoBot.Play();
            videoTop.Play();

        }

        public void putActionOff(object sender, TagVisualizerEventArgs e)
        {
            videoBot.Stop();
            videoTop.Stop();
        }
    }
}

