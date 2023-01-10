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
            get => _musicFiles;
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

        private float _vlume;
        public float Vlume
        {
            get => _vlume;
            set => ChangeProperty(ref _vlume, value);
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
            get => _playPauseCommand;
            set => ChangeProperty(ref _playPauseCommand, value);
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
                    MusicFiles.Add(new MusicFile(r.Last()));
                }
            }

            _player = new MediaPlayer();
            _player.CurrentStateChanged += _player_CurrentStateChanged;

            PlayPauseCommand = new Command(async p =>
            {
                if(_player.CurrentState == MediaPlayerState.Playing)
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


            CurrentMusicFile = MusicFiles.Last();
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            TimeProgressPart = (float)(_player.Position /_player.NaturalDuration );
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
    }
}
