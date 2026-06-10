using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ZooVillage.Services
{
    /// <summary>
    /// Менеджер для управления звуками и музыкой в приложении
    /// </summary>
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
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка при создании AudioManager: {ex.Message}");
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
                        System.Diagnostics.Debug.WriteLine($"✓ Музыка загружена: {musicFile}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Ошибка открытия музыки: {ex.Message}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"⚠ Файл музыки не найден: {musicFile}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка инициализации звука: {ex.Message}");
            }
        }

        /// <summary>
        /// Запустить фоновую музыку (зацикленно)
        /// </summary>
        public void PlayBackgroundMusic()
        {
            try
            {
                if (_backgroundMusic == null)
                {
                    System.Diagnostics.Debug.WriteLine("⚠ AudioManager не инициализирован");
                    return;
                }

                if (!_musicLoaded)
                {
                    System.Diagnostics.Debug.WriteLine("⚠ Музыка не загружена");
                    return;
                }

                if (!_isMusicPlaying)
                {
                    _backgroundMusic.Play();
                    _isMusicPlaying = true;
                    System.Diagnostics.Debug.WriteLine("▶️ Фоновая музыка запущена");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка при воспроизведении музыки: {ex.Message}");
            }
        }

        /// <summary>
        /// Остановить музыку
        /// </summary>
        public void StopMusic()
        {
            try
            {
                if (_backgroundMusic != null && _isMusicPlaying)
                {
                    _backgroundMusic.Stop();
                    _isMusicPlaying = false;
                    System.Diagnostics.Debug.WriteLine("⏹️ Музыка остановлена");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка при остановке музыки: {ex.Message}");
            }
        }

        /// <summary>
        /// Переключить музыку (вкл/выкл)
        /// </summary>
        public void ToggleMusic()
        {
            if (_isMusicPlaying)
                StopMusic();
            else
                PlayBackgroundMusic();
        }

        /// <summary>
        /// Воспроизвести звуковой эффект один раз
        /// </summary>
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
                    System.Diagnostics.Debug.WriteLine($"🔊 Звуковой эффект: {soundFileName}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка при воспроизведении звука '{soundFileName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Очистить ресурсы
        /// </summary>
        public void Cleanup()
        {
            StopMusic();
            _backgroundMusic?.Close();
        }
    }
}
