using DnDStoryWriterHalper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Media.Playback;

namespace DnDStoryWriterHalper.ViewModels
{
    public class AudioPlayerViewModel : ViewModelBase
    {
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
                ChangeProperty(ref _currentMusicFile, value);
            }
        }


        private Visibility _playButtonVisible;
        private Visibility _pauseButtonVisible;
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
        public AudioPlayerViewModel()
        {
            _player = new MediaPlayer();
            _player.CurrentStateChanged += _player_CurrentStateChanged;
        }

        private void _player_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if(sender.CurrentState == MediaPlayerState.Playing)
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
