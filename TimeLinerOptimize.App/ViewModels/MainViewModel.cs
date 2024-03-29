﻿using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TimeLinerOptimize.App.ViewModels.Base;
using TimeLinerOptimze.Core.Dtos;
using TimeLinerOptimze.Core.Helpers;
using TimeLinerOptimze.Core.Loggers;
using TimeLinerOptimze.Core.Models.Genetic;
using TimeLinerOptimze.Core.Models.TimeLiner;
using TimeLinerOptimze.Core.Repositories;
using static CSharp.Functional.Extensions.ValidationExtension;

namespace TimeLinerOptimize.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Fields

        private string _initialTimeLineFilePath;
        private string _outputDirectory;
        private string _logDirectory;
        private bool _isLog;
        private readonly Func<Option<string>> _openFileService;
        private readonly Func<Option<string>> _saveFolderDialog;
        private readonly IRepository _repository;
        private bool _isRunningGA;
        #endregion

        #region Properties

        public bool IsRunningGA
        {
            get => _isRunningGA;
            set => NotifyPropertyChanged(ref _isRunningGA, value);
        }
      

        public bool IsLog
        {
            get => _isLog;
            set => NotifyPropertyChanged(ref _isLog, value);
        }

        public string InitialTimeLinePath
        {
            get => _initialTimeLineFilePath;
            set => NotifyPropertyChanged(ref _initialTimeLineFilePath, value);
        }

        public string OutputDirectory
        {
            get => _outputDirectory;
            set => NotifyPropertyChanged(ref _outputDirectory, value);
        }

        public string LogDirectory
        {
            get => _logDirectory;
            set => NotifyPropertyChanged(ref _logDirectory, value);
        }

        public ICommand InitialTimeLineCommand { get; }
        public ICommand OutputDirectoryCommand { get; }
        public ICommand LogDirectoryCommand { get; }
        public ICommand RunGACommand { get; }

        #endregion

        #region Constuctors
        public MainViewModel(Func<Option<string>> openFileService,
                             Func<Option<string>> saveFolderDialog,
                             IRepository repository)
        {
            IsLog = true;
            InitialTimeLineCommand = new RelayCommand(OnInitialTimeLine);
            OutputDirectoryCommand = new RelayCommand(OnOutputDirectory);
            LogDirectoryCommand = new RelayCommand(OnLogDirectory);
            RunGACommand = new RelayCommand(OnRunGA, CanRunGA);
            _openFileService = openFileService;
            _saveFolderDialog = saveFolderDialog;
            _repository = repository;
            IsRunningGA = false;
        }


        #endregion

        #region Methods

        private bool CanRunGA()
        {
            if (IsLog)
            {
                var result = !(string.IsNullOrEmpty(InitialTimeLinePath) ||
                          string.IsNullOrEmpty(OutputDirectory) || string.IsNullOrEmpty(LogDirectory) ||IsRunningGA);
                return result;
            }
            else
            {
                var result = !(string.IsNullOrEmpty(InitialTimeLinePath) ||
                         string.IsNullOrEmpty(OutputDirectory) || IsRunningGA);
                return result;
            }
        }



        private async void OnRunGA()
        {
            try
            {
                IsRunningGA = true;
                var logger = IsLog ? new TextFileLogger(LogDirectory) : null;
                var task = from dtos in _repository.Read<ActivityDto>(InitialTimeLinePath)
                           from timeLines in dtos.Map(i => i.Select(dto => dto.AsActivity()).ToList().AsTimeLine()).Async()
                           from allLinesV in Task.Run(() => timeLines.Bind(timeLine => new TimeLinerGA(timeLine, new GaInput(), logger).RunGA()))
                           from bestThree in allLinesV.Map(allLines => new List<Tuple<string, TimeLine>>() { Tuple.Create("Optimized", allLines.OrderBy(l => l.TotalCost * l.TotalDuration).First()), Tuple.Create("Optimized For Cost", allLines.OrderBy(l => l.TotalCost).First()), Tuple.Create("Optimized For Duration", allLines.OrderBy(l => l.TotalDuration).First()) }).Async()
                           from result in bestThree.TraverseBind(b3 => Task.WhenAll(b3.Select(tuple => _repository.Write(tuple.Item2.Activities.Select(a => a.AsDto()).ToList(), $"{OutputDirectory}\\{tuple.Item1}.csv"))).Map(vs => vs.TraverseHarvest(v => v)))
                           select result.Match((errs) => MessageBox.Show(errs.First().Message), (_) => MessageBox.Show("Optimized TimeLines are saved successfully.")); ;
                await task;
                IsRunningGA = false;
            }
            catch (Exception e)
            {
                IsRunningGA = false;
                Debug.WriteLine(e.Message);
            }

        }

        private void OnLogDirectory()
        {
            _saveFolderDialog().Map(folder => LogDirectory = folder);
        }

        private void OnOutputDirectory()
        {
            _saveFolderDialog().Map(folder => OutputDirectory = folder);
        }

        private void OnInitialTimeLine()
        {

            _openFileService().Map(file => InitialTimeLinePath = file);

        }

        #endregion
    }
}
