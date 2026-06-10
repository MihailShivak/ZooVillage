using System;
using System.IO;
using System.Media;

namespace ZooVillage.Services
{
    /// <summary>
    /// Менеджер для управления звуками и музыкой в приложении
    /// </summary>
    public class AudioManager
    {
        private SoundPlayer _backgroundMusic;
        private bool _isMusicPlaying;
        private float _volume = 0.5f;
        private static readonly string SoundsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");

        public bool IsMusicPlaying => _isMusicPlaying;
        public float Volume
        {
            get => _volume;
            set => _volume = Math.Clamp(value, 0f, 1f);
        }

        public AudioManager()
        {
            InitializeAudio();
        }

        private void InitializeAudio()
        {
            try
            {
                // Проверяем несколько возможных путей
                var possiblePaths = new[]
                {
                    Path.Combine(SoundsPath, "mz.mp3"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Debug", "net9.0-windows", "Assets", "Sounds", "mz.mp3"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Release", "net9.0-windows", "Assets", "Sounds", "mz.mp3")
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        _backgroundMusic = new SoundPlayer(path);
                        System.Diagnostics.Debug.WriteLine($"✓ Музыка загружена из: {path}");
                        return;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"⚠ Музыка не найдена. Поиск в: {SoundsPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Ошибка при инициализации звука: {ex.Message}");
            }
        }

        /// <summary>
        /// Запустить фоновую музыку
        /// </summary>
        public void PlayBackgroundMusic()
        {
            try
            {
                if (_backgroundMusic != null && !_isMusicPlaying)
                {
                    _backgroundMusic.PlayLooping();
                    _isMusicPlaying = true;
                    System.Diagnostics.Debug.WriteLine("🎵 Фоновая музыка запущена");
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
                    System.Diagnostics.Debug.WriteLine("🔇 Музыка остановлена");
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
                    var sound = new SoundPlayer(soundPath);
                    sound.Play();
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
        public void Dispose()
        {
            StopMusic();
            _backgroundMusic?.Dispose();
        }
    }
}
