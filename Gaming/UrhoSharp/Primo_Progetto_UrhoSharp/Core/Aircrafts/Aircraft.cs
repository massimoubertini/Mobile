using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Audio;
using Urho.Physics;
using Urho.Urho2D;

namespace SamplyGame
{
    // una classe base per tutti gli aerei, inclusi il giocatore e i nemici
    public abstract class Aircraft : Component
    {
        private TaskCompletionSource<bool> liveTask;

        protected Aircraft()
        {
            ReceiveSceneUpdates = true;
        }

        // integrità corrente (minore o uguale a MaxHealth)
        public int Health { get; set; }

        // Max health value
        public virtual int MaxHealth => 30;

        // gli aerei sono vivi
        public bool IsAlive => Health > 0 && Enabled && !IsDeleted;

        // l'aereo attende fino a quando non è esploso
        public Task Play()
        {
            liveTask = new TaskCompletionSource<bool>();
            Health = MaxHealth;
            var node = Node;
            // gestione delle collisioni
            var body = node.CreateComponent<RigidBody>();
            body.Mass = 1;
            body.Kinematic = true;
            body.CollisionMask = (uint)CollisionLayer;
            CollisionShape shape = node.CreateComponent<CollisionShape>();
            shape.SetBox(CollisionShapeSize, Vector3.Zero, Quaternion.Identity);
            Init();
            node.SubscribeToNodeCollisionStart(OnCollided);
            return liveTask.Task;
        }

        // esplode l'aereo con l'animazione
        public async Task Explode()
        {
            Health = 0;
            // crea uno speciale nodo indipendente nella scena per l'esplosione
            var explosionNode = Scene.CreateChild();
            SoundSource soundSource = explosionNode.CreateComponent<SoundSource>();
            soundSource.Play(Application.ResourceCache.GetSound(Assets.Sounds.BigExplosion));
            soundSource.Gain = 0.5f;
            explosionNode.Position = Node.WorldPosition;
            OnExplode(explosionNode);
            var scaleAction = new ScaleTo(1f, 0f);
            Node.RemoveAllActions();
            Node.Enabled = false;
            await explosionNode.RunActionsAsync(scaleAction, new DelayTime(1f));
            liveTask.TrySetResult(true);
            explosionNode.Remove();
        }

        private void OnCollided(NodeCollisionStartEventArgs args)
        {
            var bulletNode = args.OtherNode;
            if (IsAlive && bulletNode.Name != null && bulletNode.Name.StartsWith(nameof(Weapon)) && args.Body.Node == Node)
            {
                var weapon = bulletNode.GetComponent<WeaponReferenceComponent>().Weapon;
                Health -= weapon.Damage;
                var killed = Health <= 0;
                if (killed)
                    Explode();
                else if (weapon.Damage > 0)
                    Hit();
                weapon.OnHit(target: this, killed: killed, bulletNode: bulletNode);
            }
        }

        private async void Hit()
        {
            var material = Node.GetComponent<StaticModel>().GetMaterial(0);
            if (material == null)
                return;
            // il materiale non dev'essere memorizzato nella cache (Clone dev'essere chiamato) o tutti gli oggetti con esso blinking
            material.SetShaderParameter("MatSpecColor", new Color(0, 0, 0, 0));
            var specColorAnimation = new ValueAnimation();
            specColorAnimation.SetKeyFrame(0.0f, new Color(1.0f, 1.0f, 1.0f, 0.5f));
            specColorAnimation.SetKeyFrame(0.1f, new Color(0, 0, 0, 0));
            material.SetShaderParameterAnimation("MatSpecColor", specColorAnimation, WrapMode.Once, 1.0f);
            await Node.RunActionsAsync(new DelayTime(1f));
        }

        protected virtual void OnExplode(Node explodeNode)
        {
            explodeNode.SetScale(2f);
            var particleEmitter = explodeNode.CreateComponent<ParticleEmitter2D>();
            particleEmitter.Effect = Application.ResourceCache.GetParticleEffect2D(Assets.Particles.Explosion);
        }

        protected virtual void Init()
        {
        }

        protected virtual CollisionLayers CollisionLayer => CollisionLayers.Enemy;

        protected virtual Vector3 CollisionShapeSize => new Vector3(1.2f, 1.2f, 1.2f);
    }

    public enum CollisionLayers : uint
    {
        Player = 2,
        Enemy = 4
    }
}