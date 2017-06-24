﻿using System;
using System.Threading.Tasks;
using Windows.Storage;
using Xunit;

namespace LibretroRT.Test
{
    public abstract class TestBase
    {
        protected string RomPath { get; private set; }

        protected ICore Target { get; private set; }

        protected TestBase(Func<ICore> coreInstancer, string romPath)
        {
            RomPath = romPath;
            Target = coreInstancer();
        }

        [Fact]
        public async Task CoreInfoIsRead()
        {
            Assert.NotNull(Target.Name);
            Assert.NotNull(Target.Version);
            Assert.NotNull(Target.SupportedExtensions);

            Assert.NotNull(Target.Geometry);
            Assert.NotNull(Target.Timing);

            await Task.Delay(50);
            Assert.NotNull(Target.SystemFolder);
            Assert.NotEmpty(Target.SystemFolder.Path);
            Assert.NotNull(Target.SaveGameFolder);
            Assert.NotEmpty(Target.SaveGameFolder.Path);

            Assert.NotNull(Target.FileDependencies);
            foreach(var i in Target.FileDependencies)
            {
                Assert.False(string.IsNullOrEmpty(i.Description) || string.IsNullOrWhiteSpace(i.Description));
                Assert.False(string.IsNullOrEmpty(i.Name) || string.IsNullOrWhiteSpace(i.Name));
                Assert.Equal(32, i.MD5.Length);
            }
        }

        [Fact]
        public async Task LoadingRomWorks()
        {
            var romsFolder = await GetRomsFolderAsync();
            var provider = new StreamProvider(romsFolder);
            Target.OpenFileStream = provider.OpenFileStream;
            Target.CloseFileStream = provider.CloseFileStream;

            var loadResult = await Task.Run(() => Target.LoadGame(RomPath));

            Assert.True(loadResult);
            Assert.NotEqual(PixelFormats.FormatUknown, Target.PixelFormat);

            var geometry = Target.Geometry;
            Assert.NotEqual(0, geometry.AspectRatio);
            Assert.NotEqual(0U, geometry.BaseWidth);
            Assert.NotEqual(0U, geometry.BaseHeight);
            Assert.NotEqual(0U, geometry.MaxHeight);
            Assert.NotEqual(0U, geometry.MaxWidth);

            var timing = Target.Timing;
            Assert.NotEqual(0, timing.FPS);
            Assert.NotEqual(0, timing.SampleRate);
        }

        [Fact]
        public async Task ExecutionWorks()
        {
            var pollInputCalled = false;
            Target.PollInput = () => pollInputCalled = true;

            var getInputStateCalled = false;
            Target.GetInputState += (d, e) =>
            {
                getInputStateCalled = true;
                return 0;
            };

            var renderVideoFrameCalled = false;
            Target.RenderVideoFrame = (d, e, f, g) =>
            {
                Assert.NotEmpty(d);
                Assert.True(e > 0);
                Assert.True(f > 0);
                Assert.True(g > 0);
                renderVideoFrameCalled = true;
            };

            var renderAudioFrameCalled = false;
            Target.RenderAudioFrames = (d) =>
            {
                Assert.NotEmpty(d);
                Assert.True(d.Length % 2 == 0);
                renderAudioFrameCalled = true;
            };

            var romsFolder = await GetRomsFolderAsync();
            var provider = new StreamProvider(romsFolder);
            Target.OpenFileStream = provider.OpenFileStream;
            Target.CloseFileStream = provider.CloseFileStream;

            var loadResult = await Task.Run(() => Target.LoadGame(RomPath));
            Assert.True(loadResult);

            await Task.Run(() =>
            {
                Target.RunFrame();
            });

            Assert.True(pollInputCalled);
            Assert.True(getInputStateCalled);
            Assert.True(renderVideoFrameCalled);
            Assert.True(renderAudioFrameCalled);
        }

        private Task<StorageFolder> GetRomsFolderAsync()
        {
            var installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            return installedLocation.GetFolderAsync("Roms").AsTask();
        }
    }
}
