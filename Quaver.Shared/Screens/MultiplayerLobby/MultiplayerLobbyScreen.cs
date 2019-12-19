using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Quaver.Server.Common.Enums;
using Quaver.Server.Common.Objects;
using Quaver.Server.Common.Objects.Multiplayer;
using Quaver.Shared.Discord;
using Quaver.Shared.Online;
using Quaver.Shared.Screens.Main;
using Wobble.Bindables;
using Wobble.Graphics.UI.Dialogs;
using Wobble.Input;

namespace Quaver.Shared.Screens.MultiplayerLobby
{
    public sealed class MultiplayerLobbyScreen : QuaverScreen
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override QuaverScreenType Type { get; } = QuaverScreenType.Lobby;

        /// <summary>
        ///    The currently visible multiplayer games
        /// </summary>
        public Bindable<List<MultiplayerGame>> VisibleGames { get; private set; }

        /// <summary>
        ///     The currently selected multiplayer game
        /// </summary>
        public Bindable<MultiplayerGame> SelectedGame { get; private set; }

        /// <summary>
        /// </summary>
        public MultiplayerLobbyScreen()
        {
            CreateBindableVisibleGames();
            CreateBindableSelectedGame();

            SetRichPresence();
            View = new MultiplayerLobbyScreenView(this);
            ScreenExiting += (sender, args) => OnlineManager.Client?.LeaveLobby();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void OnFirstUpdate()
        {
            OnlineManager.Client?.JoinLobby();
            base.OnFirstUpdate();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void Destroy()
        {
            VisibleGames.Dispose();
            SelectedGame.Dispose();

            base.Destroy();
        }

        /// <summary>
        /// </summary>
        private void HandleInput()
        {
            if (Exiting || DialogManager.Dialogs.Count != 0)
                return;

            if (KeyboardManager.IsUniqueKeyPress(Keys.Escape))
            {
                Exit(() => new MainMenuScreen());
                return;
            }
        }

        /// <summary>
        /// </summary>
        private void CreateBindableVisibleGames() => VisibleGames = new Bindable<List<MultiplayerGame>>(new List<MultiplayerGame>())
        {
            Value = new List<MultiplayerGame>()
        };

        /// <summary>
        /// </summary>
        private void CreateBindableSelectedGame() => SelectedGame = new Bindable<MultiplayerGame>(null);

        /// <summary>
        /// </summary>
        private void SetRichPresence()
        {
            DiscordHelper.Presence.Details = "Multiplayer Lobby";
            DiscordHelper.Presence.State = $"In the menus";
            DiscordRpc.UpdatePresence(ref DiscordHelper.Presence);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override UserClientStatus GetClientStatus() => new UserClientStatus(ClientStatus.InLobby, -1, "", 1, "", 0);
    }
}