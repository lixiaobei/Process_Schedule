using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace 进程调度C_sharp
{
    //转换器
    public class SimpleConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool first = (bool)values[0];
            //bool second = (bool)values[1];
            if (first) return true;
            else return false;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //初始数据容器
        ObservableCollection<PCB> items = new ObservableCollection<PCB>() {
            new PCB{ Name = "A", Atime = 0, Super = 9, Ntime = 5},
            new PCB{ Name = "B", Atime = 1, Super = 4, Ntime = 3},
            new PCB{ Name = "C", Atime = 2, Super = 10, Ntime = 2},
            new PCB{ Name = "D", Atime = 3, Super = 5, Ntime = 3},
            new PCB{ Name = "E", Atime = 4, Super = 6, Ntime = 4},
        };
        //真正处理容器
        public static List<PCB> ITEM = new List<PCB>();
        public MainWindow()
        {
            //窗体居中
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            SetBinding();
            DataGrid table = this.Table as DataGrid;
            table.ItemsSource = items;
            
        }

        //绑定优先权框
        public void SetBinding()
        {
            //准备基础绑定
            Binding b1 = new Binding("IsChecked") { Source = this.static_super };
            //Binding b2 = new Binding("IsChecked") { Source = this.dymatic_super };

            //准备MultiBinding
            MultiBinding mb = new MultiBinding() { Mode = BindingMode.OneWay };
            mb.Bindings.Add(b1);
            //mb.Bindings.Add(b2);
            mb.Converter = new SimpleConverter();
            //绑定
            this.super.SetBinding(TextBox.IsEnabledProperty, mb);
        }
        //重置按钮
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            items.Clear();
            ITEM.Clear();
            DataGrid table = this.Table as DataGrid;
            table.ItemsSource = items;
            this.super.Text = "";
            this.ntime.Text = "";
            this.atime.Text = "";
            this.pname.Text = "";
            this.time.Text = "";
            PCB.Time = 0;
            PCB.Piece = 0;
        }
        //添加按钮
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = pname.Text;
            string super = this.super.Text;
            string ntime = this.ntime.Text;
            string atime = this.atime.Text;
            string time = this.time.Text;
            Regex reg = new Regex(@"^\d+\.\d+$");
            if (reg.IsMatch(ntime) || reg.IsMatch(atime) || reg.IsMatch(time))
            {
                MessageBox.Show("输入的是小数");
                return;
            }
            if (ntime == "0")
            {
                MessageBox.Show("运行时间不准为0");
                return;
            }
            //FCFS算法
            if (this.fcfs.IsChecked == true)
            {
                if (name == "" || ntime == "" || atime == "")
                    MessageBox.Show("请不要留空");
                else Get_PCB(name, ntime, atime);
            }
            

            //简单轮转
            if (this.simple_time.IsChecked == true)
            {
                if(name == "" || ntime == "" || atime == "" || time == "")
                    MessageBox.Show("请不要留空");
                else { Get_PCB(name, ntime, atime); PCB.Piece = Convert.ToDouble(time); }
            }
            

            //静态优先级
            if(this.static_super.IsChecked == true)
            {
                if(name == "" || ntime == "" || atime == "" || super == "")
                    MessageBox.Show("请不要留空");
                else Get_PCB(name, super, ntime, atime);
            }

            //短进程
            if (this.short_first.IsChecked == true)
            {
                if (name == "" || ntime == "" || atime == "" || super == "")
                    MessageBox.Show("请不要留空");
                else Get_PCB(name, super, ntime, atime);
            }

            DataGrid table = this.Table as DataGrid;
            table.ItemsSource = items;
            this.super.Text = "";
            this.ntime.Text = "";
            this.atime.Text = "";
            this.pname.Text = "";        
        }
        //退出按钮
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }
        //开始按钮
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            int nn = items.Count();
            ITEM.Clear();
            for (int i = 0; i < nn; i++)
            {
                items[i].Rtime = 0;
                items[i].Ftime = 0;
                ITEM.Add(items[i]);
            }
            string time = this.time.Text;
            int n = ITEM.Count();
            for (int i = 0; i < n; i++)
            {
                ITEM[i].Statu = Status.Wait;
            }
            if (fcfs.IsChecked == true) PCB.FCFS(ITEM);
            else if (short_first.IsChecked == true) PCB.Short_priority(ITEM);
            else if (static_super.IsChecked == true) PCB.Static_priority(ITEM);
            else if (simple_time.IsChecked == true)
            {
                PCB.Piece = Convert.ToDouble(time);
                if (PCB.Piece <= 0)
                    MessageBox.Show("时间片必须为正整数");
                else
                    PCB.Simple_time(ITEM);
            }
            DataGrid table = this.Table as DataGrid;
            table.ItemsSource = null;
            table.ItemsSource = ITEM;
        }
        //分析按钮
        private void Analysis_Click(object sender, RoutedEventArgs e)
        {
            PCB.Analyse(ITEM);
            analyse Awin = new analyse();
            String T = "";
            //Awin.InitializeComponent();
            //FCFS算法
            if (this.fcfs.IsChecked == true) T = "FCFS";

            //简单轮转
            if (this.simple_time.IsChecked == true) T = "简单轮转";

            //静态优先级
            if (this.static_super.IsChecked == true) T = "静态优先级";

            //短进程
            if (this.short_first.IsChecked == true) T = "短进程";

            Awin.Title = T + Awin.Title;
            Awin.Show();
        }
        //初始数据状体按钮
        private void Input_Click(object sender, RoutedEventArgs e)
        {
            
            int n = items.Count();
            ITEM.Clear();
            for(int i = 0; i < n; i++)
            {
                items[i].Rtime = 0;
                items[i].Ftime = 0;
                items[i].Statu = Status.Wait;
                ITEM.Add(items[i]);
            }
            string time = this.time.Text;
            PCB.Piece = Convert.ToDouble(time);
            DataGrid table = this.Table as DataGrid;
            table.ItemsSource = items;
        }
        //无优先级
        private void Get_PCB(string name,string ntime,string atime)
        {
            PCB new_pcb = new PCB
            {
                Name = name,
                Ntime = Convert.ToDouble(ntime),
                Atime = Convert.ToDouble(atime)
            };
            items.Add(new_pcb);
            ITEM.Add(new_pcb);
            return;
        }
        //有优先级
        private void Get_PCB(string name, string super,string ntime, string atime)
        {
            PCB new_pcb = new PCB
            {
                Name = name,
                Super = Convert.ToInt16(super),
                Ntime = Convert.ToDouble(ntime),
                Atime = Convert.ToDouble(atime)
            };
            items.Add(new_pcb);
            ITEM.Add(new_pcb);
            return;
        }

        
    }
}
