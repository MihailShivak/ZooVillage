using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ZooVillage.Services
{
    // Менеджер для управления звуками и музыкой в приложении
    public class AudioManager
    {
        private MediaPlayer _backgroundMusic;
        private bool _isMusicPlaying;
        private double _volume = 0.5;
        private string _musicPath;
        private bool _musicLoaded;
        private static readonly string SoundsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");

        public bool IsMusicPlaying => _isMusicPlaying;
        public bool MusicLoaded => _musicLoaded;
        public double Volume
        {
            get => _volume;
            set
            {
                _volume = Math.Clamp(value, 0.0, 1.0);
                if (_backgroundMusic != null)
                    _backgroundMusic.Volume = _volume;
            }
        }

        public AudioManager()
        {
            try
            {
                // Убедимся, что инициализация происходит в UI потоке
                if (Application.Current?.Dispatcher != null)
                {
                    Application.Current.Dispatcher.Invoke(InitializeAudio);
                }
                else
                {
                    InitializeAudio();
                }
            }
            catch (Exception ex)
            {
                // Ошибка при инициализации - музыка будет недоступна
            }
        }

        private void InitializeAudio()
        {
            try
            {
                _backgroundMusic = new MediaPlayer();
                _backgroundMusic.Volume = _volume;

                // Подписываемся на событие завершения для зацикливания
                _backgroundMusic.MediaEnded += (s, e) =>
                {
                    if (_isMusicPlaying && _backgroundMusic != null)
                    {
                        _backgroundMusic.Position = TimeSpan.Zero;
                        _backgroundMusic.Play();
                    }
                };

                // Прямой путь к файлу музыки
                var musicFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds", "mz.mp3");

                if (File.Exists(musicFile))
                {
                    try
                    {
                        var fullPath = Path.GetFullPath(musicFile);
                        _musicPath = new Uri(fullPath, UriKind.Absolute).ToString();
                        _backgroundMusic.Open(new Uri(_musicPath));
                        _musicLoaded = true;
                    }
                    catch (Exception ex)
                    {
                        // Ошибка при открытии файла музыки
                    }
                }
                // Если файл не найден, музыка просто не будет воспроизводиться
            }
            catch (Exception ex)
            {
                // Ошибка при инициализации аудиосистемы
            }
        }

        // Запустить фоновую музыку (зацикленно)
        public void PlayBackgroundMusic()
        {
            try
            {
                if (_backgroundMusic == null || !_musicLoaded)
                    return;

                if (!_isMusicPlaying)
                {
                    _backgroundMusic.Play();
                    _isMusicPlaying = true;
                }
            }
            catch (Exception ex)
            {
                // Ошибка при воспроизведении музыки
            }
        }

        // Остановить музыку
        public void StopMusic()
        {
            try
            {
                if (_backgroundMusic != null && _isMusicPlaying)
                {
                    _backgroundMusic.Stop();
                    _isMusicPlaying = false;
                }
            }
            catch (Exception ex)
            {
                // Ошибка при остановке музыки
            }
        }

        // Переключить музыку (вкл/выкл)
        public void ToggleMusic()
        {
            if (_isMusicPlaying)
                StopMusic();
            else
                PlayBackgroundMusic();
        }

        // Воспроизвести звуковой эффект один раз
        public void PlaySoundEffect(string soundFileName)
        {
            try
            {
                var soundPath = Path.Combine(SoundsPath, soundFileName);
                if (File.Exists(soundPath))
                {
                    var soundUri = new Uri(Path.GetFullPath(soundPath), UriKind.Absolute).ToString();
                    var soundPlayer = new MediaPlayer();
                    soundPlayer.Volume = _volume;
                    soundPlayer.Open(new Uri(soundUri));
                    soundPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                // Ошибка при воспроизведении звука
            }
        }

        // Очистить ресурсы
        public void Cleanup()
        {
            StopMusic();
            _backgroundMusic?.Close();
        }
    }
}
