﻿using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using Plugin.FileSystem.Abstractions;
using RetriX.Shared.Presentation;
using RetriX.Shared.Services;
using RetriX.Shared.ViewModels;

namespace RetriX.Shared
{
    public class AppStart : IMvxAppStart
    {
        private IMvxNavigationService NavigationService { get; }
        private ICurrentViewModelPresenter Presenter { get; }
        private IEmulationService EmulationService { get; }

        public AppStart(IMvxNavigationService navigationService, ICurrentViewModelPresenter presenter, IEmulationService emulationService)
        {
            NavigationService = navigationService;
            Presenter = presenter;
            EmulationService = emulationService;
        }

        public async void Start(object hint = null)
        {
            var file = hint as IFileInfo;
            if (file == null)
            {
                await NavigationService.Navigate<GameSystemSelectionViewModel>();
                return;
            }

            var param = await EmulationService.GenerateGameLaunchEnvironmentAsync(file);
            if (param != null && Presenter.CurrentViewModel is GamePlayerViewModel)
            {
                var currentGamePlayerVM = Presenter.CurrentViewModel as GamePlayerViewModel;
                currentGamePlayerVM.Prepare(param);
                await currentGamePlayerVM.Initialize();
            }
            else if (Presenter.CurrentViewModel is GameSystemSelectionViewModel)
            {
                var currentSystemSelectionVM = Presenter.CurrentViewModel as GameSystemSelectionViewModel;
                currentSystemSelectionVM.Prepare(file);
                await currentSystemSelectionVM.Initialize();
            }
            else
            {
                await NavigationService.Navigate<GameSystemSelectionViewModel, IFileInfo>(file);
            }
        }
    }
}
