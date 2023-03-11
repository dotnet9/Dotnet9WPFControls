using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Dotnet9WPFControls.Demo.ViewModels
{
    /// <summary>
    ///     扩展ObservableCollection
    /// </summary>
    public class RangeObservableCollectionViewModel : BindableBase
    {
        private ICommand? _addOneByOneDatasCommand;
        private int _addRangeCount = 10000;
        private ICommand? _addRangeDatasCommand;
        private ICommand? _clearDatasCommand;

        /// <summary>
        ///     测试添加的数据量
        /// </summary>
        public int AddRangeCount
        {
            get => _addRangeCount;
            set
            {
                _addRangeCount = value;
                SetProperty(ref _addRangeCount, value);
            }
        }

        /// <summary>
        ///     测试数据
        /// </summary>
        public RangeCollection<string> TestDatas { get; } = new();

        /// <summary>
        ///     测试记录日志
        /// </summary>
        public RangeCollection<string> TestLogs { get; } = new();

        /// <summary>
        ///     清空数据命令
        /// </summary>
        public ICommand ClearDatasCommand => _clearDatasCommand ??= new DelegateCommand(() => TestDatas.Clear());

        /// <summary>
        ///     使用Add方法循环添加数据命令
        /// </summary>
        public ICommand AddOneByOneDatasCommand =>
            _addOneByOneDatasCommand ??= new DelegateCommand(() => ExecuteAddData());

        /// <summary>
        ///     使用AddRange扩展方法指添加数据命令
        /// </summary>
        public ICommand AddRangeDatasCommand =>
            _addRangeDatasCommand ??= new DelegateCommand(() => ExecuteAddData(false));


        /// <summary>
        ///     执行添加数据操作
        /// </summary>
        /// <param name="isAddFunc">true:使用Add方法，false:使用AddRange方法</param>
        private void ExecuteAddData(bool isAddFunc = true)
        {
            IEnumerable<string> newDatas =
                Enumerable.Range(1, AddRangeCount).Select(index => DateTime.Now.ToLongTimeString());

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            if (isAddFunc)
            {
                foreach (string newData in newDatas)
                {
                    TestDatas.Add(newData);
                }
            }
            else
            {
                TestDatas.AddRange(newDatas);
            }

            sw.Stop();

            string funcName = isAddFunc ? "Add" : "AddRange";
            TestLogs.Add($"{DateTime.Now:HH:mm:ss zz} {funcName} 添加{AddRangeCount}条数据用时 {sw.ElapsedMilliseconds}毫秒");
        }
    }
}