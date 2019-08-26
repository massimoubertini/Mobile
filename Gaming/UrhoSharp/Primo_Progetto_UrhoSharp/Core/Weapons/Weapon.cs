﻿using System;
using System.Threading.Tasks;
using Urho;
using Urho.Physics;

namespace SamplyGame
{
    public abstract class Weapon : Component
    {
        private bool isInited;

        public DateTime LastLaunchDate { get; set; }

        // durata del ricaricamento (tra due lanci)
        public virtual TimeSpan ReloadDuration => TimeSpan.FromSeconds(0.5f);

        public virtual int Damage => 1;

        // l'arma è ricaricata
        public bool IsReloading => LastLaunchDate + ReloadDuration > DateTime.Now;

        public async Task<bool> FireAsync(bool byPlayer)
        {
            if (!isInited)
            {
                isInited = true;
                Init();
            }

            if (IsReloading)
                return false;
            LastLaunchDate = DateTime.Now;
            await OnFire(byPlayer);
            return true;
        }

        public virtual void OnHit(Aircraft target, bool killed, Node bulletNode)
        {
            var body = bulletNode.GetComponent<RigidBody>();
            if (body != null)
                body.Enabled = false;
            bulletNode.SetScale(0);
        }

        protected virtual void Init()
        {
        }

        protected Node CreateRigidBullet(bool byPlayer, Vector3 collisionBox)
        {
            var carrier = Node;
            var bullet = carrier.Scene.CreateChild(nameof(Weapon) + GetType().Name);
            var carrierPos = carrier.Position;
            bullet.Position = carrierPos;
            var body = bullet.CreateComponent<RigidBody>();
            CollisionShape shape = bullet.CreateComponent<CollisionShape>();
            shape.SetBox(collisionBox, Vector3.Zero, Quaternion.Identity);
            body.Kinematic = true;
            body.CollisionLayer = byPlayer ? (uint)CollisionLayers.Enemy : (uint)CollisionLayers.Player;
            bullet.AddComponent(new WeaponReferenceComponent(this));
            return bullet;
        }

        protected Node CreateRigidBullet(bool byPlayer)
        {
            return CreateRigidBullet(byPlayer, Vector3.One);
        }

        protected abstract Task OnFire(bool byPlayer);
    }

    // un componente che dev'essere collegato ai nodi dei proiettili per avere un collegamento all'arma
    public class WeaponReferenceComponent : Component
    {
        public Weapon Weapon { get; private set; }

        public WeaponReferenceComponent(Weapon weapon)
        {
            Weapon = weapon;
        }
    }
}