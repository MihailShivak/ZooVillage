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
            // Убедимся, что инициализация происходит в UI потоке
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(InitializeAudio);
            }
            else
            {
                InitializeAudio();
            }
        }

        private void InitializeAudio()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"📂 Базовая директория: {AppDomain.CurrentDomain.BaseDirectory}");
                System.Diagnostics.Debug.WriteLine($"📂 Ожидаемый путь: {SoundsPath}");

                _backgroundMusic = new MediaPlayer();
                _backgroundMusic.Volume = _volume;

                // Подписываемся на событие завершения для зацикливания
                _backgroundMusic.MediaEnded += (s, e) =>
                {
                    if (_isMusicPlaying)
                    {
                        _backgroundMusic.Position = TimeSpan.Zero;
                        _backgroundMusic.Play();
                        System.Diagnostics.Debug.WriteLine("🔄 Музыка перезапущена (зацикл)");
                    }
                };

                // Проверяем несколько возможных путей
                var possiblePaths = new[]
                {
                    Path.Combine(SoundsPath, "mz.mp3"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds", "mz.mp3"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Debug", "net9.0-windows", "Assets", "Sounds", "mz.mp3"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Release", "net9.0-windows", "Assets", "Sounds", "mz.mp3")
                };

                foreach (var path in possiblePaths)
                {
                    System.Diagnostics.Debug.WriteLine($"🔍 Проверка: {path} - {(File.Exists(path) ? "✓ найден" : "✗ не найден")}");

                    if (File.Exists(path))
                    {
                        try
                        {
                            var fullPath = Path.GetFullPath(path);
                            var fileInfo = new FileInfo(fullPath);
                            System.Diagnostics.Debug.WriteLine($"   Размер: {fileInfo.Length / 1024} КБ");

                            _musicPath = new Uri(fullPath, UriKind.Absolute).ToString();
                            _backgroundMusic.Open(new Uri(_musicPath));
                            _musicLoaded = true;

                            System.Diagnostics.Debug.WriteLine($"✓ Музыка загружена из: {path}");
                            System.Diagnostics.Debug.WriteLine($"✓ URI: {_musicPath}");
                            return;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"❌ Ошибка открытия файла {path}: {ex.Message}");
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"⚠ Музыка не найдена ни в одном из проверяемых путей");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка при инициализации звука: {ex.Message}\n{ex.StackTrace}");
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
