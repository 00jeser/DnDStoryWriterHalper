using DnDStoryWriterHalper.Models;
using DnDStoryWriterHalper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.Media.Playback;

namespace DnDStoryWriterHalper.ViewModels
{
    public class AudioPlayerViewModel : ViewModelBase
    {
        #region variables
        private ObservableCollection<MusicFile> _musicFiles;
        public ObservableCollection<MusicFile> MusicFiles
        {
            get => new ObservableCollection<MusicFile>(_musicFiles.Where(IsSearchMusic));
            set => ChangeProperty(ref _musicFiles, value);
        }

        private MusicFile _currentMusicFile;
        public MusicFile CurrentMusicFile
        {
            get => _currentMusicFile;
            set
            {
                _player.SetUriSource(new Uri(MusicFile.BaseFilePath + value.FileName));
                _player.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged; ;
                ChangeProperty(ref _currentMusicFile, value);
            }
        }

        private Visibility _playButtonVisible = Visibility.Visible;
        private Visibility _pauseButtonVisible = Visibility.Collapsed;
        public Visibility PlayButtonVisible
        {
            get => _playButtonVisible;
            set => ChangeProperty(ref _playButtonVisible, value);
        }
        public Visibility PauseButtonVisible
        {
            get => _pauseButtonVisible;
            set => ChangeProperty(ref _pauseButtonVisible, value);
        }

        private float _timeProgressPart;
        public float TimeProgressPart
        {
            get => _timeProgressPart;
            set => ChangeProperty(ref _timeProgressPart, value);
        }

        public float Volume
        {
            get
            {
                return (float)_player.Volume;
            }
            set
            {
                _player.Volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        private bool? _autoPlay = false;
        public bool? AutoPlay
        {
            get => _autoPlay;
            set => ChangeProperty(ref _autoPlay, value);
        }

        private List<string> _searchGenres;
        public List<string> SearchGenres
        {
            get => _searchGenres;
            set
            {
                ChangeProperty(ref _searchGenres, value);
                OnPropertyChanged(nameof(MusicFiles));
            }
        }

        private ObservableCollection<string> _allGenres;
        public ObservableCollection<string> AllGenres
        {
            get => _allGenres;
            set => ChangeProperty(ref _allGenres, value);
        }


        private MediaPlayer _player;
        public MediaPlayer Player
        {
            get => _player;
        }
        #endregion
        #region Commands
        private ICommand _playPauseCommand;
        public ICommand PlayPauseCommand
        {
            get => _playPauseCommand;
            set => ChangeProperty(ref _playPauseCommand, value);
        }
        private ICommand _playCommand;
        public ICommand PlayCommand
        {
            get => _playCommand;
            set => ChangeProperty(ref _playCommand, value);
        }
        #endregion
        public AudioPlayerViewModel()
        {
            MusicFiles = new ObservableCollection<MusicFile>();
            if (!Directory.Exists(MusicFile.BaseFilePath))
            {
                Directory.CreateDirectory(MusicFile.BaseFilePath);
            }
            foreach (var music in Directory.EnumerateFiles(MusicFile.BaseFilePath))
            {
                if (music.EndsWith(".mp3"))
                {
                    var r = music.Split('\\', '/');
                    _musicFiles.Add(new MusicFile(r.Last()));
                }
            }

            var allGenres = new List<string>();
            foreach (var mf in _musicFiles)
                if (mf.Genres != null && mf.Genres.Count > 0)
                    foreach (var genre in mf.Genres)
                        allGenres.Add(genre);
            AllGenres = new ObservableCollection<string>(allGenres);
            SearchGenres = new List<string>();

            _player = new MediaPlayer();
            _player.CurrentStateChanged += _player_CurrentStateChanged;

            PlayPauseCommand = new Command(p =>
            {
                if (_player.CurrentState == MediaPlayerState.Playing)
                {
                    _player.Pause();
                }
                else
                {
                    _player.Play();
                }
            });
            PlayCommand = new Command(a =>
            {
                if (a is MusicFile mf)
                {
                    CurrentMusicFile = mf;
                    _player.Play();
                }
            });

            OnPropertyChanged(nameof(MusicFiles));
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            TimeProgressPart = (float)(_player.Position / _player.NaturalDuration);
        }

        private void _player_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
            {
                PlayButtonVisible = Visibility.Collapsed;
                PauseButtonVisible = Visibility.Visible;
            }
            else
            {
                PlayButtonVisible = Visibility.Visible;
                PauseButtonVisible = Visibility.Collapsed;
            }
        }

        private bool IsSearchMusic(MusicFile music)
        {
            bool genreContains = false;
            if(music.Genres != null)
            foreach (var g in music.Genres)
            {
                if(SearchGenres.Contains(g))
                    genreContains= true;
            }
            if(genreContains || SearchGenres.Count == 0)
                return true;
            return false;
        }
    }
}
