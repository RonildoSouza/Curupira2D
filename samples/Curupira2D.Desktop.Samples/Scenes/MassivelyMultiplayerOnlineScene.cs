using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.MassivelyMultiplayerOnline;
using WebSocketClient;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class MassivelyMultiplayerOnlineScene : SceneBase
    {
        internal string WSChannel => $"room_{nameof(MassivelyMultiplayerOnlineScene)}_001";
        internal WSClient WSClient { get; private set; }

        public MassivelyMultiplayerOnlineScene()
        {
            WSClient = new WSClient(WSChannel);
        }

        public override void LoadContent()
        {
            SetTitle(nameof(MassivelyMultiplayerOnlineScene));

            AddSystem<PlayerControllerSystem>();
            AddSystem<EnemiesControllerSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            WSClient.DisposeAsync().ConfigureAwait(true);
        }

        internal void WSSendMessage(string message) => WSClient?.Send(new Message(WSChannel, message));
    }
}
