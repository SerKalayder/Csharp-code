using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GC_comparison_wpf.GC;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace GC_comparison_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public Grid heapGrid { get; set; }
        public int dimension { get; set; }
        public MockHeap heap { get; set; }
        public Rectangle[,] rects { get; set; }
        public int objectsAmount { get; set; }
        public TimeSpan collctorSimulationTime { get; set; }
        public List<double> collectorTimeIndicators { get; set; }

        public MainWindow2()
        {
            InitializeComponent();            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // message box (left bottom)
            message.Text = "Симуляція в процесі, очікуйте ";
            messageBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFE8D5"));
            messageBorder.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE2C8B3"));

            collectorWorkTime.Text = "0";
            mutatorWorkTime.Text = "0";
            totalWorkTime.Text = "0";
            avarageWorkTime.Text = "0";
            currentTest.Text = "0";
            ComboBoxItem cbItem = (ComboBoxItem)testsAmount.SelectedItem;
            int times = Convert.ToInt32(cbItem.Content.ToString());

            collectorTimeIndicators = new List<double>();
            
            int m, s, ms;
            double time, sum, avarage; ;

            for (int i = 1; i <= times; i++)
            {
                currentTest.Text = Convert.ToString(i);
                MakeSimulation();
                time = collctorSimulationTime.TotalMilliseconds;
                collectorTimeIndicators.Add(time);
                sum = collectorAllTestsTime();
                avarage = sum / i;
                TimeSpan ts = TimeSpan.FromMilliseconds(avarage);
                avarageWorkTime.Text = String.Format("{0}:{1}:{2}", ts.Minutes, ts.Seconds, ts.Milliseconds);
            }

            // message box (left bottom)
            message.Text = "Симуляцію завершено успішно";
            messageBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCDF1D2"));
            messageBorder.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF2F9750"));
        }

        public double collectorAllTestsTime()
        {
            double total = 0;
            foreach (double t in collectorTimeIndicators)
            {
                total += t;
            }
            return total;
        }

        public void MakeSimulation()
        {
            Stopwatch mutatorStopwatch = new Stopwatch();
            TimeSpan mutatorTime = new TimeSpan(0, 0, 0);
            Stopwatch collectorStopwatch = new Stopwatch();
            TimeSpan collectorTime = new TimeSpan(0, 0, 0);
            TimeSpan totalTime = new TimeSpan(0, 0, 0);

            int m = 0;
            int s = 0;
            int ms = 0;

            collectorTime = new TimeSpan(0, 0, m, s, ms);

            collectorWorkTime.Text = String.Format("{0}:{1}:{2}",
                collectorTime.Minutes, collectorTime.Seconds, collectorTime.Milliseconds);

            GarbageCollector collector = new GarbageCollector();

            if (radioButton.IsChecked == true)
                collector = new MarkSweepCollector();
            if (radioButton1.IsChecked == true)
                collector = new CopyingCollector();
            if (radioButton2.IsChecked == true)
                collector = new ReferenceCountingCollector();

            heap = new MockHeap(dimension);

            int objsCount = objectsAmount;

            // mutator stopwatch
            mutatorStopwatch.Start();

            MockObject mo = null;
            MarkSweepCollector msc = new MarkSweepCollector();
            do
            {
                if (objsCount != 0)
                {
                    int i = 0;
                    while (i < 5)
                    {
                        mo = new MockObject(objsCount);
                        objsCount--;
                        heap = AllocateObject(mo, heap);
                        i++;
                        Thread.Sleep(50);
                        ReDraw();
                        objectsInMemory.Text = "" + heap.ObjectsInMemory();
                        PrintToConsole();
                    }
                }
                heap = MutateHeap(heap);
                PrintToConsole();

                // mutaotr time stop
                mutatorStopwatch.Stop();
                m = mutatorStopwatch.Elapsed.Minutes + mutatorTime.Minutes;
                s = mutatorStopwatch.Elapsed.Seconds + mutatorTime.Seconds;
                ms = mutatorStopwatch.Elapsed.Milliseconds + mutatorTime.Milliseconds;

                mutatorTime = new TimeSpan(0, 0, m, s, ms);

                mutatorWorkTime.Text = String.Format("{0}:{1}:{2}", m, s, ms);
                // total time stop
                m = totalTime.Minutes + mutatorStopwatch.Elapsed.Minutes;
                s = totalTime.Seconds + mutatorStopwatch.Elapsed.Seconds;
                ms = totalTime.Milliseconds + mutatorStopwatch.Elapsed.Milliseconds;

                totalTime = new TimeSpan(0, 0, m, s, ms);

                totalWorkTime.Text = String.Format("{0}:{1}:{2}", m, s, ms);

                collectorStopwatch.Start();
                heap = collector.CollectGarbage(heap);

                // collector time stop
                collectorStopwatch.Stop();
                m = collectorStopwatch.Elapsed.Minutes + collectorTime.Minutes;
                s = collectorStopwatch.Elapsed.Seconds + collectorTime.Seconds;
                ms = collectorStopwatch.Elapsed.Milliseconds + collectorTime.Milliseconds;

                collectorTime = new TimeSpan(0, 0, m, s, ms);

                collectorWorkTime.Text = String.Format("{0}:{1}:{2}",
                    collectorTime.Minutes, collectorTime.Seconds, collectorTime.Milliseconds);

                // total time stop
                m = totalTime.Minutes + collectorStopwatch.Elapsed.Minutes;
                s = totalTime.Seconds + collectorStopwatch.Elapsed.Seconds;
                ms = totalTime.Milliseconds + collectorStopwatch.Elapsed.Milliseconds;

                totalTime = new TimeSpan(0, 0, m, s, ms);

                totalWorkTime.Text = String.Format("{0}:{1}:{2}", m, s, ms);


                PrintToConsole();
                ReDraw();
                objectsInMemory.Text = "" + heap.ObjectsInMemory();

            } while (!heap.IsEmpty());

            collctorSimulationTime = collectorTime;
        }


        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // read selected amount of objects
            ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
            objectsAmount = Convert.ToInt32(cbItem.Content.ToString());

            switch (objectsAmount)
            {
                case 50:
                    dimension = 10; break;
                case 100:
                    dimension = 15; break;
                case 500:
                    dimension = 25; break;
                case 1000:
                    dimension = 35; break;
            }

            // create new heap grid
            heapGrid = new Grid();
            heapGrid.Width = Double.NaN;
            heapGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            heapGrid.VerticalAlignment = VerticalAlignment.Stretch;
            heapGrid.ShowGridLines = false;
            heapGrid.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF292929"));

            for (int rowNumber = 0; rowNumber < dimension; rowNumber++)
            {
                var row = new RowDefinition();
                heapGrid.RowDefinitions.Add(row);
            }
            for (int columnNumber = 0; columnNumber < dimension; columnNumber++)
            {
                var column = new ColumnDefinition();
                heapGrid.ColumnDefinitions.Add(column);
            }

            Grid.SetRow(heapGrid, 0);
            Grid.SetColumn(heapGrid, 1);
            mainGrid.Children.Add(heapGrid);

            rects = new Rectangle[dimension, dimension];
            InitGrid();
        }

        public void ReDraw()
        {
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                {
                    if (heap.Storage[i, j] != null)
                        rects[i, j].Fill = Brushes.White;
                    else
                        rects[i, j].Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF292929"));

                    heapGrid.Refresh();
                }
        }

        public void InitGrid()
        {
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                {
                    // Add a Rectangle Element
                    Rectangle myRect = new Rectangle();
                    myRect.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF292929"));
                    myRect.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF292929"));
                    myRect.HorizontalAlignment = HorizontalAlignment.Left;
                    myRect.VerticalAlignment = VerticalAlignment.Center;
                    myRect.Stretch = Stretch.UniformToFill;
                    Grid.SetRow(myRect, i);
                    Grid.SetColumn(myRect, j);
                    heapGrid.Children.Add(myRect);

                    rects[i, j] = myRect;
                    heapGrid.Refresh();
                }
        }

        public void PrintToConsole()
        {
            Console.WriteLine("****************** REDRAWN");
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    string s = (heap.Storage[i, j] != null) ? "   " 
                        + heap.Storage[i, j].ActiveReferences : "null";
                    Console.Write(s + " ");
                    //Convert.ToString(h.Storage[i, j].Id) 
                }
                Console.WriteLine();
            }
            Console.WriteLine("****************** END");
            Console.WriteLine("****************** END");
        }

        public MockHeap AllocateObject(MockObject o, MockHeap h)
        {
            h.Allocate(o);
            return h;
        }

        public MockHeap MutateHeap(MockHeap h)
        {
            if (!h.IsEmpty())
                Mutator.Do(h);
            return h;
        }

    }

    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

    }
}
