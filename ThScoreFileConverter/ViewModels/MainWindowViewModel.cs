﻿//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ThScoreFileConverter.Actions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.ViewModels
{
    /// <summary>
    /// The view model class for <see cref="ThScoreFileConverter.Views.MainWindow"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "For binding.")]
    internal class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// A list of the Touhou works.
        /// </summary>
        public static readonly Work[] WorksImpl = new Work[]
        {
            new Work { Number = "TH06", Title = "東方紅魔郷", IsSupported = true },
            new Work { Number = "TH07", Title = "東方妖々夢", IsSupported = true },
            new Work { Number = "TH08", Title = "東方永夜抄", IsSupported = true },
            new Work { Number = "TH09", Title = "東方花映塚", IsSupported = true },
            new Work { Number = "TH095", Title = "東方文花帖", IsSupported = true },
            new Work { Number = "TH10", Title = "東方風神録", IsSupported = true },
            new Work { Number = "TH11", Title = "東方地霊殿", IsSupported = true },
            new Work { Number = "TH12", Title = "東方星蓮船", IsSupported = true },
            new Work { Number = "TH125", Title = "ダブルスポイラー", IsSupported = true },
            new Work { Number = "TH128", Title = "妖精大戦争", IsSupported = true },
            new Work { Number = "TH13", Title = "東方神霊廟", IsSupported = true },
            new Work { Number = "TH14", Title = "東方輝針城", IsSupported = true },
            new Work { Number = "TH143", Title = "弾幕アマノジャク", IsSupported = true },
            new Work { Number = "TH15", Title = "東方紺珠伝", IsSupported = true },
            new Work { Number = "TH16", Title = "東方天空璋", IsSupported = true },
            new Work { Number = "TH165", Title = "秘封ナイトメアダイアリー", IsSupported = true },
            new Work { Number = "TH17", Title = "東方鬼形獣", IsSupported = false },
            new Work { Number = string.Empty, Title = string.Empty, IsSupported = false },
            new Work { Number = "TH075", Title = "東方萃夢想", IsSupported = true },
            new Work { Number = "TH105", Title = "東方緋想天", IsSupported = true },
            new Work { Number = "TH123", Title = "東方非想天則", IsSupported = true },
            new Work { Number = "TH135", Title = "東方心綺楼", IsSupported = true },
            new Work { Number = "TH145", Title = "東方深秘録", IsSupported = true },
            new Work { Number = "TH155", Title = "東方憑依華", IsSupported = true },
        };

        /// <summary>
        /// The instance that executes a conversion process.
        /// </summary>
        private ThConverter converter;

        /// <summary>
        /// Indicates whether a conversion process is idle.
        /// </summary>
        private bool isIdle;

        /// <summary>
        /// A log text.
        /// </summary>
        private string log;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="dialogService">An <see cref="IDialogService"/>.</param>
        public MainWindowViewModel(IDialogService dialogService)
        {
            this.DialogService = dialogService;

            this.converter = null;

            this.Title = Assembly.GetExecutingAssembly().GetName().Name;
            this.Works = WorksImpl;

            this.SelectScoreFileCommand =
                new DelegateCommand<OpenFileDialogActionResult>(this.SelectScoreFile);
            this.SelectBestShotDirectoryCommand =
                new DelegateCommand<FolderBrowserDialogActionResult>(this.SelectBestShotDirectory);
            this.TemplateFilesSelectionChangedCommand =
                new DelegateCommand(this.OnTemplateFilesSelectionChanged);
            this.AddTemplateFilesCommand =
                new DelegateCommand<OpenFileDialogActionResult>(this.AddTemplateFiles);
            this.DeleteTemplateFilesCommand =
                new DelegateCommand<IList>(this.DeleteTemplateFiles, this.CanDeleteTemplateFiles);
            this.DeleteAllTemplateFilesCommand =
                new DelegateCommand(this.DeleteAllTemplateFiles, this.CanDeleteAllTemplateFiles);
            this.SelectOutputDirectoryCommand =
                new DelegateCommand<FolderBrowserDialogActionResult>(this.SelectOutputDirectory);
            this.ConvertCommand =
                new DelegateCommand(this.Convert, this.CanConvert);

            this.DraggingCommand =
                new DelegateCommand<DragEventArgs>(this.OnDragging);
            this.DropScoreFileCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropScoreFile);
            this.DropBestShotDirectoryCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropBestShotDirectory);
            this.DropTemplateFilesCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropTemplateFiles);
            this.DropOutputDirectoryCommand =
                new DelegateCommand<DragEventArgs>(this.OnDropOutputDirectory);

            this.OpenAboutWindowCommand = new DelegateCommand(this.OpenAboutWindow);
            this.OpenSettingWindowCommand = new DelegateCommand(this.OpenSettingWindow);

            this.PropertyChanged += this.OnPropertyChanged;

            if (string.IsNullOrEmpty(this.LastWorkNumber))
                this.LastWorkNumber = WorksImpl.First().Number;
            else
                this.RaisePropertyChanged(nameof(this.LastWorkNumber));
        }

        #region Properties to bind a view

        /// <summary>
        /// Gets a title string.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a list of the Touhou works.
        /// </summary>
        public IEnumerable<Work> Works { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the conversion process is idle.
        /// </summary>
        public bool IsIdle
        {
            get { return this.isIdle; }
            private set { this.SetProperty(ref this.isIdle, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the conversion process can handle best shot files.
        /// </summary>
        public bool CanHandleBestShot
        {
            get { return (this.converter != null) && this.converter.HasBestShotConverter; }
        }

        /// <summary>
        /// Gets a number string indicating the last selected work.
        /// </summary>
        public string LastWorkNumber
        {
            get
            {
                return Settings.Instance.LastTitle;
            }

            private set
            {
#if false
                // Note: The following occurs CS0206.
                this.SetProperty(ref Settings.Instance.LastTitle, value);
#else
                if (Settings.Instance.LastTitle != value)
                {
                    Settings.Instance.LastTitle = value;
                    if (!Settings.Instance.Dictionary.ContainsKey(value))
                        Settings.Instance.Dictionary.Add(value, new SettingsPerTitle());
                    this.RaisePropertyChanged(nameof(this.LastWorkNumber));
                }
#endif
            }
        }

        /// <summary>
        /// Gets a string indicating the supported versions of the score file to convert.
        /// </summary>
        public string SupportedVersions
        {
            get
            {
                return (this.converter != null)
                    ? Resources.strSupportedVersions + this.converter.SupportedVersions
                    : string.Empty;
            }
        }

        /// <summary>
        /// Gets a path of the score file.
        /// </summary>
        public string ScoreFile
        {
            get
            {
                return CurrentSetting.ScoreFile;
            }

            private set
            {
                if ((CurrentSetting.ScoreFile != value) && File.Exists(value))
                {
                    CurrentSetting.ScoreFile = value;
                    this.RaisePropertyChanged(nameof(this.ScoreFile));
                }
            }
        }

        /// <summary>
        /// Gets the initial directory to select a score file.
        /// </summary>
        public string OpenScoreFileDialogInitialDirectory
        {
            get
            {
                try
                {
                    return Path.GetDirectoryName(this.ScoreFile);
                }
                catch (ArgumentException)
                {
                    return string.Empty;
                }
                catch (PathTooLongException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets a path of the best shot directory.
        /// </summary>
        public string BestShotDirectory
        {
            get
            {
                return CurrentSetting.BestShotDirectory;
            }

            private set
            {
                if ((CurrentSetting.BestShotDirectory != value) && Directory.Exists(value))
                {
                    CurrentSetting.BestShotDirectory = value;
                    this.RaisePropertyChanged(nameof(this.BestShotDirectory));
                }
            }
        }

        /// <summary>
        /// Gets a list of the paths of template files.
        /// </summary>
        public IEnumerable<string> TemplateFiles
        {
            get
            {
                return CurrentSetting.TemplateFiles;
            }

            private set
            {
                CurrentSetting.TemplateFiles = value.Where(elem => File.Exists(elem)).ToArray();
                this.RaisePropertyChanged(nameof(this.TemplateFiles));
            }
        }

        /// <summary>
        /// Gets the initial directory to select template files.
        /// </summary>
        public string OpenTemplateFilesDialogInitialDirectory
        {
            get
            {
                try
                {
                    return Path.GetDirectoryName(this.TemplateFiles.LastOrDefault());
                }
                catch (ArgumentException)
                {
                    return string.Empty;
                }
                catch (PathTooLongException)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets a path of the output directory.
        /// </summary>
        public string OutputDirectory
        {
            get
            {
                return CurrentSetting.OutputDirectory;
            }

            private set
            {
                if ((CurrentSetting.OutputDirectory != value) && Directory.Exists(value))
                {
                    CurrentSetting.OutputDirectory = value;
                    this.RaisePropertyChanged(nameof(this.OutputDirectory));
                }
            }
        }

        /// <summary>
        /// Gets a name of the output directory for image files.
        /// </summary>
        public string ImageOutputDirectory
        {
            get
            {
                return CurrentSetting.ImageOutputDirectory;
            }

            private set
            {
                if (CurrentSetting.ImageOutputDirectory != value)
                {
                    CurrentSetting.ImageOutputDirectory = value;
                    this.RaisePropertyChanged(nameof(this.ImageOutputDirectory));
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the conversion process can replace spell card names.
        /// </summary>
        public bool CanReplaceCardNames
        {
            get
            {
                return (this.converter != null) && this.converter.HasCardReplacer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the conversion process hides untried cards.
        /// </summary>
        public bool HidesUntriedCards
        {
            get
            {
                return CurrentSetting.HideUntriedCards;
            }

            private set
            {
                if (CurrentSetting.HideUntriedCards != value)
                {
                    CurrentSetting.HideUntriedCards = value;
                    this.RaisePropertyChanged(nameof(this.HidesUntriedCards));
                }
            }
        }

        /// <summary>
        /// Gets a log text.
        /// </summary>
        public string Log
        {
            get { return this.log; }
            private set { this.SetProperty(ref this.log, value); }
        }

        #region Commands

        /// <summary>
        /// Gets the command to select a score file.
        /// </summary>
        public DelegateCommand<OpenFileDialogActionResult> SelectScoreFileCommand { get; private set; }

        /// <summary>
        /// Gets the command to select a best shot directory.
        /// </summary>
        public DelegateCommand<FolderBrowserDialogActionResult> SelectBestShotDirectoryCommand
        {
            get; private set;
        }

        /// <summary>
        /// Gets the command invoked when the selection of template files is changed.
        /// </summary>
        public DelegateCommand TemplateFilesSelectionChangedCommand { get; private set; }

        /// <summary>
        /// Gets the command to add some files to the list of template files.
        /// </summary>
        public DelegateCommand<OpenFileDialogActionResult> AddTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command to delete some files from the list of template files.
        /// </summary>
        public DelegateCommand<IList> DeleteTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command to delete all files from the list of template files.
        /// </summary>
        public DelegateCommand DeleteAllTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command to select an output directory.
        /// </summary>
        public DelegateCommand<FolderBrowserDialogActionResult> SelectOutputDirectoryCommand
        {
            get; private set;
        }

        /// <summary>
        /// Gets the command to convert the score file.
        /// </summary>
        public DelegateCommand ConvertCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when a dragging event is occurred on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DraggingCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when a score file is dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropScoreFileCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when a best shot directory is dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropBestShotDirectoryCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when some template files are dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropTemplateFilesCommand { get; private set; }

        /// <summary>
        /// Gets the command invoked when an output directory is dropped on a UI element.
        /// </summary>
        public DelegateCommand<DragEventArgs> DropOutputDirectoryCommand { get; private set; }

        /// <summary>
        /// Gets the command to open an about window.
        /// </summary>
        public DelegateCommand OpenAboutWindowCommand { get; private set; }

        /// <summary>
        /// Gets the command to open a setting window.
        /// </summary>
        public DelegateCommand OpenSettingWindowCommand { get; private set; }

        #endregion

        #endregion

        /// <summary>
        /// Gets the setting for the currently selected Touhou work.
        /// </summary>
        private static SettingsPerTitle CurrentSetting
        {
            get { return Settings.Instance.Dictionary[Settings.Instance.LastTitle]; }
        }

        /// <summary>
        /// Gets the <see cref="IDialogService"/>.
        /// </summary>
        private IDialogService DialogService { get; }

        /// <summary>
        /// Overrides the mouse cursor for the entire application.
        /// </summary>
        /// <param name="cursor">The new cursor or <c>null</c>.</param>
        private void OverrideCursor(Cursor cursor)
        {
            var dispatcher = App.Current.Dispatcher;
            if (dispatcher.CheckAccess())
                Mouse.OverrideCursor = cursor;
            else
                dispatcher.Invoke(() => this.OverrideCursor(cursor));
        }

        #region Methods for command implementation

        /// <summary>
        /// Selects a score file.
        /// </summary>
        /// <param name="result">A result of <see cref="OpenFileDialogAction"/>.</param>
        private void SelectScoreFile(OpenFileDialogActionResult result)
        {
            this.ScoreFile = result.FileName;
        }

        /// <summary>
        /// Selects a best shot directory.
        /// </summary>
        /// <param name="result">A result of <see cref="FolderBrowserDialogAction"/>.</param>
        private void SelectBestShotDirectory(FolderBrowserDialogActionResult result)
        {
            this.BestShotDirectory = result.SelectedPath;
        }

        /// <summary>
        /// Invoked when the selection of template files is changed.
        /// </summary>
        private void OnTemplateFilesSelectionChanged()
        {
            this.DeleteTemplateFilesCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Adds some files to the list of template files.
        /// </summary>
        /// <param name="result">A result of <see cref="OpenFileDialogAction"/>.</param>
        private void AddTemplateFiles(OpenFileDialogActionResult result)
        {
            this.TemplateFiles = this.TemplateFiles.Union(result.FileNames);
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="DeleteTemplateFiles"/> can be invoked.
        /// </summary>
        /// <param name="selectedItems">A list indicating the path strings which will be deleted.</param>
        /// <returns>
        /// <c>true</c> if <see cref="DeleteTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteTemplateFiles(IList selectedItems)
        {
            return (selectedItems != null) && (selectedItems.Count > 0);
        }

        /// <summary>
        /// Deletes some files from the list of template files.
        /// </summary>
        /// <param name="selectedItems">A list indicating the path strings which are deleted.</param>
        private void DeleteTemplateFiles(IList selectedItems)
        {
            if ((selectedItems != null) && (selectedItems.Count > 0))
                this.TemplateFiles = this.TemplateFiles.Except(selectedItems.Cast<string>());
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="DeleteAllTemplateFiles"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="DeleteAllTemplateFiles"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanDeleteAllTemplateFiles()
        {
            return this.TemplateFiles.Any();
        }

        /// <summary>
        /// Deletes all files from the list of template files.
        /// </summary>
        private void DeleteAllTemplateFiles()
        {
            this.TemplateFiles = new string[] { };
        }

        /// <summary>
        /// Selects an output directory.
        /// </summary>
        /// <param name="result">A result of <see cref="FolderBrowserDialogAction"/>.</param>
        private void SelectOutputDirectory(FolderBrowserDialogActionResult result)
        {
            this.OutputDirectory = result.SelectedPath;
        }

        /// <summary>
        /// Returns a value indicating whether <see cref="Convert"/> can be invoked.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <see cref="Convert"/> can be invoked; otherwise, <c>false</c>.
        /// </returns>
        private bool CanConvert()
        {
            return !string.IsNullOrEmpty(this.ScoreFile) &&
                   this.TemplateFiles.Any() &&
                   !string.IsNullOrEmpty(this.OutputDirectory) &&
                   !(this.CanHandleBestShot && string.IsNullOrEmpty(this.BestShotDirectory)) &&
                   !(this.CanHandleBestShot && string.IsNullOrEmpty(this.ImageOutputDirectory));
        }

        /// <summary>
        /// Converts the score file.
        /// </summary>
        private void Convert()
        {
            this.IsIdle = false;
            this.Log = Resources.msgStartConversion + Environment.NewLine;
            new Thread(new ParameterizedThreadStart(this.converter.Convert)).Start(CurrentSetting);
        }

        /// <summary>
        /// Invoked when a dragging event is occurred.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnDragging(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                this.Log += ex.Message + Environment.NewLine;
                throw;
            }
        }

        /// <summary>
        /// Invoked when a score file is dropped on a UI element.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnDropScoreFile(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        var filePath = droppedPaths.FirstOrDefault(path => File.Exists(path));
                        if (filePath != null)
                            this.ScoreFile = filePath;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log += ex.Message + Environment.NewLine;
                throw;
            }
        }

        /// <summary>
        /// Invoked when a best shot directory is dropped on a UI element.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnDropBestShotDirectory(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        var dirPath = droppedPaths.FirstOrDefault(path => Directory.Exists(path));
                        if (dirPath != null)
                            this.BestShotDirectory = dirPath;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log += ex.Message + Environment.NewLine;
                throw;
            }
        }

        /// <summary>
        /// Invoked when some template files are dropped on a UI element.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnDropTemplateFiles(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        this.TemplateFiles = this.TemplateFiles
                            .Union(droppedPaths.Where(path => File.Exists(path)));
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log += ex.Message + Environment.NewLine;
                throw;
            }
        }

        /// <summary>
        /// Invoked when an output directory is dropped on a UI element.
        /// </summary>
        /// <param name="e">The event data.</param>
        private void OnDropOutputDirectory(DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedPaths)
                    {
                        var dirPath = droppedPaths.FirstOrDefault(path => Directory.Exists(path));
                        if (dirPath != null)
                            this.OutputDirectory = dirPath;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log += ex.Message + Environment.NewLine;
                throw;
            }
        }

        /// <summary>
        /// Invoked when opening an about window is requested.
        /// </summary>
        private void OpenAboutWindow()
            => this.DialogService.ShowDialog(nameof(AboutWindowViewModel), new DialogParameters(), result => { });

        /// <summary>
        /// Invoked when opening a setting window is requested.
        /// </summary>
        private void OpenSettingWindow()
            => this.DialogService.ShowDialog(nameof(SettingWindowViewModel), new DialogParameters(), result => { });

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the event indicating a property value is changed.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsIdle":
                    this.OverrideCursor(this.IsIdle ? null : Cursors.Wait);
                    break;

                case "LastWorkNumber":
                    this.converter = ThConverterFactory.Create(Settings.Instance.LastTitle);
                    this.converter.ConvertFinished += this.OnConvertFinished;
                    this.converter.ConvertAllFinished += this.OnConvertAllFinished;
                    this.converter.ExceptionOccurred += this.OnExceptionOccurred;
                    this.IsIdle = true;
                    this.Log = string.Empty;

                    this.RaisePropertyChanged(nameof(this.SupportedVersions));
                    this.RaisePropertyChanged(nameof(this.ScoreFile));
                    this.RaisePropertyChanged(nameof(this.CanHandleBestShot));
                    this.RaisePropertyChanged(nameof(this.BestShotDirectory));
                    this.RaisePropertyChanged(nameof(this.TemplateFiles));
                    this.RaisePropertyChanged(nameof(this.OutputDirectory));
                    this.RaisePropertyChanged(nameof(this.ImageOutputDirectory));
                    this.RaisePropertyChanged(nameof(this.CanReplaceCardNames));
                    this.RaisePropertyChanged(nameof(this.HidesUntriedCards));

                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "ScoreFile":
                    this.RaisePropertyChanged(nameof(this.OpenScoreFileDialogInitialDirectory));
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "BestShotDirectory":
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "TemplateFiles":
                    this.RaisePropertyChanged(nameof(this.OpenTemplateFilesDialogInitialDirectory));
                    this.DeleteTemplateFilesCommand.RaiseCanExecuteChanged();
                    this.DeleteAllTemplateFilesCommand.RaiseCanExecuteChanged();
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "OutputDirectory":
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;

                case "ImageOutputDirectory":
                    this.ConvertCommand.RaiseCanExecuteChanged();
                    break;
            }
        }

        /// <summary>
        /// Handles the event indicating the conversion process per file has finished.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnConvertFinished(object sender, ThConverterEventArgs e)
        {
            this.Log += e.Message + Environment.NewLine;
        }

        /// <summary>
        /// Handles the event indicating the all conversion process has finished.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnConvertAllFinished(object sender, ThConverterEventArgs e)
        {
            this.Log += Resources.msgEndConversion + Environment.NewLine;
            this.IsIdle = true;
        }

        /// <summary>
        /// Handles the event indicating an exception has occurred.
        /// </summary>
        /// <param name="sender">The instance where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void OnExceptionOccurred(object sender, ExceptionOccurredEventArgs e)
        {
#if DEBUG
            this.Log += e.Exception.Message + Environment.NewLine;
#endif
            this.Log += Resources.msgErrUnhandledException + Environment.NewLine;
            this.IsIdle = true;
        }

        #endregion
    }
}
